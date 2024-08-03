using Microsoft.Extensions.Localization;
namespace ggolbik.csharp.i18n;

/// <summary>
/// This is just to have access to the CultureInfo is the StringLocalizer.
/// </summary>
public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
  /// <summary>
  /// Creates an JsonStringLocalizer
  /// </summary>
  /// <param name="resourceSource"> The System.Type</param>
  /// <returns></returns>
  public IStringLocalizer Create(Type resourceSource)
  {
    return new JsonStringLocalizer();
  }

  /// <summary>
  /// Creates an JsonStringLocalizer
  /// </summary>
  /// <param name="baseName">The base name of the resource to load strings from.</param>
  /// <param name="location">The location to load resources from.</param>
  /// <returns></returns>
  public IStringLocalizer Create(string baseName, string location)
  {
    return new JsonStringLocalizer();
  }
}