using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace BitpinClient;

public static class DependencyInjection
{
    public static IServiceCollection AddBitpinClient(this IServiceCollection services, BitpinClientSettings settings)
    {
        if (settings.Key is null)
        {
            throw new ArgumentNullException($"{nameof(settings.Key)} cannot be null");
        }

        if (settings.Secret is null)
        {
            throw new ArgumentNullException($"{nameof(settings.Secret)} cannot be null");
        }

        services.AddHttpClient<BitpinClientService>()
            .AddStandardResilienceHandler();

        services.AddResiliencePipeline("default", x =>
        {
            x.AddRetry(new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                Delay = TimeSpan.FromSeconds(3),
                MaxRetryAttempts = 15,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
            })
            .AddTimeout(TimeSpan.FromSeconds(10));
        });

        services.AddSingleton<BitpinClientService>();

        services.AddSingleton(settings);

        return services;
    }
}
