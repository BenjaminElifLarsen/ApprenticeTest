using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Shared.Service;

/// <summary>
/// Base service with identification, logger, and httpClient.
/// </summary>
public abstract class HttpBaseService
{
    private static readonly Dictionary<Type, HttpClient> s_clients;

    private readonly HttpClient _client;

    static HttpBaseService()
    {
        s_clients = [];
    }

    protected HttpBaseService(string baseUrl)
    {
        if(!s_clients.TryGetValue(GetType(), out var client))
        {
            client = new()
            {
                BaseAddress = new Uri(baseUrl)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            s_clients.Add(GetType(), client);
        }
        _client = client;
    }

    protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
       return await _client.SendAsync(request);
    }
}

public static class HttpExtensions
{
    public static HttpRequestMessage AttachBody<T>(this HttpRequestMessage request, T body)
    {
        request.Content = JsonContent.Create(body);
        return request;
    }

    public static T ToModel<T>(this HttpResponseMessage response)
    {
        return JsonSerializer.Deserialize<T>(response.Content.ReadAsStream(), options: new() { PropertyNameCaseInsensitive = true })!;
    }
}
