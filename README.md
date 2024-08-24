# Matchmaker

The matchmaker, as implemented in this project, is an internal JSON-based HTTP API service, not directly contacted by the clients. For it to be publicly accessible, its endpoints need to check for authorisation from incoming clients; such authorisation is the responsibility of some out-of-scope gateway service.

The session it creates are considered identifiers to lobbies where users go to before they start the actual game.

The service has three endpoints:

## 1. Create a user's game/match session

**Endpoint**: **POST /users/{id}/sessions**

**Request body**:

```json
{
  "latencyInMilliseconds": 125
}
```

**Note**: Latency is expected to be in the range of 0 (inclusive) to 1000 (exclusive).

**Possible responses**:

1. 204 No Content (upon successful queuing of the request)
2. 409 Conflict (if a session already exists for the user)
3. 400 Bad Request (if latency is missing or is invalid)

The second type of response will also include the session id in its response body as:

```json
{
  "sessionId": 123456789
}
```

## 2. Get a user's game/match session

**Endpoint**: **GET /users/{id}/session**

**Possible responses**:

1. 200 Ok (if a session were to exist for the user)
2. 404 Not Found (if a session does not exist for the user)
3. 400 Bad Request (if user id is invalid)

The first type of response will also include the session id in its response body as:

```json
{
  "sessionId": 123456789
}
```

## 3. Delete a user's game/match session

**Endpoint**: **DELETE /users/{id}/session**

**Possible responses**:

1. 204 No Content (if the session is successfully deleted)
2. 404 Not Found (if the session or its request does not exist)
3. 400 Bad Request (if user id is invalid)

---
