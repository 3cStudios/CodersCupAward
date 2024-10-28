using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface ITextTranslatorService
{
    Task<AvailableLanguage?> AvailableLanguages();
    Task<string> TranslateTextAnyLanguageAsync(string inputText, string languageCodeFrom, string languageCodeTo);
    Task<string> TranslateTextFromEnglishAsync(string inputText, string languageCodeTo);
}