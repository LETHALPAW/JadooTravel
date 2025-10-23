using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Text.RegularExpressions;

namespace JadooTravel.Services.LanguageService
{
    public class LanguageService
    {
        private readonly IOpenAIService _openAIService;

        public LanguageService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public async Task<string> TranslateAsync(string text, string targetLang)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

       
            if (targetLang.Equals("tr", StringComparison.OrdinalIgnoreCase))
                return text;

            //if (Regex.IsMatch(text, @"<[^>]+>") || text.Contains("src=") || text.Contains("href=") || text.Contains("/public/"))
            //    return text;


            string langName = targetLang switch
            {
                "en" => "English",
                "fr" => "French",
                "es" => "Spanish",
                "tr" => "Turkish",
                _ => "English"
            };

            try
            {
                var response = await _openAIService.ChatCompletion.CreateCompletion(
                    new ChatCompletionCreateRequest
                    {
                        Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
                        Messages = new List<ChatMessage>
                        {
                            ChatMessage.FromSystem($"You are a professional translator. Translate the following text to {langName}. Keep the formatting and tone."),
                            ChatMessage.FromUser(text)
                        },
                        MaxTokens = 400,
                        Temperature = 0
                    });

                if (response.Successful)
                    return response.Choices.First().Message.Content.Trim();
            }
            catch
            {
                
            }

            return text;
        }
    }
}
