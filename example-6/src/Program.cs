using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using ggolbik.csharp.i18n;
using Microsoft.Extensions.Options;

namespace ggolbik.csharp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      Program.AddServices(builder);

      // Build the WebApplication.
      var app = builder.Build();

      // Configure the HTTP request pipeline.
      Program.ConfigurePipeline(app);

      // Run the WebApplication.
      app.Run();
    }

    public static void ConfigurePipeline(WebApplication app)
    {
      if (app.Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.Urls.Add("http://0.0.0.0:80");
      }

      // configure Localization
      var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
      app.UseRequestLocalization(localizationOptions.Value);

      Program.AddSwaggerMiddleware(app);

      app.UseAuthorization();

      // Adds endpoints for controller 
      app.MapControllers();
    }

    public static void AddSwaggerMiddleware(WebApplication app)
    {
      // insert middleware to expose the generated Swagger as JSON endpoint(s)
      app.UseSwagger();
      // insert the swagger-ui middleware if you want to expose interactive documentation, specifying the Swagger JSON endpoint(s) to power it from.
      app.UseSwaggerUI();
    }

    public static void AddServices(WebApplicationBuilder builder)
    {
      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      Program.AddLocalizationService(builder);
    }

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
  }
}