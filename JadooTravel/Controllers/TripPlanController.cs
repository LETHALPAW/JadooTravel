using JadooTravel.Dtos.TripPlanDtos;
using JadooTravel.Services.TripPlanServices;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.Controllers
{
	public class TripPlanController : Controller
	{
		private readonly ITripPlanService _tripPlanService;

		public TripPlanController(ITripPlanService tripPlanService)
		{
			_tripPlanService = tripPlanService;
		}

		public async Task<IActionResult> TripPlanList()
		{
			var values = await _tripPlanService.GetAllTripPlanAsync();
			return View(values);
		}

		[HttpGet]
		public async Task<IActionResult> UpdateTripPlan(string id)
		{
			var value = await _tripPlanService.GetTripPlanByIdAsync(id);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateTripPlan(UpdateTripPlanDto updateTripPlanDto)
		{
			if (!ModelState.IsValid)
				return View(updateTripPlanDto);

			await _tripPlanService.UpdateTripPlanAsync(updateTripPlanDto);
			return RedirectToAction("TripPlanList");
		}
	}
}
