using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ggolbik.csharp.swagger;
using ggolbik.csharp.i18n;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace ggolbik.csharp
{
    // https://stackoverflow.com/questions/59756374/configuring-apiversiondescriptions-in-netcore-3-0-without-using-build-service-p
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

        /// <summary>
        /// Configures the middleware pipeline.
        /// WebApplicationBuilder configures a middleware pipeline that wraps middleware with UseRouting and UseEndpoints.
        /// However, apps can change the order in which UseRouting and UseEndpoints run by calling these methods explicitly.
        /// <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order">Middleware order</see>
        /// </summary>
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

            //  UseRequestLocalization must appear before any middleware that might check the request culture.
            var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            // use index files as default if no file is specified e.g. index.html
            app.UseDefaultFiles();

            // provide static files from the wwwroot folder
            app.UseStaticFiles();

            // add swagger doc
            Program.UseSwaggerMiddleware(app);

            // enable routing
            app.UseRouting();

            // Adds endpoints for controller 
            app.MapControllers();
        }

        public static void UseSwaggerMiddleware(WebApplication app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api/doc/{documentName}/doc.json";
                /*
                // Force URL to lowercase in swagger
                options.PreSerializeFilters.Add((document, request) =>
                {
                  var paths = document.Paths.ToDictionary(item => item.Key.ToLowerInvariant(), item => item.Value);
                  document.Paths.Clear();
                  foreach (var pathItem in paths)
                  {
                    document.Paths.Add(pathItem.Key, pathItem.Value);
                  }
                });
                */
            });
            // insert the swagger-ui middleware if you want to expose interactive documentation, specifying the Swagger JSON endpoint(s) to power it from.
            app.UseSwaggerUI(options =>
            {
                // Access swagger at route path
                options.RoutePrefix = "api/doc";

                options.InjectStylesheet("/css/swagger-mycustom.css");

                options.InjectJavascript("/js/swagger-custom.js", "text/javascript");

                // see https://swagger.io/docs/open-source-tools/swagger-ui/usage/configuration/ for more info about the UI configuration.
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    // define the endpoints for the different API routes.
                    options.SwaggerEndpoint($"/api/doc/{desc.GroupName}/doc.json", desc.ApiVersion.ToString("'v'VVV"));
                    // Define whether the schemas of the API models should be shown. The value of -1 will hide the schemas.
                    options.DefaultModelsExpandDepth(0);
                    // Define whether the API groups should be expanded by default
                    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                }
            });

            /*
            options.SupportedSubmitMethods(new Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod[] {
                //Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod.Get
            });
            */
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            // Add API Versioning to the project
            builder.Services.AddApiVersioning(options =>
            {
                // Advertise the API versions supported for the particular endpoint
                options.ReportApiVersions = true;
                // Specify the default API Version
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            // add api explorer for swagger
            builder.Services.AddVersionedApiExplorer(setup =>
            {
                // see https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Format#custom-api-version-format-strings for more info about the format.
                // this option is only necessary when versioning by url segment.
                //setup.GroupNameFormat = "'v'VVV"; // the specified format code will format the version as "'v'major[.minor][-status]"
                setup.SubstituteApiVersionInUrl = true;
            });

            // configure swagger gen options
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // add service to generate swagger doc
            builder.Services.AddSwaggerGen(options =>
            {
                // customize type name of Schemas
                options.CustomSchemaIds(type => type.ToString());
                options.SupportNonNullableReferenceTypes();
            });

            // add i18n services
            builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            builder.Services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
            builder.Services.AddLocalization(options => {
                // specify where to look for the JSON resource files.
                options.ResourcesPath = Path.Combine(AppContext.BaseDirectory, "Resources");
            });
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
            builder.Services.AddSingleton<ITranslationService, TranslationService>();
        }
    }
}