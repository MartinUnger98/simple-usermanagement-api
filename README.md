# UserManagementAPI

A minimal .NET 10 Web API for managing users with CRUD operations, request validation, logging, and token-based middleware authentication.

## Features

- CRUD endpoints for users:
  - `GET /api/users` — list all users
  - `GET /api/users/{id}` — get a single user by ID
  - `POST /api/users` — create a new user
  - `PUT /api/users/{id}` — update an existing user
  - `DELETE /api/users/{id}` — remove a user
- Validation via DTOs and data annotations
- Global bad request response formatting for invalid models
- Middleware:
  - `ExceptionHandlingMiddleware` for friendly error responses and logging
  - `RequestResponseLoggingMiddleware` for request/response logging
  - `TokenAuthenticationMiddleware` for bearer token protection of `/api` endpoints
- Swagger UI with bearer auth support

## Configuration

The app uses an API token defined in `appsettings.json`:

```json
"Authentication": {
  "ApiToken": "techhive-secret-token-123"
}
```

## Run

```bash
cd UserManagementAPI
dotnet run
```

Then open Swagger UI at:

```text
https://localhost:7028/swagger
```

## Using Swagger auth

1. Open Swagger UI
2. Click `Authorize`
3. Enter the token value as:
   - `Bearer techhive-secret-token-123`
4. Use the CRUD endpoints

## Validation

The project validates user input using data annotations on `CreateUserDto` and `UpdateUserDto`.
Invalid requests return a structured `400 Bad Request` response with details.

## Notes

- The repository is in-memory and seeded with sample users.
- The auth middleware allows Swagger UI access without a token, but protects actual `/api` routes.
