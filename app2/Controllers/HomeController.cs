using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace BasicOpenTelemetry.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController(ILogger<HomeController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("API got hit");
        return NotFound();
        return Ok();
    }

}


public static class DiagnosticsConfig
{
    //Resource name for Aspire Dashboard
    public const string ServiceName = "BasicOpenTelemetry.API";

    public static Meter Meter = new(ServiceName);

    //Metric to track the number of students
    public static Counter<int> StudentCounter = Meter.CreateCounter<int>("students.count");

}
