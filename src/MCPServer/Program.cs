using BitpinClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

var settings = new BitpinClientSettings()
{
    Key = "****",
    Secret = "****",
};

builder.Services.AddBitpinClient(settings);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

await app.RunAsync();