using ggolbik.csharp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ggolbik.csharp.controllers
{

  [ApiController]
  [Route("Locale")]
  public class LocaleController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<LocaleController> _logger;
    private readonly IStringLocalizer<LocaleController> _localizer;

    public LocaleController(ILogger<LocaleController> logger, IStringLocalizer<LocaleController> localizer)
    {
      _logger = logger;
      _localizer = localizer;
    }

    [HttpGet(Name = "GetLocale")]
    public String GetLocal()
    {
      return _localizer["hellouser", "test"];
    }

  }
}