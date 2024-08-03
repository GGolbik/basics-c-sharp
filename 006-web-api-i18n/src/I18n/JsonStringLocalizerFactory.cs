using Microsoft.Extensions.Localization;

namespace ggolbik.csharp.i18n
{
  /// <summary>
  /// This is just to have access to the CultureInfo is the StringLocalizer.
  /// </summary>
  public class JsonStringLocalizerFactory : IStringLocalizerFactory
  {
    public IStringLocalizer Create(Type resourceSource)
    {
      return new JsonStringLocalizer();
    }

    public IStringLocalizer Create(string baseName, string location)
    {
      return new JsonStringLocalizer();
    }
  }
}
