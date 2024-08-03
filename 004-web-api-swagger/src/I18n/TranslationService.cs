using Microsoft.Extensions.Localization;
namespace ggolbik.csharp.i18n;

public class TranslationService : ITranslationService
{
    private IStringLocalizer<TranslationService> _localizer;

    public TranslationService(IStringLocalizer<TranslationService> localizer)
    {
        this._localizer = localizer;
    }

    public string GetHello()
    {
        return _localizer["hello"];
    }

    public string GetHelloUser(string username)
    {
        return _localizer["hellouser", username];
    }

}