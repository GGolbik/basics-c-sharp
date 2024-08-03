using System.Globalization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace ggolbik.csharp.i18n
{
  /// <summary>
  /// The logic to get the data from the JSON file
  /// </summary>
  public class JsonStringLocalizer : IStringLocalizer
  {
    List<JsonLocalization> localization = new List<JsonLocalization>();
    public JsonStringLocalizer()
    {
      //read all json file
      JsonSerializer serializer = new JsonSerializer();
      localization = JsonConvert.DeserializeObject<List<JsonLocalization>>(File.ReadAllText(@"Resources/i18n.json")) ?? new List<JsonLocalization>();
    }

    public LocalizedString this[string name]
    {
      get
      {
        var value = this.GetString(name);
        return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
      }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
      get
      {
        var format = this.GetString(name);
        var value = string.Format(format ?? name, arguments);
        return new LocalizedString(name, value, resourceNotFound: format == null);
      }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
      return localization.Where(l => l.LocalizedValue.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name)).Select(l => new LocalizedString(l.Key ?? "", l.LocalizedValue[CultureInfo.CurrentCulture.Name], true));
    }

    public IStringLocalizer WithCulture(CultureInfo culture)
    {
      return new JsonStringLocalizer();
    }

    private string? GetString(string name)
    {
      var query = localization.Where(l => l.LocalizedValue.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name));
      var value = query.FirstOrDefault(l => l.Key == name);
      if (value == null)
      {
        return null;
      }
      return value.LocalizedValue[CultureInfo.CurrentCulture.Name];
    }
  }
}
