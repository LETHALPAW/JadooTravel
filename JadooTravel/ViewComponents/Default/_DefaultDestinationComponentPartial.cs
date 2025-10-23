using JadooTravel.Services.DestinationServices;
using JadooTravel.Services.LanguageService;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace JadooTravel.ViewComponents.Default
{
    public class _DefaultDestinationComponentPartial : ViewComponent
    {
        private readonly IDestinationService _destinationService;
        private readonly LanguageService _languageService;

        public _DefaultDestinationComponentPartial(IDestinationService destinationService, LanguageService languageService)
        {
            _destinationService = destinationService;
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lang = HttpContext.Session.GetString("lang") ?? "tr";
            var values = await _destinationService.GetAllDestinationAsync();

            if (lang != "tr" && values != null && values.Any())
            {
                foreach (var item in values)
                {
                    var props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType != typeof(string)) continue;

                        var propName = prop.Name.ToLower();
                        var value = prop.GetValue(item)?.ToString();

                        if (string.IsNullOrWhiteSpace(value)) continue;

                        // 🔹 Görsel/URL alanlarını atla
                        if (propName.Contains("image") || propName.Contains("url") ||
                            propName.Contains("icon") || propName.Contains("video"))
                            continue;

                        // 🔹 HTML içerikleri atla
                        if (value.TrimStart().StartsWith("<"))
                            continue;

                        try
                        {
                            string translatedValue = string.Empty;

                            if (propName == "citycountry")
                            {
                                // 🔹 Tüm olası ayraçları tanımla
                                var separators = new[] { '/', ',', '-', '|', '&' };

                                // 🔹 Değeri ayraçlara göre böl
                                var parts = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                // 🔹 Şehir ve ülke ayır
                                var city = parts.Length >= 1 ? parts[0].Trim() : value;
                                var country = parts.Length >= 2 ? parts[^1].Trim() : "";

                                // 🔹 Çevirileri yap
                                var translatedCity = await _languageService.TranslateAsync(city, lang);
                                var translatedCountry = !string.IsNullOrWhiteSpace(country)
                                    ? await _languageService.TranslateAsync(country, lang)
                                    : "";

                                // 🔹 Birleştir (orijinal ayırıcıyla benzer görünüm için)
                                translatedValue = $"{translatedCity} & {translatedCountry}";
                            }
                            else
                            {
                                translatedValue = await _languageService.TranslateAsync(value, lang);
                            }

                            // 🔹 Log (test amaçlı)
                            Console.WriteLine($"[Çeviri] {propName}: {value} -> {translatedValue}");

                            if (!string.IsNullOrWhiteSpace(translatedValue) && translatedValue != value)
                                prop.SetValue(item, translatedValue);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Çeviri hatası: {propName} -> {ex.Message}");
                        }
                    }
                }
            }

            return View(values);
        }

    }
}
