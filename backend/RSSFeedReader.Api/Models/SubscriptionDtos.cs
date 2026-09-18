namespace RSSFeedReader.Api.Models;

public sealed class CreateSubscriptionRequest
{
    public string? Url { get; set; }
}

public sealed class SubscriptionResponse
{
    public Guid Id { get; init; }

    public required string Url { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public bool IsActive { get; init; }
}

public sealed class ErrorResponse
{
    public required string Error { get; init; }
}
