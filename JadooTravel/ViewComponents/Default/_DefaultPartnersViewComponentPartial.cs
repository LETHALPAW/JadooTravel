using JadooTravel.Services.LanguageService;
using JadooTravel.Services.PartnerServices;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.ViewComponents.Default
{
    public class _DefaultPartnersViewComponentPartial :ViewComponent
    {
        private readonly IPartnerService _partnerService;
        private readonly LanguageService _languageService;

        public _DefaultPartnersViewComponentPartial(IPartnerService partnerService, LanguageService languageService)
        {
            _partnerService = partnerService;
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lang = HttpContext.Session.GetString("lang") ?? "tr";

            var values = await _partnerService.GetAllPartnerAsync();
            return View(values);
        }
    }
}
