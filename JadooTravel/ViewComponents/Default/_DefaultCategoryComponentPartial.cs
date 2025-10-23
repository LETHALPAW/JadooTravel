using JadooTravel.Services.CategoryServices;
using JadooTravel.Services.LanguageService;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace JadooTravel.ViewComponents.Default
{
    public class _DefaultCategoryComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly LanguageService _languageService;

        public _DefaultCategoryComponentPartial(ICategoryService categoryService, LanguageService languageService)
        {
            _categoryService = categoryService;
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lang = HttpContext.Session.GetString("lang") ?? "tr";
            var categories = await _categoryService.GetAllCategoryAsync();

            if (lang != "tr" && categories != null && categories.Any())
            {
                foreach (var item in categories)
                {
                    var props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(string))
                        {
                            var propName = prop.Name.ToLower();
                            var value = prop.GetValue(item)?.ToString();

                            // 🔹 URL veya görsel alanları çevirmiyoruz
                            if (string.IsNullOrWhiteSpace(value) ||
                                propName.Contains("image") ||
                                propName.Contains("icon") ||
                                propName.Contains("url") ||
                                propName.Contains("video"))
                            {
                                continue;
                            }

                            // 🔹 HTML tag veya bağlantı içeren metinleri de çevirmiyoruz
                            if (value.Contains("<") || value.Contains("src=") || value.Contains("href="))
                            {
                                continue;
                            }

                            // 🔹 Sadece düz metinleri çevir
                            var translatedValue = await _languageService.TranslateAsync(value, lang);
                            prop.SetValue(item, translatedValue);
                        }
                    }
                }
            }

            return View(categories);
        }
    }
}
