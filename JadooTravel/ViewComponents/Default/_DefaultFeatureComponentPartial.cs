using JadooTravel.Services.FeatureServices;
using JadooTravel.Services.LanguageService;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace JadooTravel.ViewComponents.Default
{
    public class _DefaultFeatureComponentPartial : ViewComponent
    {
        private readonly IFeatureService _featureService;
        private readonly LanguageService _languageService;

        public _DefaultFeatureComponentPartial(IFeatureService featureService, LanguageService languageService)
        {
            _featureService = featureService;
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var lang = HttpContext.Session.GetString("lang") ?? "tr";

      
            var values = await _featureService.GetFeatureAsync();

 
            if (lang != "tr" && values != null)
            {
                var props = values.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        var value = prop.GetValue(values)?.ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            var translatedValue = await _languageService.TranslateAsync(value, lang);
                            prop.SetValue(values, translatedValue);
                        }
                    }
                }
            }

            return View(values);
        }
    }
}
