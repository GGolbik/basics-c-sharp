namespace ggolbik.csharp
{

  public class WeatherForecast
  {
    /// <summary>
    /// The date time of the data point.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Temperature in Celsius 
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Temperature in Fahrenheit 
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// A summary of the content.
    /// </summary>
    public string? Summary { get; set; }
  }
}