using JadooTravel.Services.LanguageService;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.ViewComponents.Default
{
    public class _DefaultNavbarComponentPartial : ViewComponent
    {
        private readonly LanguageService _languageService;

        public _DefaultNavbarComponentPartial(LanguageService languageService)
        {
            _languageService = languageService;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
