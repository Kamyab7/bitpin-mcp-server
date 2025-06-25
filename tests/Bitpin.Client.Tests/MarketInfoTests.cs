using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Bitpin.Client.Tests;

internal sealed class MarketInfoTests
{
    private ServiceProvider _serviceProvider;
    private BitpinClientService _service;

    [SetUp]
    public void Setup()
    {
        try
        {
            var serviceCollection = new ServiceCollection();

            // In a real environment, these would be loaded from environment variables or a secure configuration
            var settings = new BitpinClientSettings()
            {
                Key = Environment.GetEnvironmentVariable("BITPIN_API_KEY") ?? throw new ArgumentNullException("BITPIN_API_KEY cannot be null"),
                Secret = Environment.GetEnvironmentVariable("BITPIN_API_SECRET") ?? throw new ArgumentNullException("BITPIN_API_SECRET cannot be null"),
                ApiUrl = new Uri(Environment.GetEnvironmentVariable("BITPIN_API_URL") ?? "https://api.bitpin.org/api/v1/")
            };
            serviceCollection.AddBitpinClient(settings);

            _serviceProvider = serviceCollection.BuildServiceProvider();
            _service = _serviceProvider.GetRequiredService<BitpinClientService>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Test setup failed: {ex.Message}");
            throw;
        }
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider?.Dispose();
    }

    [Test]
    [Category("MarketData")]
    [TestCase(361)]
    public async Task ShouldReturnAllTheCurrenciesOnBitpin(int expectedCurrenciesCount)
    {
        var result = await _service.GetCurrenciesListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(expectedCurrenciesCount);
    }

    [Test]
    [Category("MarketData")]
    [TestCase(645)]
    public async Task ShouldReturnAllTheMarketsOnBitpin(int expectedMarketsCount)
    {
        var result = await _service.GetMarketsListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(expectedMarketsCount);
    }

    [Test]
    [Category("MarketData")]
    [TestCase(645)]
    public async Task ShouldReturnAllTheTickersOnBitpin(int expectedTickersCount)
    {
        var result = await _service.GetTickersListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(expectedTickersCount);
    }

    [Test]
    [Category("Authentication")]
    public async Task ShouldReturnToken()
    {
        var result = await _service.GetTokenAsync();

        result.ShouldNotBeNull();
        result.AccessToken.ShouldNotBeNullOrEmpty();
        result.RefreshToken.ShouldNotBeNullOrEmpty();
    }

    [Test]
    [Category("UserData")]
    public async Task ShouldReturnUserWallets()
    {
        var result = await _service.GetWalletsListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Test]
    [Category("MarketData")]
    [TestCase("BTC_IRT")]
    [TestCase("ETH_IRT", Ignore = "Optional test case")]
    public async Task ShouldReturnMatchesForASymbol(string symbol)
    {
        var result = await _service.GetMatchesListAsync(symbol);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Test]
    [Category("Authentication")]
    public async Task ShouldRefreshTheAccessToken()
    {
        var token = await _service.GetTokenAsync();
        var refreshedToken = await _service.RefreshTokenAsync();

        token.ShouldNotBeNull();
        refreshedToken.ShouldNotBeNull();
        token.AccessToken.ShouldNotBe(refreshedToken.AccessToken);
    }

    [Test]
    [Category("OrderManagement")]
    public async Task Should_Return_Completed_Orders()
    {
        var result = await _service.GetCompletedOrdersAsync();
        result.ShouldNotBeEmpty();
    }

    [Test]
    [Category("OrderManagement")]
    public async Task Should_Return_Pending_Orders()
    {
        var result = await _service.GetPendingOrdersAsync();
        result.ShouldBeEmpty();
    }

    [Test]
    [Category("OrderManagement")]
    public async Task ShouldReturnTheOrders()
    {
        var result = await _service.GetOrderListAsync();

        result.ShouldNotBeEmpty();
        result.Count().ShouldNotBe(0);
    }

    [Test]
    [Ignore("This test cancels a real order on the exchange and should only be run manually")]
    [Category("OrderManagement")]
    [TestCase(1)]
    public async Task ShouldCancelAPendingOrder(int orderId)
    {
        await _service.CancelOrderAsync(orderId);
    }

