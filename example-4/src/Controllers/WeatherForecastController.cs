using ggolbik.csharp.i18n;
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
    private readonly ITranslationService _translationService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ITranslationService translationService)
    {
      _logger = logger;
      _translationService = translationService;
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

    
    /// <summary>
    /// Provides some sample data.
    /// </summary>
    /// <returns>Sample Data</returns>
    [Produces("application/json", "application/xml")]
    [HttpGet]
    [Route("Get1")]
    public IEnumerable<WeatherForecast> Get1()
    {
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }

    /// <summary>
    /// Provides some sample data.
    /// </summary>
    /// <returns>Sample Data</returns>
    [Produces("application/json", "application/xml")]
    [HttpGet]
    [Route("Get2")]
    public IEnumerable<WeatherForecast> Get2()
    {
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }

    /// <summary>
    /// Provides some sample data. This action is only available at the version "1.2-beta"
    /// </summary>
    /// <returns>Sample Data</returns>
    [Produces("application/json", "application/xml")]
    [HttpGet]
    [MapToApiVersion("1.2-beta")]
    [Route("Get3")]
    public IEnumerable<WeatherForecast> Get3()
    {
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }

    /// <summary>
    /// Provides an example exception. The exception will be only available if app.UseDeveloperExceptionPage() is enabled.
    /// </summary>
    /// <returns>Sample Data</returns>
    /// <response code="200">The 200 (OK) status code indicates that the request has succeeded. The payload sent in a 200 response depends on the request method.</response>
    /// <response code="500">The 500 (Internal Server Error) status code indicates that the server encountered an unexpected condition that prevented it from fulfilling the request.</response>

    [Produces("application/json", "application/xml")]
    [HttpGet]
    [Route("Get4")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public IEnumerable<WeatherForecast> Get4()
    {
      throw new Exception("Example exception");
    }

    /// <summary>
    /// Provides some sample data.
    /// </summary>
    /// <returns>Sample Data</returns>
    [Produces("application/json", "application/xml")]
    [HttpGet]
    [Route("Get5")]
    public ActionResult<IEnumerable<WeatherForecast>> Get5()
    {
      return StatusCode(StatusCodes.Status200OK,
        Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
          Date = DateTime.Now.AddDays(index),
          TemperatureC = Random.Shared.Next(-20, 55),
          Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray()
      );
    }

    /// <summary>
    /// Provides some localized string
    /// </summary>
    /// <returns>Sample Data</returns>
    [Produces("application/json", "application/xml")]
    [HttpGet]
    [Route("Get6")]
    public string Get6(string? username = null)
    {
      if(username == null) {
        return _translationService.GetHello();
      } else {
        return _translationService.GetHelloUser(username);
      }
    }
  }
}