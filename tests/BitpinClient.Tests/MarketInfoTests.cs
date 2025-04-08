using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace BitpinClient.Tests;

public class Tests
{
    private ServiceProvider _serviceProvider;
    private BitpinClientService _service;

    [SetUp]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        var settings = new BitpinClientSettings()
        {
            Key = "****",
            Secret = "****",
        };

        serviceCollection.AddBitpinClient(settings);

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _service = _serviceProvider.GetRequiredService<BitpinClientService>();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider?.Dispose();
    }

    [Test]
    [TestCase(361)]
    public async Task Should_Return_All_The_Currencies_on_Bitpin(int expectedCurrenciesCount)
    {
        var result = await _service.GetCurrenciesListAsync();

        result.Should().NotBeNullOrEmpty();
        result.Count().Should().Be(expectedCurrenciesCount);
    }

    [Test]
    [TestCase(645)]
    public async Task Should_Return_All_The_Markets_on_Bitpin(int expectedMarketsCount)
    {
        var result = await _service.GetMarketsListAsync();

        result.Should().NotBeNullOrEmpty();
        result.Count().Should().Be(expectedMarketsCount);
    }

    [Test]
    [TestCase(645)]
    public async Task Should_Return_All_The_Tickers_on_Bitpin(int expectedTickersCount)
    {
        var result = await _service.GetTickersListAsync();

        result.Should().NotBeNullOrEmpty();
        result.Count().Should().Be(expectedTickersCount);
    }

    [Test]
    public async Task Should_Return_Token()
    {
        var result = await _service.GetTokenAsync();

        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task Should_Returned_User_Wallets()
    {
        var result = await _service.GetWalletsListAsync();

        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    [TestCase("BTC_IRT")]
    public async Task Should_Return_Matches_For_A_Symbol(string symbol)
    {
        var result = await _service.GetMatchesListAsync(symbol);

        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task Should_Refresh_The_Access_Token()
    {
        var token = await _service.GetTokenAsync();
        var refreshedToken = await _service.RefreshTokenAsync();

        token.Should().NotBeNull();
        refreshedToken.Should().NotBeNull();
        token.AccessToken.Should().NotBe(refreshedToken.AccessToken);
    }

    [Test]
    public async Task Should_Return_Completed_Orders()
    {
        var result = await _service.GetCompletedOrdersAsync();
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_Return_Pending_Orders()
    {
        var result = await _service.GetPendingOrdersAsync();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task Should_Return_The_Orders()
    {
        var result = await _service.GetOrderListAsync();

        result.Should().NotBeEmpty();
        result.Count().Should().NotBe(0);
    }

    [Ignore("")]
    [Test]
    [TestCase(1)]
    public async Task Should_Cancel_A_Pending_Order(int orderId)
    {
        await _service.CancelOrderAsync(orderId);
    }

    [Test]
    [TestCase(1061895427)]
    public async Task Should_Return_A_Order(int orderId)
    {
        var result = await _service.GetOrderByIdAsync(orderId);
        result.Should().NotBeNull();
    }

    [Ignore("This test creates a real limit order on the exchange. Since it would create actual orders each time it runs, it's skipped from execution.")]
    [Test]
    public async Task Should_Create_A_limit_Order()
    {
        var result = await _service.CreateLimitOrderAsync(new Models.CreateLimitOrderRequest()
        {
            Symbol= "USDT_IRT",
            BaseAmount=1,
            Side="buy",
            Type="limit",
            Price=106868,
        });

        result.Should().NotBeNull();
        result.State.Should().Be("active");
    }

    [Ignore("This test creates a real market order on the exchange. Since it would create actual orders each time it runs, it's skipped from execution.")]
    [Test]
    public async Task Should_Create_A_market_Order()
    {
        var result = await _service.CreateMarketOrderAsync(new Models.CreateMarketOrderRequest()
        {
            Symbol = "USDT_IRT",
            BaseAmount = 1,
            Side = "buy",
            Type = "market",
        });

        result.Should().NotBeNull();
        result.State.Should().Be("active");
    }

    [Ignore("This test creates a real stop-limit order on the exchange. Since it would create actual orders each time it runs, it's skipped from execution.")]
    [Test]
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

        result.Should().NotBeNull();
        result.State.Should().Be("initial");
    }

    [Ignore("This test creates a real OCO order on the exchange. Since it would create actual orders each time it runs, it's skipped from execution.")]
    [Test]
    public async Task Should_Create_A_Oco_Order()
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

        result.Should().NotBeNull();
        result.State.Should().Be("initial");
    }

    [Test]
    [TestCase("BTC_IRT")]
    public async Task Should_Return_Orderbook_For_A_Symbol(string symbol)
    {
        var result = await _service.GetOrderbookAsync(symbol);

        result.Should().NotBeNull();
        result.Asks.Should().NotBeNull();
        result.Bids.Should().NotBeNull();

        // Verify that both asks and bids contain arrays of price and amount
        result.Asks.Should().AllSatisfy(ask =>
        {
            ask.Should().HaveCount(2);
            ask[0].Should().NotBeNullOrEmpty(); // price
            ask[1].Should().NotBeNullOrEmpty(); // amount
        });

        result.Bids.Should().AllSatisfy(bid =>
        {
            bid.Should().HaveCount(2);
            bid[0].Should().NotBeNullOrEmpty(); // price
            bid[1].Should().NotBeNullOrEmpty(); // amount
        });
    }
}
