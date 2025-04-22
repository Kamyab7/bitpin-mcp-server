using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace BitpinClient.Tests;

public class MarketInfoTests
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
                Key = "****",
                Secret = "****",
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
    public async Task Should_Return_All_The_Currencies_on_Bitpin(int expectedCurrenciesCount)
    {
        var result = await _service.GetCurrenciesListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(expectedCurrenciesCount);
    }

    [Test]
    [Category("MarketData")]
    [TestCase(645)]
    public async Task Should_Return_All_The_Markets_on_Bitpin(int expectedMarketsCount)
    {
        var result = await _service.GetMarketsListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(expectedMarketsCount);
    }

    [Test]
    [Category("MarketData")]
    [TestCase(645)]
    public async Task Should_Return_All_The_Tickers_on_Bitpin(int expectedTickersCount)
    {
        var result = await _service.GetTickersListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(expectedTickersCount);
    }

    [Test]
    [Category("Authentication")]
    public async Task Should_Return_Token()
    {
        var result = await _service.GetTokenAsync();

        result.ShouldNotBeNull();
        result.AccessToken.ShouldNotBeNullOrEmpty();
        result.RefreshToken.ShouldNotBeNullOrEmpty();
    }

    [Test]
    [Category("UserData")]
    public async Task Should_Return_User_Wallets()
    {
        var result = await _service.GetWalletsListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Test]
    [Category("MarketData")]
    [TestCase("BTC_IRT")]
    [TestCase("ETH_IRT", Ignore = "Optional test case")]
    public async Task Should_Return_Matches_For_A_Symbol(string symbol)
    {
        var result = await _service.GetMatchesListAsync(symbol);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Test]
    [Category("Authentication")]
    public async Task Should_Refresh_The_Access_Token()
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
    public async Task Should_Return_The_Orders()
    {
        var result = await _service.GetOrderListAsync();

        result.ShouldNotBeEmpty();
        result.Count().ShouldNotBe(0);
    }

    [Test]
    [Ignore("This test cancels a real order on the exchange and should only be run manually")]
    [Category("OrderManagement")]
    [TestCase(1)]
    public async Task Should_Cancel_A_Pending_Order(int orderId)
    {
        await _service.CancelOrderAsync(orderId);
    }

    [Test]
    [Category("OrderManagement")]
    [TestCase(1061895427)]
    public async Task Should_Return_An_Order(int orderId)
    {
        var result = await _service.GetOrderByIdAsync(orderId);
        result.ShouldNotBeNull();
    }

    [Test]
    [Ignore("This test creates a real limit order on the exchange and should only be run manually")]
    [Category("OrderCreation")]
    public async Task Should_Create_A_Limit_Order()
    {
        var result = await _service.CreateLimitOrderAsync(new Models.CreateLimitOrderRequest()
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
    public async Task Should_Create_A_Market_Order()
    {
        var result = await _service.CreateMarketOrderAsync(new Models.CreateMarketOrderRequest()
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
    public async Task Should_Create_A_StopLimit_Order()
    {
        var result = await _service.CreateStopLimitOrderAsync(new Models.CreateStopLimitOrderRequest()
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
    public async Task Should_Create_An_Oco_Order()
    {
        var result = await _service.CreateOcoOrderAsync(new Models.CreateOcoOrderRequest()
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
    public async Task Should_Return_Orderbook_For_A_Symbol(string symbol)
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
    public async Task Should_Handle_Invalid_Symbol_Gracefully()
    {
        // Act
        Func<Task> act = async () => await _service.GetOrderbookAsync("INVALID_SYMBOL");

        // Assert
        var exception = await Should.ThrowAsync<Exception>(act);
        exception.Message.ShouldContain("*");
    }

    [Test]
    [Category("ErrorHandling")]
    public async Task Should_Handle_Invalid_Order_Id_Gracefully()
    {
        // Act
        var result = await _service.GetOrderByIdAsync(-1);

        // Assert
        result.ShouldBeNull();
    }
}
