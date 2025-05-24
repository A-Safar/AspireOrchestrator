using BasicOpenTelemetry.Controllers;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
Uri openTelemetryUri = new Uri("http://localhost:4317");
var openTelemetryConfig = !string.IsNullOrEmpty(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
if (openTelemetryConfig)
{
    openTelemetryUri = new Uri(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
}

builder.Services.AddOpenTelemetry()
    .ConfigureResource(res => res
        .AddService(DiagnosticsConfig.ServiceName))
    .WithMetrics(metrics =>
    {
        metrics
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation();

        metrics.AddMeter(DiagnosticsConfig.Meter.Name);

        metrics.AddOtlpExporter(opt => opt.Endpoint = openTelemetryUri);
    })
    .WithTracing(tracing =>
    {

        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation();

        tracing.AddOtlpExporter(opt => opt.Endpoint = openTelemetryUri);

    }
    );

builder.Logging.AddOpenTelemetry(log =>
{
    log.AddOtlpExporter(opt => opt.Endpoint = openTelemetryUri);
    log.IncludeScopes = true;
    log.IncludeFormattedMessage = true;
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
