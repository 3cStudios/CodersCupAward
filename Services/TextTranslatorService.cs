using System.Text;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Newtonsoft.Json;

namespace CodersCupAward.Services
{
    public class TextTranslatorService : ITextTranslatorService
    {
        private readonly IConfiguration _configuration;

        public TextTranslatorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AvailableLanguage?> AvailableLanguages()
        {
            const string route = "/languages?api-version=3.0&scope=translation";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(Constants.CognitiveServicesEndpoint + route);
                request.Headers.Add(Constants.CognitiveServicesHeaderKey, _configuration["Elev8.FRS.CognitiveServices.Key"]);
                // location required if you're using a multi-service or regional (not global) resource.
                request.Headers.Add(Constants.CognitiveServicesHeaderRegion, _configuration["Elev8.FRS.CognitiveServices.Location"]);

                // Send the request and get response.
                var response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AvailableLanguage>(result);

            }
        }

        public async Task<string> TranslateTextAnyLanguageAsync(string inputText, string languageCodeFrom, string languageCodeTo)
        {
            var body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);
            var route = $"/translate?api-version=3.0&from={languageCodeFrom}&to={languageCodeTo}";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(Constants.CognitiveServicesEndpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add(Constants.CognitiveServicesHeaderKey, _configuration["Elev8.FRS.CognitiveServices.Key"]);
                // location required if you're using a multi-service or regional (not global) resource.
                request.Headers.Add(Constants.CognitiveServicesHeaderRegion, _configuration["Elev8.FRS.CognitiveServices.Location"]);

                // Send the request and get response.
                var response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                var result = await response.Content.ReadAsStringAsync();
                var translatedText = JsonConvert.DeserializeObject<List<TextTranslation>>(result) ?? new List<TextTranslation>();
                return TranslatedMessageText(translatedText);
            }
        }
        public async Task<string> TranslateTextFromEnglishAsync(string inputText, string languageCodeTo)
        {
            if (languageCodeTo == "en") return inputText;

            var body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);
            var route = $"/translate?api-version=3.0&textType=html&from=en&to={languageCodeTo}";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(Constants.CognitiveServicesEndpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add(Constants.CognitiveServicesHeaderKey, _configuration["Elev8.FRS.CognitiveServices.Key"]);
                // location required if you're using a multi-service or regional (not global) resource.
                request.Headers.Add(Constants.CognitiveServicesHeaderRegion, _configuration["Elev8.FRS.CognitiveServices.Location"]);

                // Send the request and get response.
                var response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                var result = await response.Content.ReadAsStringAsync();
                var translatedText = JsonConvert.DeserializeObject<List<TextTranslation>>(result) ?? new List<TextTranslation>();
                return TranslatedMessageText(translatedText);
            }
        }

        private string TranslatedMessageText(List<TextTranslation> textTranslations)
        {
            if (textTranslations.Count == 0) return string.Empty;
            return textTranslations[0].Translations.Count == 0 ? string.Empty : textTranslations[0].Translations[0].text;
        }
    }
}
