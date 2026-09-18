using RSSFeedReader.Api.Models;

using Microsoft.AspNetCore.Mvc;
using RSSFeedReader.Api.Controllers;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Tests;

public sealed class SubscriptionsApiTests
{
    [Fact]
    public void CreateSubscription_ReturnsCreatedSubscription()
    {
        var controller = new SubscriptionsController(new SubscriptionStore());

        var result = controller.CreateSubscription(new() { Url = "https://example.com/feed.xml" });
        var created = Assert.IsType<CreatedResult>(result.Result);
        var response = Assert.IsType<SubscriptionResponse>(created.Value);

        Assert.Equal("https://example.com/feed.xml", response.Url);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.True(response.IsActive);
        Assert.NotEqual(default, response.CreatedAt);
    }

    [Fact]
    public void GetSubscriptions_PreservesEarlierEntries()
    {
        var controller = new SubscriptionsController(new SubscriptionStore());
        controller.CreateSubscription(new() { Url = "https://example.com/one.xml" });
        controller.CreateSubscription(new() { Url = "https://example.com/two.xml" });

        var result = controller.GetSubscriptions();
        var response = Assert.IsType<OkObjectResult>(result.Result);
        var subscriptions = Assert.IsAssignableFrom<IReadOnlyList<SubscriptionResponse>>(response.Value);

        Assert.Equal(2, subscriptions.Count);
        Assert.Equal("https://example.com/one.xml", subscriptions[0].Url);
        Assert.Equal("https://example.com/two.xml", subscriptions[1].Url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-a-url")]
    [InlineData("ftp://example.com/feed.xml")]
    public void CreateSubscription_RejectsInvalidUrls(string? url)
    {
        var controller = new SubscriptionsController(new SubscriptionStore());

        var result = controller.CreateSubscription(new() { Url = url });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var error = Assert.IsType<ErrorResponse>(badRequest.Value);
        Assert.Equal("A valid feed URL is required.", error.Error);
    }

    [Fact]
    public void CreateSubscription_RejectsDuplicateWithoutMutatingList()
    {
        var store = new SubscriptionStore();
        var controller = new SubscriptionsController(store);
        controller.CreateSubscription(new() { Url = "https://example.com/feed.xml" });

        var result = controller.CreateSubscription(new() { Url = "HTTPS://EXAMPLE.COM/feed.xml" });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var error = Assert.IsType<ErrorResponse>(badRequest.Value);
        Assert.Equal("This subscription is already on the list.", error.Error);
        Assert.Single(store.GetAll());
    }
}
