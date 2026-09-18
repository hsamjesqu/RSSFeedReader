# Subscription API Contract

## Purpose

The API provides the minimal contract needed for subscription management in the MVP.

## Endpoints

### POST /api/subscriptions

Creates a new subscription for the active user.

**Request body**
```json
{
  "url": "https://example.com/feed.xml"
}
```

**Success response**
```json
{
  "id": "guid",
  "url": "https://example.com/feed.xml",
  "createdAt": "2026-09-17T00:00:00Z",
  "isActive": true
}
```

**Validation failure response**
```json
{
  "error": "A valid feed URL is required."
}
```

### GET /api/subscriptions

Returns the current list of subscriptions for the active user.

**Success response**
```json
[
  {
    "id": "guid",
    "url": "https://example.com/feed.xml",
    "createdAt": "2026-09-17T00:00:00Z",
    "isActive": true
  }
]
```

## Notes

- This contract intentionally excludes feed-fetch and item-display endpoints because those are not part of the MVP.
- The API is designed to evolve into richer feed behavior in later phases without a rewrite.
