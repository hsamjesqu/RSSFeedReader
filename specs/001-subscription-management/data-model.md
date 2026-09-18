# Data Model: Subscription Management

## Entities

### Subscription

Represents a single RSS or Atom feed the user wants to track.

**Fields**
- Id: unique identifier for the subscription
- Url: the feed URL entered by the user
- CreatedAt: timestamp when the subscription was added
- IsActive: indicates whether the subscription is currently enabled in the app

**Relationships**
- A user has zero or more subscriptions.
- Each subscription belongs to a single user in the MVP.

**Validation rules**
- Url MUST be present and not blank.
- Url MUST be treated as a valid feed URL format before accepting it.
- Duplicate entries SHOULD be prevented when the same URL is submitted again.

### User

Represents the person using the application in the MVP.

**Fields**
- Id: unique identifier for the user
- Name: display-friendly label for the active user context
- Subscriptions: collection of feed subscriptions assigned to the user

**Relationships**
- One user may own multiple subscriptions.

## State transitions

- Initial state: no subscriptions
- After add action: subscription moves to active state and is added to the list
- On invalid input: subscription is rejected and no list update occurs
- On duplicate submission: the app may reject the request with a clear validation response

## Notes

This model is intentionally minimal and aligned with the MVP scope. It does not include feed item data, refresh scheduling, or persistence.
