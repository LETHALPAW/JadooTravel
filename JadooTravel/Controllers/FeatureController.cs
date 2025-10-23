using JadooTravel.Dtos.FeatureDtos;
using JadooTravel.Services.FeatureServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.Controllers
{

	public class FeatureController : Controller
	{
		private readonly IFeatureService _featureService;

		public FeatureController(IFeatureService featureService)
		{
			_featureService = featureService;
		}

		public async Task<IActionResult> Index()
		{
			var feature = await _featureService.GetFeatureAsync();
			return View(feature);
		}

		[HttpGet]
		public IActionResult CreateFeature()
		{
			return View(new CreateFeatureDto());
		}

		[HttpPost]
		public async Task<IActionResult> CreateFeature(CreateFeatureDto dto)
		{

			await _featureService.CreateFeatureAsync(dto);
			return RedirectToAction("Index");

		}

		[HttpGet]
		public async Task<IActionResult> UpdateFeature(string id)
		{
			var value = await _featureService.GetFeatureByIdAsync(id);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
		{
			await _featureService.UpdateFeatureAsync(updateFeatureDto);
			return RedirectToAction("Index");
		}
	}
}
