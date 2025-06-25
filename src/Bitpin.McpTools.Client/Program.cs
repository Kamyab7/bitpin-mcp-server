using ModelContextProtocol.Client;

var clientTransport = new StdioClientTransport(new()
{
    Name = "Bitpin Mcp Server",
    Command = "dotnet",
    Arguments = ["run", "--project", "C:\\Users\\Kamyab\\Desktop\\bitpin-mcp-server\\src\\Bitpin.McpServer\\Bitpin.McpServer.csproj", "--no-build"],
    EnvironmentVariables = new Dictionary<string, string?> {
        {"BITPIN_API_KEY","Test" },
        {"BITPIN_API_SECRET","Test" },
        {"BITPIN_API_URL","https://api.bitpin.org/api/v1/" },
    }
});



await using var mcpClient = await McpClientFactory.CreateAsync(clientTransport);

var tools = await mcpClient.ListToolsAsync();
foreach (var tool in tools)
{
    Console.WriteLine($"Connected to server with tools: {tool.Name}");
}
