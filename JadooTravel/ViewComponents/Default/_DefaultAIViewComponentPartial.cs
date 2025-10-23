using JadooTravel.Services.LanguageService;
using Microsoft.AspNetCore.Mvc;

namespace JadooTravel.ViewComponents.Default
{
    public class _DefaultAIViewComponentPartial : ViewComponent
    {
        private readonly LanguageService _languageService;

        public _DefaultAIViewComponentPartial(LanguageService languageService)
        {
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var lang = HttpContext.Session.GetString("lang") ?? "tr";

            var title = await _languageService.TranslateAsync("Yapay Zeka ile Rota Önerisi Al", lang);
            var placeholder = await _languageService.TranslateAsync("Ülke veya Şehir giriniz...", lang);
            var buttonText = await _languageService.TranslateAsync("Rota Oluştur", lang);
            var modalTitle = await _languageService.TranslateAsync("Yapay Zeka Rota Önerisi", lang);
            var loadingText = await _languageService.TranslateAsync("Yapay zeka önerisi hazırlanıyor...", lang);
            var resultPlaceholder = await _languageService.TranslateAsync("Sonuç burada görünecek...", lang);
           
            ViewData["CloseText"] = await _languageService.TranslateAsync("Kapat", lang);
            ViewData["Title"] = title;
            ViewData["Placeholder"] = placeholder;
            ViewData["ButtonText"] = buttonText;
            ViewData["ModalTitle"] = modalTitle;
            ViewData["LoadingText"] = loadingText;
            ViewData["ResultPlaceholder"] = resultPlaceholder;

            return View();
        }
    }
}
