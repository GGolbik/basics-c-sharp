
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
    }
  }
}