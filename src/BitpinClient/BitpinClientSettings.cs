namespace Bitpin.Client;

public class BitpinClientSettings
{
    public string Key { get; init; } = null!;

    public string Secret { get; init; } = null!;

    public Uri ApiUrl { get; init; } = null!;
}
