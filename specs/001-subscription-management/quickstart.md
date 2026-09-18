# Quickstart: Validate the Subscription Management MVP

## Prerequisites

- .NET SDK for the project runtime
- Access to the repository and local development environment
- A browser for confirming the UI behavior

## Run the app

1. Start the backend from the API project.
2. Start the frontend from the Blazor UI project.
3. Open the local frontend URL in the browser.

## Validation scenarios

### Scenario 1: Add a valid feed URL

1. Navigate to the subscription page.
2. Enter a valid RSS or Atom feed URL.
3. Submit the form.
4. Confirm the subscription appears in the list.

**Expected outcome**: The new subscription is visible immediately and the list remains accurate.

### Scenario 2: Reject blank input

1. Navigate to the subscription page.
2. Leave the input empty.
3. Submit the form.

**Expected outcome**: The submission is rejected and the user sees a clear validation message.

### Scenario 3: Reject unusable input

1. Enter a value that is not a valid feed URL.
2. Submit the form.

**Expected outcome**: The entry is not added to the list and the app explains the problem clearly.

### Scenario 4: Verify list integrity

1. Add multiple valid subscriptions.
2. Confirm the list displays each item separately.
3. Confirm prior entries remain visible.

**Expected outcome**: The UI reflects the full current set of subscriptions without losing earlier entries.
