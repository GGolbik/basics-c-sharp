using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ggolbik.csharp.swagger;

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
      app.UseSwaggerUI(options =>
      {
        // see https://swagger.io/docs/open-source-tools/swagger-ui/usage/configuration/ for more info about the UI configuration.
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var desc in provider.ApiVersionDescriptions)
        {
          // define the endpoints for the different API routes.
          options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString("'v'VVVV"));
          // Define whether the schemas of the API models should be shown. The value of -1 will hide the schemas.
          options.DefaultModelsExpandDepth(0);
          // Define whether the API groups should be expanded by default
          options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        }

        // Access swagger at route path
        options.RoutePrefix = string.Empty;
      });

    }

    public static void AddServices(WebApplicationBuilder builder)
    {
      builder.Services.AddControllers();

      // Add API Versioning to the project
      builder.Services.AddApiVersioning();

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

      // generate swagger
      builder.Services.AddSwaggerGen();
    }
  }
}