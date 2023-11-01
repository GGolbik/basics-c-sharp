# Example 6 - ASP .NET Web-API - i18n

[ASP.NET Core localization with json files](https://stackoverflow.com/a/44782669)

# Table of Contents

* [Create](#create)
* [Add required packages](#add-required-packages)
* [Add your translations](#add-your-translations)
* [Add JsonLocalization Classes](#add-jsonlocalization-classes)
* [Add Service](#add-service)
* [Configure](#configure)
* [Add controller](#add-controller)
* [Access Data](#access-data)

# Create

Create a new console application project:
~~~
dotnet new webapi --no-https --framework net6.0 --name example-6 --output ./src
~~~

# Add required packages

https://www.nuget.org/packages/Microsoft.Extensions.Localization
~~~
dotnet add package Microsoft.Extensions.Localization
~~~

https://www.nuget.org/packages/Newtonsoft.Json/
~~~
dotnet add package Newtonsoft.Json
~~~

https://www.nuget.org/packages/Askmethat.Aspnet.JsonLocalizer/
~~~
dotnet add package Askmethat.Aspnet.JsonLocalizer
~~~

# Add your translations

Add new JSON File `Resources/i18n.json` to the project which holds the data:
~~~json
[
  {
    "Key": "hello",
    "LocalizedValue" : {
      "en-US": "Hello",
      "en": "Hello",
      "de-DE": "Hallo",
      "de": "Hallo"
    }
  },
  {
    "Key": "hellouser",
    "LocalizedValue" : {
      "en-US": "Hello {0}",
      "en": "Hello {0}",
      "de-DE": "Hallo {0}",
      "de": "Hallo {0}"
    }
  }
]
~~~

Add the following entry to the `*.csproj` file to add all files of resources folder.
~~~XML
<ItemGroup>
  <EmbeddedResource Include="Resources/**/*" />
</ItemGroup>
~~~

# Add JsonLocalization Classes

`I18n/JsonLocalization.cs`
~~~C#
namespace ggolbik.csharp.i18n
{
  class JsonLocalization
  {
    public string? Key { get; set; }
    public Dictionary<string, string> LocalizedValue = new Dictionary<string, string>();
  }
}
~~~

`I18n/JsonStringLocalizer.cs`
~~~C#
using System.Globalization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace ggolbik.csharp.i18n
{
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
~~~

`I18n/JsonStringLocalizerFactory.cs`
~~~C#
using Microsoft.Extensions.Localization;

namespace ggolbik.csharp.i18n
{
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
~~~

# Add Service

[Handle culture in route (URL) via RequestCultureProviders](https://stackoverflow.com/a/38170740)

~~~C#
public static void AddLocalizationService(WebApplicationBuilder builder)
{
  builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
  builder.Services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
  builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
  builder.Services.Configure<RequestLocalizationOptions>(options =>
  {
    CultureInfo[] supportedCultures = new[]
    {
      new CultureInfo("en-US"),
      new CultureInfo("en"),
      new CultureInfo("de-DE"),
      new CultureInfo("de")
    };
      // State what the default culture for your application is. This will be used if no specific culture can be determined for a given request.
    // The difference between culture and uiculture is that culture affects culture-dependent functions like money, date formatting and so on, while uiculture affects what resources are loaded for the application.
    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

    // You must explicitly state which cultures your application supports.
    // These are the cultures the app supports for formatting numbers, dates, etc.
    options.SupportedCultures = supportedCultures;

    // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
    options.SupportedUICultures = supportedCultures;

    // You can change which providers are configured to determine the culture for requests, or even add a custom
    // provider with your own logic. The providers will be asked in order to provide a culture for each request,
    // and the first to provide a non-null result that is in the configured supported cultures list will be used.
    // By default, the following built-in providers are configured:
    // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
    // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
    // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
    options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
  });
}
~~~

# Configure

~~~C#
// configure Localization
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);
~~~

# Add controller

Add `IStringLocalizer` to your controller:
~~~C#
private readonly IStringLocalizer<LocaleController> _localizer;

public LocaleController(ILogger<LocaleController> logger, IStringLocalizer<LocaleController> localizer)
{
  _logger = logger;
  _localizer = localizer;
}
~~~

# Access Data

Then you can use the `_localizer` to access your data:
~~~C#
[HttpGet(Name = "GetLocale")]
public String GetLocal()
{
  return _localizer["hellouser", "test"];
}
~~~