using System.Net.Http.Json;
using System.Text.Json;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<SubscriptionResponse>> GetSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<SubscriptionResponse[]>("subscriptions", JsonOptions, cancellationToken)
            ?? [];
    }

    public async Task<SubscriptionResponse> CreateSubscriptionAsync(string url, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("subscriptions", new CreateSubscriptionRequest { Url = url }, JsonOptions, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SubscriptionResponse>(JsonOptions, cancellationToken)
                ?? throw new InvalidOperationException("The API returned an empty subscription response.");
        }

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions, cancellationToken);
        throw new InvalidOperationException(error?.Error ?? "The subscription could not be added.");
    }
}

public sealed class CreateSubscriptionRequest
{
    public string? Url { get; set; }
}

public sealed class SubscriptionResponse
{
    public Guid Id { get; init; }

    public string Url { get; init; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }

    public bool IsActive { get; init; }
}

public sealed class ErrorResponse
{
    public string Error { get; init; } = string.Empty;
}
