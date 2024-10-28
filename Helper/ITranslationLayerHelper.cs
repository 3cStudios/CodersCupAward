namespace CodersCupAward.Helper;

public interface ITranslationLayerHelper
{
    Task<string> TranslateApplicationMessage(string message);
    Task<string> TranslateValueAsync(string value, string languageCode);
}