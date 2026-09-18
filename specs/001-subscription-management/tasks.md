---

description: "Executable task list for the subscription management MVP"
---

# Tasks: Subscription Management MVP

**Input**: Design documents from `/specs/001-subscription-management/`

**Prerequisites**: [plan.md](plan.md), [spec.md](spec.md), [research.md](research.md), [data-model.md](data-model.md), [contracts/subscription-api.md](contracts/subscription-api.md), [quickstart.md](quickstart.md)

**Tests**: Test tasks are included because the project constitution requires validation evidence for feed submission, state updates, routing, and API contracts.

**Organization**: Tasks are grouped by user story so each increment can be implemented and verified independently after the foundational work is complete.

## Phase 1: Setup

- [ ] T001 Create the .NET solution file at `RSSFeedReader.sln` and establish the `backend/` and `frontend/` project directories described in `specs/001-subscription-management/plan.md`
- [ ] T002 [P] Create the ASP.NET Core Web API project at `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj` targeting .NET 8
- [ ] T003 [P] Create the Blazor WebAssembly project at `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj` targeting .NET 8
- [ ] T004 [P] Create the backend test project at `backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj` with xUnit and a project reference to `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`
- [ ] T005 Add the backend, frontend, and backend test projects to the solution file `./RSSFeedReader.sln`

## Phase 2: Foundational

- [ ] T006 Configure the backend startup pipeline in `backend/RSSFeedReader.Api/Program.cs` with controllers or minimal API routing, JSON responses, development CORS, and the configured local API port
- [ ] T007 Configure the frontend startup pipeline in `frontend/RSSFeedReader.UI/Program.cs` with an `HttpClient` whose base address comes from `frontend/RSSFeedReader.UI/wwwroot/appsettings.json`
- [ ] T008 [P] Add the frontend API configuration in `frontend/RSSFeedReader.UI/wwwroot/appsettings.json` and set its base URL to the backend API route without hardcoding the URL in components
- [ ] T009 [P] Configure the allowed frontend origins in `backend/RSSFeedReader.Api/Program.cs` to match the frontend launch settings and document the local ports in `backend/RSSFeedReader.Api/Properties/launchSettings.json` and `frontend/RSSFeedReader.UI/Properties/launchSettings.json`
- [ ] T010 Remove template demo pages from `frontend/RSSFeedReader.UI/Pages/` and update `frontend/RSSFeedReader.UI/Layout/NavMenu.razor` so only the MVP subscription route owns `/`

## Phase 3: User Story 1 - Add a subscription and see it appear (Priority: P1)

**Goal**: A user can enter a valid feed URL, submit it, and see the new subscription immediately alongside existing entries.

**Independent test**: Run the backend and frontend, submit `https://example.com/feed.xml` on the subscription page, and verify the returned subscription appears without a page refresh; submit a second URL and verify the first remains visible.

