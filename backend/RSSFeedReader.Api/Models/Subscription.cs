namespace RSSFeedReader.Api.Models;

public sealed class Subscription
{
    public Guid Id { get; init; }

    public required string Url { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public bool IsActive { get; init; }
}
