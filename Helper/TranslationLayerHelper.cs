using CodersCupAward.Services;

namespace CodersCupAward.Helper
{
    public class TranslationLayerHelper( ITextTranslatorService textTranslatorService) : ITranslationLayerHelper
    {
        
      
        public async Task<string> TranslateApplicationMessage(string message)
        {
            return await textTranslatorService.TranslateTextFromEnglishAsync(message, "es");
        }

        public async Task<string> TranslateValueAsync(string value, string languageCode)
        {
            return await textTranslatorService.TranslateTextFromEnglishAsync(value, languageCode);

        }

       
    }
}
