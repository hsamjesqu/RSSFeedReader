using RSSFeedReader.Api.Models;

namespace RSSFeedReader.Api.Services;

public sealed class SubscriptionStore
{
    private readonly List<Subscription> subscriptions = [];
    private readonly Lock syncRoot = new();

    public IReadOnlyList<Subscription> GetAll()
    {
        lock (syncRoot)
        {
            return subscriptions.ToArray();
        }
    }

    public bool TryAdd(string? url, out Subscription? subscription, out string? error)
    {
        subscription = null;
        error = ValidateUrl(url);
        if (error is not null)
        {
            return false;
        }

        var normalizedUrl = url!.Trim();
        lock (syncRoot)
        {
            if (subscriptions.Any(existing => string.Equals(existing.Url, normalizedUrl, StringComparison.OrdinalIgnoreCase)))
            {
                error = "This subscription is already on the list.";
                return false;
            }

            subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                Url = normalizedUrl,
                CreatedAt = DateTimeOffset.UtcNow,
                IsActive = true
            };
            subscriptions.Add(subscription);
            return true;
        }
    }

    private static string? ValidateUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return "A valid feed URL is required.";
        }

        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var parsedUrl) ||
            (parsedUrl.Scheme != Uri.UriSchemeHttp && parsedUrl.Scheme != Uri.UriSchemeHttps) ||
            string.IsNullOrWhiteSpace(parsedUrl.Host))
        {
            return "A valid feed URL is required.";
        }

        return null;
    }
}
