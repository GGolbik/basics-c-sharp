using ggolbik.csharp.model;
using Microsoft.AspNetCore.Mvc;

namespace ggolbik.csharp.controllers
{

  [ApiController]
  [ApiVersion("1", Deprecated = true)]
  [ApiVersion("1.1")]
  [ApiVersion("1.2-beta")]
  //[ApiVersion("2022-04-01")] // It's unknown how this can be provided by URL path. See https://github.com/domaindrivendev/Swashbuckle.AspNetCore
  //[ApiVersion("2022-04-01-beta")] // It's unknown how this can be provided by URL path. See https://github.com/domaindrivendev/Swashbuckle.AspNetCore
  //[ApiVersion("2022-04-01.1.0-beta")] // It's unknown how this can be provided by URL path. See https://github.com/domaindrivendev/Swashbuckle.AspNetCore
  [ApiVersion("2")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
      _logger = logger;
    }

    /// <summary>
    /// Provides some sample data.
    /// </summary>
    /// <returns>Sample Data</returns>
    [Produces("application/json", "application/xml")]
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }
  }
}