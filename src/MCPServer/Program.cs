using Bitpin.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


DotNetEnv.Env.TraversePath().Load();

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

var settings = new BitpinClientSettings()
{
    Key = Environment.GetEnvironmentVariable("BITPIN_API_KEY") ?? throw new ArgumentNullException("BITPIN_API_KEY cannot be null"),
    Secret = Environment.GetEnvironmentVariable("BITPIN_API_SECRET") ?? throw new ArgumentNullException("BITPIN_API_SECRET cannot be null"),
    ApiUrl = new Uri(Environment.GetEnvironmentVariable("BITPIN_API_URL") ?? "https://api.bitpin.org/api/v1/")
};

builder.Services.AddBitpinClient(settings);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

await app.RunAsync();
