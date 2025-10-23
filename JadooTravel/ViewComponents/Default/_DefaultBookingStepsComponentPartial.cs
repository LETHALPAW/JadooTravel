using JadooTravel.Dtos.ReservationDtos;
using JadooTravel.Models;
using JadooTravel.Services.LanguageService;
using JadooTravel.Services.TripPlanServices;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace JadooTravel.ViewComponents.Default
{
    public class _DefaultBookingStepsComponentPartial : ViewComponent
    {
        private readonly ITripPlanService _tripPlanService;
        private readonly LanguageService _languageService;

        public _DefaultBookingStepsComponentPartial(ITripPlanService tripPlanService, LanguageService languageService)
        {
            _tripPlanService = tripPlanService;
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lang = HttpContext.Session.GetString("lang") ?? "tr";
            var tripPlans = await _tripPlanService.GetAllTripPlanAsync();

            if (lang != "tr" && tripPlans != null && tripPlans.Any())
            {
                foreach (var item in tripPlans)
                {
                    var props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(string))
                        {
                            var propName = prop.Name.ToLower();
                            var value = prop.GetValue(item)?.ToString();

                            // 🔹 URL veya medya alanlarını çevirme
                            if (string.IsNullOrWhiteSpace(value) ||
                                propName.Contains("image") ||
                                propName.Contains("icon") ||
                                propName.Contains("url") ||
                                propName.Contains("video"))
                            {
                                continue;
                            }

                            // 🔹 HTML veya tag içeren metinleri çevirme
                            if (value.Contains("<") || value.Contains("src=") || value.Contains("href="))
                            {
                                continue;
                            }

                            // 🔹 Gerçek metinleri çevir
                            var translatedValue = await _languageService.TranslateAsync(value, lang);
                            prop.SetValue(item, translatedValue);
                        }
                    }
                }
            }

            var model = new TripPlanReservationViewModel
            {
                TripPlans = tripPlans,
                Reservation = new CreateReservationDto()
            };

            return View(model);
        }
    }
}