    [Test]
    [Category("OrderManagement")]
    [TestCase(1061895427)]
    public async Task ShouldReturnAnOrder(int orderId)
    {
        var result = await _service.GetOrderByIdAsync(orderId);
        result.ShouldNotBeNull();
    }

    [Test]
    [Ignore("This test creates a real limit order on the exchange and should only be run manually")]
    [Category("OrderCreation")]
    public async Task ShouldCreateLimitOrder()
    {
        var result = await _service.CreateLimitOrderAsync(new Bitpin.Client.Models.CreateLimitOrderRequest()
        {
            Symbol= "USDT_IRT",
            BaseAmount=1,
            Side="buy",
            Type="limit",
            Price=106868,
        });

        result.ShouldNotBeNull();
        result.State.ShouldBe("active");
    }

    [Test]
    [Ignore("This test creates a real market order on the exchange and should only be run manually")]
    [Category("OrderCreation")]
    public async Task ShouldCreateMarketOrder()
    {
        var result = await _service.CreateMarketOrderAsync(new Bitpin.Client.Models.CreateMarketOrderRequest()
        {
            Symbol = "USDT_IRT",
            BaseAmount = 1,
            Side = "buy",
            Type = "market",
        });

        result.ShouldNotBeNull();
        result.State.ShouldBe("active");
    }

    [Test]
    [Ignore("This test creates a real stop-limit order on the exchange and should only be run manually")]
    [Category("OrderCreation")]
    public async Task ShouldCreateStopLimitOrder()
    {
        var result = await _service.CreateStopLimitOrderAsync(new Bitpin.Client.Models.CreateStopLimitOrderRequest()
        {
            Symbol = "USDT_IRT",
            BaseAmount = 1,
            Side = "buy",
            Type = "stop_limit",
            Price = 106868,
            StopPrice=104000
        });

        result.ShouldNotBeNull();
        result.State.ShouldBe("initial");
    }

    [Test]
    [Ignore("This test creates a real OCO order on the exchange and should only be run manually")]
    [Category("OrderCreation")]
    public async Task ShouldCreateOcoOrder()
    {
        var result = await _service.CreateOcoOrderAsync(new Bitpin.Client.Models.CreateOcoOrderRequest()
        {
            Symbol = "USDT_IRT",
            BaseAmount = 1,
            Side = "sell",
            Type = "oco",
            OcoTargetPrice=110000,
            Price = 105000,
            StopPrice = 106000
        });

        result.ShouldNotBeNull();
        result.State.ShouldBe("initial");
    }

    [Test]
    [Category("MarketData")]
    [TestCase("BTC_IRT")]
    [TestCase("ETH_IRT", Ignore = "Optional test case")]
    public async Task ShouldReturnOrderbookForASymbol(string symbol)
    {
        var result = await _service.GetOrderbookAsync(symbol);

        result.ShouldNotBeNull();
        result.Asks.ShouldNotBeNull();
        result.Bids.ShouldNotBeNull();

        // Verify that both asks and bids contain arrays of price and amount
        foreach (var ask in result.Asks)
        {
            ask.Count.ShouldBe(2); // Checking there are exactly two items (price and amount)
            ask[0].ShouldNotBeNullOrEmpty(); // price
            ask[1].ShouldNotBeNullOrEmpty(); // amount
        }

        foreach (var bid in result.Bids)
        {
            bid.Count.ShouldBe(2); // Checking there are exactly two items (price and amount)
            bid[0].ShouldNotBeNullOrEmpty(); // price
            bid[1].ShouldNotBeNullOrEmpty(); // amount
        }
    }

    [Test]
    [Category("ErrorHandling")]
    public async Task ShouldHandleInvalidSymbolGracefully()
    {
        // Act
        Func<Task> act = async () => await _service.GetOrderbookAsync("INVALID_SYMBOL");

        // Assert
        var exception = await Should.ThrowAsync<Exception>(act);
        exception.Message.ShouldContain("*");
    }

    [Test]
    [Category("ErrorHandling")]
    public async Task ShouldHandleInvalidOrderIdGracefully()
    {
        // Act
        var result = await _service.GetOrderByIdAsync(-1);

        // Assert
        result.ShouldBeNull();
    }
}
