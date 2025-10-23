using JadooTravel.Services.DestinationServices;
using JadooTravel.Services.LanguageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        private readonly IDestinationService _destinationService;
        private readonly LanguageService _languageService;

        public DefaultController(IDestinationService destinationService, LanguageService languageService)
        {
            _destinationService = destinationService;
            _languageService = languageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDestinationDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Geçersiz id");

            var value = await _destinationService.GetDestinationByIdAsync(id);
            if (value == null)
                return NotFound();

            var lang = HttpContext.Session.GetString("lang") ?? "tr";

            try
            {
                if (lang != "tr")
                {
                    value.CityCountry = !string.IsNullOrWhiteSpace(value.CityCountry)
                        ? await _languageService.TranslateAsync(value.CityCountry, lang)
                        : value.CityCountry;

                    value.DayNight = !string.IsNullOrWhiteSpace(value.DayNight)
                        ? await _languageService.TranslateAsync(value.DayNight, lang)
                        : value.DayNight;

                    value.Description = !string.IsNullOrWhiteSpace(value.Description)
                        ? await _languageService.TranslateAsync(value.Description, lang)
                        : value.Description;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Çeviri hatası: {ex.Message}");
            }

            return Json(new
            {
                destinationId = value.DestinationId,
                cityCountry = value.CityCountry,
                imageUrl = value.ImageUrl,
                dayNight = value.DayNight,
                capacity = value.Capacity,
                price = value.Price,
                description = value.Description,
                status = value.Status
            });
        }

    }
}
