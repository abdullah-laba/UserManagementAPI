# UserManagementAPI

UserManagementAPI is a minimal ASP.NET Core (.NET 10) project that demonstrates a small User Management API alongside Razor Pages. It includes a simple in-memory data store, full CRUD endpoints for users, and middleware for error handling, authentication, and request/response logging.

This repository is intended as a learning/demo project and is not production-ready. It was built and enhanced using automated coding assistance to illustrate common API patterns and middleware usage.

## Features

- RESTful API endpoints for user management (GET, POST, PUT, DELETE)
- Razor Pages for simple UI (project includes default Home pages)
- In-memory data store (Models/Db)
- Input validation using DataAnnotations on the User model
- Exception handling middleware that returns JSON error responses
- Authentication middleware that validates a Bearer token for `/api` routes
- Request/Response logging middleware (method, path, status, elapsed time)

## Requirements

- .NET 10 SDK
- Visual Studio 2026 or the `dotnet` CLI

## Quick start

1. Clone the repository

   git clone https://github.com/abdullah-laba/UserManagementAPI.git
   cd UserManagementAPI

2. Build and run (CLI)

   dotnet build
   dotnet run --project UserManagementAPI.csproj

3. Open in browser

- Razor Pages: https://localhost:{PORT}/ (default Home/Index)
- API endpoints: https://localhost:{PORT}/api/users

Replace {PORT} with the actual HTTPS port printed by the application when it starts.

## Configuration

The authentication middleware uses a simple static token by default. Configure it in `appsettings.json`:

```json
"ApiSettings": {
  "ApiKey": "dev-token"
}
```

For production use, replace this with a secure secret (environment variable, Key Vault, etc.) and implement proper JWT validation.

## API Endpoints

All API endpoints require an Authorization header with a Bearer token (for `/api` routes):

Header:

  Authorization: Bearer dev-token

Examples (replace {PORT} and {id}):

- Get all users

  curl -k -H "Authorization: Bearer dev-token" https://localhost:{PORT}/api/users

- Get user by id

  curl -k -H "Authorization: Bearer dev-token" https://localhost:{PORT}/api/users/{id}

- Create user

  curl -k -X POST -H "Content-Type: application/json" -H "Authorization: Bearer dev-token" -d '{"name":"Alice","email":"alice@example.com"}' https://localhost:{PORT}/api/users

- Update user

  curl -k -X PUT -H "Content-Type: application/json" -H "Authorization: Bearer dev-token" -d '{"name":"Alice Smith","email":"alice.smith@example.com"}' https://localhost:{PORT}/api/users/{id}

- Delete user

  curl -k -X DELETE -H "Authorization: Bearer dev-token" https://localhost:{PORT}/api/users/{id}

Notes:
- The API returns 401 for missing/invalid tokens and 404 for missing resources.
- The in-memory database resets when the application restarts.

## Middleware implemented

- ExceptionHandlingMiddleware (Middleware/ExceptionHandlingMiddleware.cs)
  - Catches unhandled exceptions, logs them, and returns a JSON error response.
- AuthenticationMiddleware (Middleware/AuthenticationMiddleware.cs)
  - Protects routes under `/api` by validating a Bearer token configured in `appsettings.json`.
- RequestResponseLoggingMiddleware (Middleware/RequestResponseLoggingMiddleware.cs)
  - Logs incoming requests and outgoing responses with status code and duration.

The pipeline order (configured in Program.cs) is:
1. ExceptionHandlingMiddleware
2. AuthenticationMiddleware
3. RequestResponseLoggingMiddleware

This ordering ensures errors are handled first, authentication failures are returned as 401, and requests/responses are logged.

## Validation

The `User` model uses DataAnnotations for basic validation:

- Name: required, 1-100 characters
- Email: optional, must be a valid email when provided

Because the controllers use the `[ApiController]` attribute, invalid models automatically return 400 responses with validation details.

## Testing

- Manual testing: use Postman, curl, or HTTPie to exercise endpoints and verify behavior for valid/invalid tokens, invalid payloads, non-existent IDs, and error conditions.
- Recommended automated steps (not included): add unit and integration tests to cover the service and middleware behavior.

## Future improvements

- Replace static token with JWT validation and ASP.NET Core authentication/authorization
- Persist data with EF Core and a relational database (SQLite, SQL Server)
- Add pagination for GET /api/users
- Add request/response body logging with size limits and sensitive-data redaction
- Add unit/integration tests for middleware and controller logic

## Contributing

1. Fork the repository
2. Create a branch for your change
3. Open a pull request with a clear description of your changes

## License

This project is provided as-is for learning and demo purposes. Choose an appropriate open-source license before publishing publicly.
