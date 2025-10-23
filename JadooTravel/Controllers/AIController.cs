using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace JadooTravel.Controllers
{
    [AllowAnonymous]
    public class AIController : Controller
    {
        private readonly IOpenAIService _openAIService;

        public AIController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAIRecommendation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return Json(new { success = false, message = "⚠️ Lütfen bir ülke veya şehir giriniz." });

            try
            {
                // 🧠 1) Metin tabanlı öneri al (ChatGPT)
                var completionResult = await _openAIService.ChatCompletion.CreateCompletion(
                    new ChatCompletionCreateRequest
                    {
                        Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
                        Messages = new List<ChatMessage>
                        {
                            ChatMessage.FromSystem("Sen deneyimli bir tur rehberisin. Kısa, açıklayıcı ve samimi rota önerileri ver."),
                            ChatMessage.FromUser($"Bana {location} için gezilecek yerler ve rota önerisi yaz. Liste şeklinde, kısa açıklamalarla yaz.")
                        },
                        MaxTokens = 400,
                        Temperature = 0.8F
                    });

                var textResponse = completionResult.Successful
                    ? completionResult.Choices.First().Message.Content
                    : $"❌ Hata: {completionResult.Error?.Message}";

                // 🖼️ 2) Görsel oluştur (DALL·E 3)
                var imageResult = await _openAIService.Image.CreateImage(new ImageCreateRequest
                {
                    Prompt = $"{location} şehrindeki popüler gezilecek yerlerin bir fotoğrafı, profesyonel seyahat fotoğrafı stili",
                    Size = StaticValues.ImageStatics.Size.Size1024
                });

                var imageUrl = imageResult.Successful
                    ? imageResult.Results.First().Url
                    : null;

                // 🎁 3) JSON olarak metin + görseli birlikte döndür
                return Json(new { success = true, text = textResponse, image = imageUrl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"❌ Bir hata oluştu: {ex.Message}" });
            }
        }
    }
}


