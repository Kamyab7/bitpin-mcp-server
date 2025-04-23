using BitpinClient;
using BitpinClient.Models;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace BitpinSseServer.Tools;

[McpServerToolType]
internal sealed class BitpinExchangeTools
{
    private readonly BitpinClientService _bitpinClientService;

    public BitpinExchangeTools(BitpinClientService bitpinClientService)
    {
        _bitpinClientService = bitpinClientService;
    }

    [McpServerTool, Description("""
            Allows you to retrieve the BitpinExchange balance of your wallets across different cryptocurrencies.
            Using this API, you can view your asset balances individually and filter them based on your needs.
        """)]
    public async Task<IEnumerable<Wallet>?> GetWalletsListAsync() => await _bitpinClientService.GetWalletsListAsync();

    [McpServerTool, Description("""
           Retrieve the full list of trading markets supported on Bitpin.
           The information includes the market symbol, market name, base currency, and quote currency—helping you better understand the available markets.
        """)]
    public async Task<IEnumerable<Market>?> GetMarketsListAsync() => await _bitpinClientService.GetMarketsListAsync();

    [McpServerTool, Description("""
            Retrieve the current price list of Bitpin's markets.
        """)]
    public async Task<IEnumerable<Ticker>?> GetTickersListAsync() => await _bitpinClientService.GetTickersListAsync();

    [McpServerTool, Description("""
           Retrieve a complete list of cryptocurrencies supported on Bitpin.
           The data includes the currency symbol, name, tradability status, and decimal precision for each asset—helping you better understand the available cryptocurrencies and how they can be traded.
        """)]
    public async Task<IEnumerable<Currency>?> GetCurrenciesAsync() => await _bitpinClientService.GetCurrenciesListAsync();

    [McpServerTool, Description("""
           The order book allows you to view open orders in each of Bitpin's markets.
           It includes lists of buy orders (bids) and sell orders (asks).
        """)]
    public async Task<Orderbook?> GetOrderbooksAsync([Description("The symbol representing the trading pair (e.g., 'USDT_IRT' for Tether to Iranian Rial).")] string symbol) => await _bitpinClientService.GetOrderbookAsync(symbol);

    [McpServerTool, Description("""
           Retrieve the latest trades executed in each market.
           The data includes price details, traded volume, and the trade type (buy or sell).
        """)]
    public async Task<IEnumerable<Match>?> GetMatchesAsync([Description("The symbol representing the trading pair (e.g., 'USDT_IRT' for Tether to Iranian Rial).")] string symbol) => await _bitpinClientService.GetMatchesListAsync(symbol);

    [McpServerTool, Description("""
           A limit order allows you to buy or sell a cryptocurrency at a specific price you set.
           This type of order is only executed when the market price reaches your specified limit.

           For example, if you want to buy 100 USDT at 62,000 Tomans, you would place a limit buy order.
        """)]
    public async Task<CreateOrderResponse?> CreateLimitOrderAsync(
        [Description("The trading pair symbol (e.g., 'USDT_IRT' for Tether to Iranian Rial).")] string symbol,
        [Description("The amount of the base currency you want to buy or sell.")] decimal baseAmount,
        [Description("The order side: 'buy' or 'sell'.")] string side,
        [Description("The price at which you want to place the limit order.")] decimal price
    ) => await _bitpinClientService.CreateLimitOrderAsync(new CreateLimitOrderRequest()
    {
        Symbol = symbol,
        BaseAmount = baseAmount,
        Side = side,
        Type = "limit",
        Price = price
    });

    [McpServerTool, Description("""
        A market order allows you to instantly buy or sell a cryptocurrency at the best available price in the market.
        This type of order is useful for taking advantage of rapid price movements, as it is executed immediately at the current best price.

        Example:
        Suppose you want to buy 100 USDT at the market price. With a market order, you don’t need to specify a price—
        your order will be filled instantly at the best available rate.
    """)]
    public async Task<CreateOrderResponse?> CreateMarketOrderAsync(
        [Description("The trading pair symbol (e.g., 'USDT_IRT' for Tether to Iranian Rial).")] string symbol,
        [Description("The amount of the base currency you want to buy or sell.")] decimal baseAmount,
        [Description("The order side: 'buy' or 'sell'.")] string side
    ) => await _bitpinClientService.CreateMarketOrderAsync(new CreateMarketOrderRequest()
    {
        Symbol = symbol,
        BaseAmount = baseAmount,
        Side = side,
        Type = "market"
    });

