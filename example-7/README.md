# Example 7 - ASP .Net Web-API - Versioning - OpenAPI Swagger

# Table of Contents

* [Create](#create)
* [API Versioning](#api-versioning)
  * [ApiVersion Attribute](#apiversion-attribute)
  * [MapToApiVersion Attribute](#maptoapiversion-attribute)
  * [HTTP Header](#http-header)
  * [URL Path](#url-path)
* [Swagger](#swagger)
  * [API Documentation](#api-documentation)
  * [Swagger Path](#swagger-path)
  * [Disable Submit Buttons](#disable-submit-buttons)
* [Http Response Message](#http-response-message)


# Create

Create a new console application project:
~~~
dotnet new webapi --no-https --framework net6.0 --name example-7 --output ./src
~~~

# API Versioning

The API versioning extensions define simple metadata attributes and conventions that you use to describe which API versions are implemented by your services. You don't need to learn any new routing concepts or change the way you implement your services in ASP.NET today. The [wiki](https://github.com/Microsoft/aspnet-api-versioning/wiki) provides more information.

https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
~~~
dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
~~~

Add API Versioning to the project
~~~C#
services.AddApiVersioning(options =>
{
    // Advertise the API versions supported for the particular endpoint
    options.ReportApiVersions = true;
    // Specify the default API Version
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    // If the client hasn't specified the API version in the request, use the default API version number 
    options.AssumeDefaultVersionWhenUnspecified = true;
});
~~~

## ApiVersion Attribute

Add the attribute `ApiVersion` to the controller or methods. The method is than available at 
- http://localhost/api/weatherforecast.

You can define different version format strings (see [wiki](https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Format#custom-api-version-format-strings) and [ApiVersion Pattern](https://github.com/dotnet/aspnet-api-versioning/blob/master/src/Common/ApiVersion.cs#L27) `^(\d{4}-\d{2}-\d{2})?\.?(\d{0,9})\.?(\d{0,9})\.?-?(.*)$`).

~~~C#
[ApiController]
[ApiVersion("1", Deprecated = true)]
[ApiVersion("1.1")]
[ApiVersion("1.2-beta")]
[ApiVersion("2")]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
~~~

## MapToApiVersion Attribute

There's another important attribute named `MapToApiVersion`.
You can use it to map an action/method to a specific version.
The following code snippet shows how this can be accomplished.

~~~C#
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("1.1")]
[ApiVersion("2.0")]
[Route("api/[controller]")]
public class DefaultController : ControllerBase
{
  string[] authors = new string[]
  { "Joydip Kanjilal", "Steve Smith", "Stephen Jones" };
  [HttpGet]
  public IEnumerable<string> Get()
  {
      return authors;
  }
  [HttpGet("{id}")]
  [MapToApiVersion("2.0")]
  public string Get(int id)
  {
     return authors[id];
  }
}
~~~

## HTTP Header

There are alternative behaviors how to provide the API version
[wiki/API-Version-Reader](https://github.com/dotnet/aspnet-api-versioning/wiki/API-Version-Reader)
- URL Query String (QueryStringApiVersionReader): `https://...?v=2.0`
- Extended Header (HeaderApiVersionReader): `X-Api-Version`
- Media Type which can be set in `Content-Type` and `Accept` header (MediaTypeApiVersionReader): `application/json;v=2.0`

~~~C#
  /// <summary>
  /// Adds versioning to the API URLs
  /// </summary>
  /// <param name="builder">The web builder</param>
  private void AddApiVersioningService(WebApplicationBuilder builder)
  {
      builder.Services.AddApiVersioning(options =>
      {
          // Advertise the API versions supported for the particular endpoint.
          // It will add both api-supported-versions and api-deprecated-versions headers to our response.
          options.ReportApiVersions = true;
          // Specify the default API Version
          options.DefaultApiVersion = new ApiVersion(1, 0);
          // If the client hasn't specified the API version in the request, use the default API version number 
          options.AssumeDefaultVersionWhenUnspecified = true;
          // Finally, because we are going to support different versioning schemes, with the ApiVersionReader property, we combine different ways of reading the API version (from a query string, request header, and media type).
          options.ApiVersionReader = ApiVersionReader.Combine(
              new QueryStringApiVersionReader("api-version"),
              new HeaderApiVersionReader("api-version")
              //new MediaTypeApiVersionReader("api-version") // Header "Accept": "application/json;api-version=2.0"
          );
      });
      builder.Services.AddVersionedApiExplorer(setup =>
      {
          // see https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Format#custom-api-version-format-strings for more info about the format.
          // this option is only necessary when versioning by url segment.
          //setup.GroupNameFormat = "'v'VVV"; // the specified format code will format the version as "'v'major[.minor][-status]"
          setup.GroupNameFormat = "'v'VVVV";

          // SubstituteApiVersionInUrl is only necessary when versioning by the URI segment.
          setup.SubstituteApiVersionInUrl = true;
      });
  }
~~~

## URL Path

A disadvantage of the version in the URL is the fact, that the client service must always change the URLs.
A better solution is to insert the version in the HTTP header.

It is possible to add the api version in the URL.
Update `Route` attribute to the controller or methods.
The method is than available at 
- http://localhost/api/v1/weatherforecast.

~~~C#
[ApiController]
[ApiVersion("1", Deprecated = true)]
[ApiVersion("1.1")]
[ApiVersion("1.2-beta")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WeatherForecastController : ControllerBase
~~~

# Swagger

If not already defined, add [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
~~~
dotnet add package Swashbuckle.AspNetCore
~~~

Next, add the ApiExplorer service to the collection. You must set `SubstituteApiVersionInUrl` value to true, so that the placeholder `{version:apiVersion}` in the routes is automatically replaced by swagger. Otherwise the user has to enter the value manually.

The `AddEndpointsApiExplorer` can be replaced by the `AddVersionedApiExplorer`
~~~C#
builder.Services.AddVersionedApiExplorer(setup =>
{
    // see https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Format#custom-api-version-format-strings for more info about the format.
    // this option is only necessary when versioning by url segment.
    //setup.GroupNameFormat = "'v'VVV"; // the specified format code will format the version as "'v'major[.minor][-status]"

    setup.SubstituteApiVersionInUrl = true;
});
~~~

Create a new `ConfigureSwaggerOptions` class which is used to configure swagger to provide a page for each API version.

~~~C#
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ggolbik.csharp.swagger
{

  /// <summary>
  /// Configures the Swagger generation options.
  /// </summary>
  /// <remarks>This allows API versioning to define a Swagger document per API version after the
  /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
  public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
  {
    readonly IApiVersionDescriptionProvider provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
      // add a swagger document for each discovered API version
      // note: you might choose to skip or document deprecated API versions differently
      foreach (var description in provider.ApiVersionDescriptions)
      {
        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
      }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
      var info = new OpenApiInfo()
      {
        Title = "Sample API",
        Version = description.ApiVersion.ToString(),
        Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
        Contact = new OpenApiContact() { Name = "GGolbik", Email = "support@ggolbik.de" },
        License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
      };

      if (description.IsDeprecated)
      {
        info.Description += " <b>This API version has been deprecated.</b>";
      }

      return info;
    }
  }
}
~~~

Add the configuration class to the services:
~~~
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
~~~

Configure the UI documentation:
~~~C#
app.UseSwaggerUI(options =>
{
    // see https://swagger.io/docs/open-source-tools/swagger-ui/usage/configuration/ for more info about the UI configuration.
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var desc in provider.ApiVersionDescriptions)
    {
        // specifying the Swagger JSON endpoint.
        // define the endpoints for the different API routes.
        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString("'v'VVVV"));
        // Define whether the schemas of the API models should be shown. The value of -1 will hide the schemas.
        options.DefaultModelsExpandDepth(0);
        // Define whether the API groups should be expanded by default
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    }
});
~~~

## API Documentation

We need to add some additional entries to provide the API documentation (`summary`)

~~~C#
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    ...
    public void Configure(SwaggerGenOptions options)
    {
        ...
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    }
    ...
}
~~~

Add entry to `*.csproj` file to generate doc.
~~~XML
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
~~~

You can ignore missing documentation warningns by adding following entry to *.csproj file.
~~~XML
<PropertyGroup>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
~~~

## Swagger Path

You can define the path where swagger should be available with:
~~~C#
app.UseSwaggerUI(options =>
{
  // Access swagger at route path
  options.RoutePrefix = string.Empty;
}

~~~

## Disable Submit Buttons

~~~C#
app.UseSwaggerUI(options => {
    options.SupportedSubmitMethods(new Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod[] {});
});
~~~

The great thing about the way this is set up if you can allow some actions and not others. For example, say you wanted to allow get actions but disable the rest. The following would allow for that.

~~~C#
app.UseSwaggerUI(options =>
{
    options.SupportedSubmitMethods(new Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod[] { Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Get });
});
~~~

# Http Response Message

You can also create a custom HttpResponseMessage directly from an action method.

~~~C#
[HttpGet]
[Route("Get5")]
public ActionResult Get5()
{
  return StatusCode((int)HttpStatusCode.OK,
    Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = DateTime.Now.AddDays(index),
      TemperatureC = Random.Shared.Next(-20, 55),
      Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    }).ToArray()
  );
}
~~~

You can also use the built in types.
~~~C#
return Ok(/* body */);
return BadRequest(/* body */);
~~~
