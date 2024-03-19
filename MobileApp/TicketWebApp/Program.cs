using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using TicketClassLib.Services;
using TicketWebApp.Components;
using TicketWebApp.Data;
using TicketWebApp.Services;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(); // added this for part 2 hw
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddSingleton<ITicketService, ApiTicketService>();
builder.Services.AddSingleton<IEventService, ApiEventService>();
builder.Services.AddDbContextFactory<PostgresContext>(optionsBuilder => optionsBuilder.UseNpgsql("Name=TicketsDB"));
builder.Services.AddScoped<EmailSender>();

// for hw 2
const string serviceName = "message-api";
builder.Logging.AddOpenTelemetry(Options =>
{
    Options
        .SetResourceBuilder(
            ResourceBuilder
            .CreateDefault()
            .AddService(serviceName)
        )
        .AddConsoleExporter()
        .AddOtlpExporter(o =>
        {
            o.Endpoint = new Uri("http://otel-collector:4317/");
        });

});
// end 


builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
    .AddAspNetCoreInstrumentation()
    .AddSource(DiagnosticsConfiguration.SourceName1)
    .AddSource(DiagnosticsConfiguration.SourceName2)
    .AddConsoleExporter()
    .AddOtlpExporter(o =>
    {
        o.Endpoint = new Uri("http://otel-collector:4317/");
    })
    )
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddMeter("OpenTeleApi.Api")
        .AddConsoleExporter()
        .AddPrometheusHttpListener()
        .AddOtlpExporter(o =>
        {
            o.Endpoint = new Uri("http://otel-collector:4317/");
        }
        )
    );




builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1"); });

app.UseHttpsRedirection();

app.MapHealthChecks("/healthCheck", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
});

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();
//System.TypeLoadException: 'Could not load type 'System.ServiceProviderExtensions' from assembly 'OpenTelemetry, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = 7bd6737fe5b6
app.Run();

public partial class Program { }