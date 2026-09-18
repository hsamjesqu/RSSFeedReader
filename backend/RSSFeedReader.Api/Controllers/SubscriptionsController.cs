using Microsoft.AspNetCore.Mvc;
using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Controllers;

[ApiController]
[Route("api/subscriptions")]
public sealed class SubscriptionsController(SubscriptionStore store) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<SubscriptionResponse>> GetSubscriptions()
    {
        return Ok(store.GetAll().Select(ToResponse).ToArray());
    }

    [HttpPost]
    public ActionResult<SubscriptionResponse> CreateSubscription(CreateSubscriptionRequest request)
    {
        if (!store.TryAdd(request.Url, out var subscription, out var error))
        {
            return BadRequest(new ErrorResponse { Error = error! });
        }

        return Created($"api/subscriptions/{subscription!.Id}", ToResponse(subscription));
    }

    private static SubscriptionResponse ToResponse(Subscription subscription) => new()
    {
        Id = subscription.Id,
        Url = subscription.Url,
        CreatedAt = subscription.CreatedAt,
        IsActive = subscription.IsActive
    };
}