- [ ] T011 [P] [US1] Create the `Subscription` model in `backend/RSSFeedReader.Api/Models/Subscription.cs` with `Id`, `Url`, `CreatedAt`, and `IsActive` fields matching `specs/001-subscription-management/data-model.md`
- [ ] T012 [P] [US1] Create the add-subscription request and response DTOs in `backend/RSSFeedReader.Api/Models/SubscriptionDtos.cs` matching the JSON shapes in `specs/001-subscription-management/contracts/subscription-api.md`
- [ ] T013 [US1] Implement the single-user in-memory subscription store in `backend/RSSFeedReader.Api/Services/SubscriptionStore.cs`, preserving prior entries and creating active subscriptions with unique IDs and creation timestamps
- [ ] T014 [US1] Implement `POST /api/subscriptions` in `backend/RSSFeedReader.Api/Controllers/SubscriptionsController.cs` to add a valid subscription through `SubscriptionStore` and return the created subscription as JSON
- [ ] T015 [US1] Implement the subscription API client in `frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs` for posting the URL and deserializing the contract response
- [ ] T016 [US1] Create the subscription page in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` with an accessible URL input, submit button, and list rendering that adds the successful response immediately
- [ ] T017 [US1] Add the subscription page route and navigation entry in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` and `frontend/RSSFeedReader.UI/Layout/NavMenu.razor`, ensuring the page is the only `/` route
- [ ] T018 [US1] Add API contract tests in `backend/RSSFeedReader.Api.Tests/SubscriptionsApiTests.cs` proving a valid POST returns the created URL, an ID, a creation timestamp, and `isActive: true`
- [ ] T019 [US1] Add frontend configuration and rendering smoke coverage in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` or the project’s established UI test location proving a successful submission updates the visible list without navigation

## Phase 4: User Story 2 - Review current subscriptions (Priority: P2)

**Goal**: A user can retrieve and review every current subscription as a separate readable list item.

**Independent test**: Add multiple subscriptions, reload or revisit the subscription view, call `GET /api/subscriptions`, and verify every current item is returned and displayed separately.

- [ ] T020 [US2] Implement `GET /api/subscriptions` in `backend/RSSFeedReader.Api/Controllers/SubscriptionsController.cs` to return the active user’s current in-memory subscriptions in a stable order
- [ ] T021 [US2] Extend `frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs` with a get-subscriptions operation matching the array response in `specs/001-subscription-management/contracts/subscription-api.md`
- [ ] T022 [US2] Update `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` to load the current list on initialization and render each subscription as a distinct accessible list item
- [ ] T023 [US2] Add API and store tests in `backend/RSSFeedReader.Api.Tests/SubscriptionsApiTests.cs` proving multiple entries survive subsequent additions and GET returns all current entries
- [ ] T024 [US2] Add a browser smoke scenario in `specs/001-subscription-management/quickstart.md` or the established UI test location proving reload/revisit displays the current subscription list without losing entries

## Phase 5: User Story 3 - Handle invalid or incomplete submissions (Priority: P3)

**Goal**: Blank, clearly invalid, and duplicate submissions are rejected with clear feedback and do not corrupt the list.

**Independent test**: Submit blank input, malformed input, and a duplicate URL; verify each rejected request shows a visible message and does not create an additional list entry.

- [ ] T025 [US3] Add request validation in `backend/RSSFeedReader.Api/Models/SubscriptionDtos.cs` and `backend/RSSFeedReader.Api/Controllers/SubscriptionsController.cs` so `Url MUST be present and not blank` and `Url MUST be treated as a valid feed URL format before accepting it`
- [ ] T026 [US3] Add duplicate URL detection in `backend/RSSFeedReader.Api/Services/SubscriptionStore.cs` so duplicate entries are prevented and the API returns the documented validation error shape
- [ ] T027 [US3] Add frontend validation and error-state rendering in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` so rejected submissions show a clear user-facing message and do not update the list
- [ ] T028 [US3] Add invalid, blank, and duplicate API tests in `backend/RSSFeedReader.Api.Tests/SubscriptionsApiTests.cs` proving rejected requests do not mutate the in-memory collection
- [ ] T029 [US3] Add keyboard-accessibility and visible-validation smoke checks in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` or the established UI test location for blank and malformed submissions

## Final Phase: Polish and Cross-Cutting Verification

- [ ] T030 [P] Add XML documentation or focused comments only where needed in `backend/RSSFeedReader.Api/Models/`, `backend/RSSFeedReader.Api/Services/`, and `backend/RSSFeedReader.Api/Controllers/` to document the in-memory MVP boundary and deferred feed-fetch behavior
- [ ] T031 [P] Add focused styling and semantic markup in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` and `frontend/RSSFeedReader.UI/wwwroot/css/` so labels, validation messages, list items, and keyboard focus are clear and usable
- [ ] T032 Run `dotnet clean ./RSSFeedReader.sln` and `dotnet build ./RSSFeedReader.sln` to verify the solution file `./RSSFeedReader.sln` builds after template cleanup and feature implementation
- [ ] T033 Run the backend test suite with `dotnet test backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj` and retain the results as validation evidence
- [ ] T034 Execute all scenarios in `specs/001-subscription-management/quickstart.md`, including valid add, multiple-item review, blank rejection, malformed URL rejection, and duplicate handling
- [ ] T035 Review the completed implementation against `specs/001-subscription-management/spec.md`, `specs/001-subscription-management/contracts/subscription-api.md`, and `.specify/memory/constitution.md`, confirming no feed fetching, persistence, removal, or item rendering was added

## Dependencies and Execution Order

### Dependency Graph

```text
T001 -> T002, T003, T004 -> T005
T005 -> T006, T007, T008, T009, T010
T006 + T007 + T010 -> US1
US1 -> US2
US1 -> US3
US2 + US3 -> Polish and cross-cutting verification
```

### User Story Completion Order

1. **US1 (P1)** depends on setup and foundational configuration; it establishes the subscription model, in-memory store, POST endpoint, and primary UI.
2. **US2 (P2)** depends on US1’s store and UI; it adds GET retrieval and reload/revisit behavior.
3. **US3 (P3)** depends on US1’s POST path; it strengthens validation and duplicate handling without changing the primary workflow.
4. **Polish** follows the story increments and provides final build, test, quickstart, and constitution verification.

## Parallel Execution Examples

### Setup and foundational work

```text
Parallel: T002, T003, T004
Parallel: T008, T009 after T005
```

### US1

```text
Parallel: T011 and T012
After T011/T012: T013 and T015 can proceed in parallel
After T013: T014
After T014/T015: T016 and T018 can proceed in parallel
```

### US2

```text
After US1: T020 and T021 can proceed in parallel
After T020/T021: T022 and T023 can proceed in parallel
```

### US3

```text
After US1: T025 and T027 can proceed in parallel
After T025: T026 and T028 can proceed in parallel
After T027: T029
```

## Implementation Strategy

1. Complete setup and foundational configuration so the backend and frontend run independently with aligned ports and CORS.
2. Deliver **US1 as the MVP slice**: add a URL through the API and render the successful subscription immediately.
3. Add **US2** to make the current list retrievable and stable on reload/revisit.
4. Add **US3** to protect the primary flow with validation, duplicate prevention, and accessible error messaging.
5. Finish with build, automated tests, manual quickstart scenarios, and constitution review.

## Independent Test Criteria Summary

- **US1**: A valid URL POST returns a created subscription and the Blazor page displays it immediately; a second valid URL does not remove the first.
- **US2**: GET returns all current subscriptions as separate entries, and reloading or revisiting the page displays the same current list.
- **US3**: Blank, malformed, and duplicate submissions are rejected with visible feedback and no list mutation.

## MVP Scope Recommendation

Implement through **US1** after completing Setup and Foundational phases. This is the smallest complete slice that satisfies the project goal: adding a subscription URL and displaying it in the UI. US2 and US3 should follow before calling the broader feature production-ready within the stated MVP quality expectations.

## Format Validation

All implementation tasks use the required checklist format:

- Every task starts with `- [ ]`.
- Every task has a sequential ID from `T001` through `T035`.
- `[P]` appears only on tasks identified as parallelizable.
- Every user-story task includes its corresponding `[US1]`, `[US2]`, or `[US3]` label.
- Every task description includes one or more concrete file paths.
