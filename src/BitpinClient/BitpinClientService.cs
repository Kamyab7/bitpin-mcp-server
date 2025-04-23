using BitpinClient.Models;
using System.Text.Json;

namespace BitpinClient;

public class BitpinClientService
{
    private readonly HttpClient _httpClient = new();
    private readonly BitpinClientSettings _settings;
    private string _accessToken = String.Empty;
    private string _refreshToken = String.Empty;
    private DateTime _expirationTime;

    public BitpinClientService(BitpinClientSettings bitpinClientSettings)
    {
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
        _settings = bitpinClientSettings;
        _httpClient.BaseAddress = _settings.ApiUrl;
    }

    public async Task<IEnumerable<Market>?> GetMarketsListAsync()
        => await RequestHandlerAsync<IEnumerable<Market>>(HttpMethod.Get, "mkt/markets/");

    public async Task<IEnumerable<Ticker>?> GetTickersListAsync()
        => await RequestHandlerAsync<IEnumerable<Ticker>>(HttpMethod.Get, "mkt/tickers/");

    public async Task<IEnumerable<Wallet>?> GetWalletsListAsync()
        => await RequestHandlerAsync<IEnumerable<Wallet>>(HttpMethod.Get, "wlt/wallets/", true);

    public async Task<IEnumerable<Match>?> GetMatchesListAsync(string symbol)
        => await RequestHandlerAsync<IEnumerable<Match>>(HttpMethod.Get, $"mth/matches/{symbol}/");

    public async Task<Orderbook?> GetOrderbookAsync(string symbol)
        => await RequestHandlerAsync<Orderbook>(HttpMethod.Get, $"mth/orderbook/{symbol}/");

    public async Task<IEnumerable<Currency>?> GetCurrenciesListAsync()
        => await RequestHandlerAsync<IEnumerable<Currency>>(HttpMethod.Get, "mkt/currencies/");

    public async Task<IEnumerable<CompleteOrder>?> GetCompletedOrdersAsync()
        => await RequestHandlerAsync<IEnumerable<CompleteOrder>>(HttpMethod.Get, "odr/fills/", true);

    public async Task<IEnumerable<PendingOrder>?> GetPendingOrdersAsync()
        => await RequestHandlerAsync<IEnumerable<PendingOrder>>(HttpMethod.Get, "odr/orders/", true);

    public async Task CancelOrderAsync(int orderId)
        => await RequestHandlerAsync(HttpMethod.Delete, $"odr/orders/{orderId}/", true);

    public async Task<Order?> GetOrderByIdAsync(int orderId)
           => await RequestHandlerAsync<Order>(HttpMethod.Get, $"odr/orders/{orderId}/", true);

    public async Task<IEnumerable<Order>?> GetOrderListAsync()
       => await RequestHandlerAsync<IEnumerable<Order>>(HttpMethod.Get, $"odr/orders/", true);

    public async Task<CreateOrderResponse?> CreateLimitOrderAsync(CreateLimitOrderRequest createLimitOrderRequest)
        => await RequestHandlerAsync<CreateOrderResponse, CreateLimitOrderRequest>(HttpMethod.Post, "odr/orders/", createLimitOrderRequest, true);

    public async Task<CreateOrderResponse?> CreateMarketOrderAsync(CreateMarketOrderRequest createMarketOrderRequest)
        => await RequestHandlerAsync<CreateOrderResponse, CreateMarketOrderRequest>(HttpMethod.Post, "odr/orders/", createMarketOrderRequest, true);

    public async Task<CreateOrderResponse?> CreateStopLimitOrderAsync(CreateStopLimitOrderRequest createStopLimitOrderRequest)
        => await RequestHandlerAsync<CreateOrderResponse, CreateStopLimitOrderRequest>(HttpMethod.Post, "odr/orders/", createStopLimitOrderRequest, true);

    public async Task<CreateOrderResponse?> CreateOcoOrderAsync(CreateOcoOrderRequest createOcoOrderRequest)
        => await RequestHandlerAsync<CreateOrderResponse, CreateOcoOrderRequest>(HttpMethod.Post, "odr/orders/", createOcoOrderRequest, true);

    public async Task<GetTokenResponse?> GetTokenAsync()
    {
        var result = await RequestHandlerAsync<GetTokenResponse, GetTokenRequest>(HttpMethod.Post, "usr/authenticate/", new GetTokenRequest()
        {
            Key = _settings.Key,
            Secret = _settings.Secret,
        });

        _accessToken = result!.AccessToken;
        _refreshToken = result.RefreshToken;
        _expirationTime = DateTime.Now.AddMinutes(13);

        return result;
    }

    public async Task<RefreshTokenResponse?> RefreshTokenAsync()
        => await RequestHandlerAsync<RefreshTokenResponse, RefreshTokenRequest>(HttpMethod.Post, "usr/refresh_token/", new RefreshTokenRequest()
        {
            RefreshToken = _refreshToken,
        });

    private async Task<TResponse?> RequestHandlerAsync<TResponse>(HttpMethod httpMethod, string url, bool accessTokenRequired = false)
    {
        using (var request = new HttpRequestMessage(httpMethod, url))
        {
            if (accessTokenRequired)
            {
                var token = await TokenHandlerAsync();
                request.Headers.Add("Authorization", $"Bearer {token}");
            }

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TResponse>(responseContent);
        }
    }

    private async Task<TResponse?> RequestHandlerAsync<TResponse, TRequest>(HttpMethod httpMethod, string url, TRequest requestbody, bool accessTokenRequired = false)
    {
        var requestBody = JsonSerializer.Serialize(requestbody);    

        using (var request = new HttpRequestMessage(httpMethod, url))
        {
            using (var content = new StringContent(requestBody, null, "application/json"))
            {
                if (accessTokenRequired)
                {
                    var token = await TokenHandlerAsync();
                    request.Headers.Add("Authorization", $"Bearer {token}");
                }

                request.Content = content;

                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<TResponse>(responseContent);
            }
        }
    }

    private async Task RequestHandlerAsync(HttpMethod httpMethod, string url, bool accessTokenRequired = false)
    {
        using (var request = new HttpRequestMessage(httpMethod, url))
        {
            if (accessTokenRequired)
            {
                var token = await TokenHandlerAsync();
                request.Headers.Add("Authorization", $"Bearer {token}");
            }

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }

    private async Task<string> TokenHandlerAsync()
    {
        if (String.IsNullOrEmpty(_accessToken))
        {
            var result = await GetTokenAsync();
            _accessToken = result!.AccessToken;
            _refreshToken = result.RefreshToken;
            _expirationTime = DateTime.Now.AddMinutes(13);
        }

        if (DateTime.Now < _expirationTime)
        {
            var result = await RefreshTokenAsync();
            _accessToken = result!.AccessToken;
            _expirationTime = DateTime.Now.AddMinutes(13);
        }

        return _accessToken;
    }
}
