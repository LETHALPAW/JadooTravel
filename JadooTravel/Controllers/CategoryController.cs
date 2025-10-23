using JadooTravel.Dtos.CategoryDtos;
using JadooTravel.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JadooTravel.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public async Task<IActionResult> CategoryList()
		{
			var categories = await _categoryService.GetAllCategoryAsync();
			return View(categories);
		}

		[HttpGet]
		public IActionResult CreateCategory()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
		{
			if (!ModelState.IsValid)
			{
				return View(createCategoryDto);
			}

			await _categoryService.CreateCategoryAsync(createCategoryDto);
			return RedirectToAction("CategoryList");
		}

		public async Task<IActionResult> DeleteCategory(string id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest();

			await _categoryService.DeleteCategoryAsync(id);
			return RedirectToAction("CategoryList");
		}

		// GET: UpdateCategory
		[HttpGet]
		public async Task<IActionResult> UpdateCategory(string id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest();

			var value = await _categoryService.GetCategoryByIdAsync(id);
			if (value == null)
				return NotFound();

			// Map gelen modele View'un beklediği GetCategoryByIdDto'ya dönüştür.
			var model = MapProperties<GetCategoryByIdDto>(value);
			return View(model);
		}

		// POST: UpdateCategory
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
		{
			if (!ModelState.IsValid)
			{
				// Eğer POST sırasında model geçersizse, view aynı tipte bir modele ihtiyaç duyacaktır.
				// UpdateCategoryDto ile view'un kullandığı modeli aynı tutmuyorsan,
				// burada GetCategoryByIdDto'ya maplemeyi düşünebilirsin.
				// Basitçe yeniden view'a UpdateCategoryDto dönüyorum:
				return View(updateCategoryDto);
			}

			await _categoryService.UpdateCategoryAsync(updateCategoryDto);
			return RedirectToAction("CategoryList");
		}

		/// <summary>
		/// Basit reflection tabanlı eşleme: source'daki property'leri,
		/// hedef tipte aynı isimli property'lere (case-insensitive) kopyalar.
		/// Null-safety ve temel tip uyumluluğu sağlar.
		/// </summary>
		private TDest MapProperties<TDest>(object source) where TDest : new()
		{
			if (source == null) return default;

			var dest = new TDest();
			var srcType = source.GetType();
			var destType = typeof(TDest);

			var srcProps = srcType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var destProps = destType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
									.Where(p => p.CanWrite);

			foreach (var dprop in destProps)
			{
				// case-insensitive eşleme
				var sprop = srcProps.FirstOrDefault(sp => string.Equals(sp.Name, dprop.Name, StringComparison.OrdinalIgnoreCase));
				if (sprop == null) continue;

				try
				{
					var sVal = sprop.GetValue(source);
					if (sVal == null)
					{
						dprop.SetValue(dest, null);
						continue;
					}

					// Eğer tipler uyuyorsa direkt ata
					if (dprop.PropertyType.IsAssignableFrom(sprop.PropertyType))
					{
						dprop.SetValue(dest, sVal);
						continue;
					}

					// Basit dönüştürme denemesi (ör. int -> string, string -> int vb.)
					try
					{
						var converted = Convert.ChangeType(sVal, Nullable.GetUnderlyingType(dprop.PropertyType) ?? dprop.PropertyType);
						dprop.SetValue(dest, converted);
					}
					catch
					{
						// dönüştürülemediyse atlama
					}
				}
				catch
				{
					// bir property set işlemi hata verirse atla
					continue;
				}
			}

			return dest;
		}
	}
}
