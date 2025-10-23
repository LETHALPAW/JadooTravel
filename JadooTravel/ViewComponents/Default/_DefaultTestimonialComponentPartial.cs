using JadooTravel.Services.CategoryServices;
using JadooTravel.Services.LanguageService;
using JadooTravel.Services.TestimonialServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using System.Reflection;

namespace JadooTravel.ViewComponents.Default
{
    public class _DefaultTestimonialComponentPartial : ViewComponent
    {

        private readonly ITestimonialService  _testimonialService;
        private readonly LanguageService _languageService;

        public _DefaultTestimonialComponentPartial(ITestimonialService testimonialService, LanguageService languageService)
        {
            _testimonialService = testimonialService;
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lang = HttpContext.Session.GetString("lang") ?? "tr";
            var values = await _testimonialService.GetAllTestimonialAsync();

            if (lang != "tr" && values != null && values.Any())
            {
                foreach (var item in values)
                {
                    var props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(string))
                        {
                            var value = prop.GetValue(item)?.ToString();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var translatedValue = await _languageService.TranslateAsync(value, lang);
                                prop.SetValue(item, translatedValue);
                            }
                        }
                    }
                }
            }


            return View(values);
        }
       
    }
}
