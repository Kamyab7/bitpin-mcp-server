using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry;
using BitpinSseServer.Tools;
using BitpinClient;

DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport()    
    .WithTools<BitpinExchangeTools>();

builder.Services.AddOpenTelemetry()
    .WithTracing(b => b.AddSource("*")
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .WithMetrics(b => b.AddMeter("*")
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .WithLogging()
    .UseOtlpExporter();

var settings = new BitpinClientSettings()
{
    Key = Environment.GetEnvironmentVariable("BITPIN_API_KEY") ?? throw new ArgumentNullException("BITPIN_API_KEY cannot be null"),
    Secret = Environment.GetEnvironmentVariable("BITPIN_API_SECRET") ?? throw new ArgumentNullException("BITPIN_API_SECRET cannot be null"),
    ApiUrl = new Uri(Environment.GetEnvironmentVariable("BITPIN_API_URL") ?? "https://api.bitpin.org/api/v1/")
};

builder.Services.AddBitpinClient(settings);

var app = builder.Build();

app.MapMcp();

await app.RunAsync();