    [McpServerTool, Description("""
        A stop_limit order is a combination of a stop order and a limit order. In this type of order, you set a 'stop price'. 
        Once the market price reaches that stop price, a limit order is triggered.

        This order type is useful for protecting against potential losses or capturing profits from market fluctuations.

        Example:
        Suppose you want to buy 100 USDT when its price exceeds 62,000 Tomans, but you want the order to be filled at 61,900 Tomans.
    """)]
    public async Task<CreateOrderResponse?> CreateStopLimitOrderAsync(
        [Description("The trading pair symbol (e.g., 'USDT_IRT' for Tether to Iranian Rial).")] string symbol,
        [Description("The amount of the base currency you want to buy or sell.")] decimal baseAmount,
        [Description("The order side: 'buy' or 'sell'.")] string side,
        [Description("The stop price at which the limit order will be triggered.")] decimal stopPrice,
        [Description("The price at which you want the limit order to be executed once the stop price is reached.")] decimal price
    ) => await _bitpinClientService.CreateStopLimitOrderAsync(new CreateStopLimitOrderRequest()
    {
        Symbol = symbol,
        BaseAmount = baseAmount,
        Side = side,
        Type = "stop_limit",
        StopPrice = stopPrice,
        Price = price
    });

    [McpServerTool, Description("""
        An OCO (One Cancels the Other) order is a combination of a limit order and a stop-limit order.  
        In this type of order, two orders are placed simultaneously in the market. If one is executed, the other is automatically canceled.

        This order type is useful for managing both buy and sell orders in the market simultaneously.

        Example:
        Suppose you own 1 Bitcoin and want to sell it when the price reaches $70,000. However, if the price drops to $58,000, you want to sell at $57,990 to limit your losses.
    """)]
    public async Task<CreateOrderResponse?> CreateOcoOrderAsync(
        [Description("The trading pair symbol (e.g., 'BTCUSDT' for Bitcoin to Tether).")] string symbol,
        [Description("The order type: 'limit' or 'stop-limit'. This is always 'oco' for an OCO order.")] string type,
        [Description("The order side: 'buy' or 'sell'.")] string side,
        [Description("The amount of the base currency you want to buy or sell.")] decimal baseAmount,
        [Description("The target price for the limit order. Once the market reaches this price, the limit order is triggered.")] decimal ocoTargetPrice,
        [Description("The stop price at which the stop-limit order will be triggered.")] decimal stopPrice,
        [Description("The price at which the stop-limit order will be executed once the stop price is reached.")] decimal price
    ) => await _bitpinClientService.CreateOcoOrderAsync(new CreateOcoOrderRequest()
    {
        Symbol = symbol,
        Type = "oco",
        Side = side,
        BaseAmount = baseAmount,
        OcoTargetPrice = ocoTargetPrice,
        StopPrice = stopPrice,
        Price = price
    });

    [McpServerTool, Description("""
            Retrieve and review your list of orders.
        """)]
    public async Task<IEnumerable<Order>?> GetOrdersAsync() => await _bitpinClientService.GetOrderListAsync();


    [McpServerTool, Description("""
        View the details of your executed trades.  
        This feature allows you to review your trade history and access information about each trade, such as the amount, price, and fee details.
    """)]
    public async Task<IEnumerable<CompleteOrder>?> GetCompeletedOrdersAsync() => await _bitpinClientService.GetCompletedOrdersAsync();

    [McpServerTool, Description("""
        Using this API, you can view a specific order by its order ID.
    """)]
    public async Task<Order?> GetOrderByIdAsync([Description("The unique identifier for the order.")] int orderId) => await _bitpinClientService.GetOrderByIdAsync(orderId);

    [McpServerTool, Description("""
        By calling this API, you can cancel a specific order using its order ID.
    """)]
    public async Task CancelOrderByIdAsync([Description("The unique identifier for the order to be canceled.")] int orderId) => await _bitpinClientService.CancelOrderAsync(orderId);
}
