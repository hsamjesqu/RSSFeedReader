# Research: Subscription Management MVP

## Decision

Use a minimal ASP.NET Core Web API + Blazor WebAssembly architecture for the MVP, with a single-user in-memory subscription list and explicit validation of input before a subscription is added.

## Rationale

This choice matches the project goals and stakeholder documents, which call for:

- a quick proof-of-concept for adding subscriptions
- a simple UI to display the current subscription list
- no production-grade feed parsing or persistence in the MVP
- a technology path that can evolve toward richer feed features without a rewrite

The architecture keeps the scope narrow while preserving compatibility with future Extended-MVP work like feed fetching, background polling, and persistence.

## Alternatives considered

### Option 1: Single-process desktop or console app

This would minimize infrastructure but would not match the intended ASP.NET Core + Blazor architecture described in the project documentation and would not fit the stated frontend/backend separation expected by the team.

### Option 2: Full production-ready feed reader from day one

This would add complexity not required for the MVP, including feed parsing, persistence, and operational concerns. It conflicts with the project goal of demonstrating basic subscription management without the overhead of a production system.

### Option 3: Shared in-memory model only with no API boundary

This could simplify the initial build but would reduce separation of concerns and would not align with the documented design for a backend API and UI front end.

## Resolved unknowns

- User type: single local user, no multi-user authentication required in the MVP
- Data storage: in-memory collection only for the initial scope
- Validation: basic input validation to reject blank or clearly unusable values
- Feature boundary: add subscriptions and display the list; no fetch, parse, or item rendering in this phase
