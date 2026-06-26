# Golinks Backend

REST API for URL shortening with access tracking and metrics. Built with ASP.NET Core 10, CQRS, and Auth0 authentication.

## Tech Stack

- **.NET 10** / ASP.NET Core
- **Entity Framework Core 10** with PostgreSQL (Npgsql)
- **MediatR 14** — CQRS (Commands and Queries)
- **Mapster 7** — object mapping
- **FluentValidation 11** — request validation
- **Auth0** — JWT Bearer authentication
- **Swagger** — API documentation
- **Docker** — containerization

## Architecture

The project follows a layered architecture with CQRS:

```
Golinks.Domain         → Entities and DTOs
Golinks.Repository     → DbContext (EF Core), migrations
Golinks.Application    → Commands, Queries, Handlers, Validators, Mappings
Golinks.WebAPI         → Controllers, Middlewares, API configuration
```

Errors are handled with the `Result<T>` pattern and returned as `ProblemDetails` (RFC 7807).

## Endpoints

Routes follow kebab-case convention.

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| `GET` | `/{slug}` | Public | Redirect to the original URL and track the access |
| `GET` | `/links` | Required | List links (paginated) |
| `GET` | `/links/{id}` | Required | Get link by ID |
| `POST` | `/links` | `golinks:admin` | Create a link |
| `PUT` | `/links/{id}` | `golinks:admin` | Update a link |
| `DELETE` | `/links/{id}` | `golinks:admin` | Delete a link |
| `GET` | `/links/{id}/qrcode` | Required | Get a PNG QR code for the link's public URL |
| `GET` | `/metrics` | Public | List links with access metrics |
| `GET` | `/health/live` | Public | Liveness probe (process is responding) |
| `GET` | `/health/ready` | Public | Readiness probe (database is reachable) |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- An [Auth0](https://auth0.com/) account with an API configured

## Configuration

Create or update `appsettings.Development.json` with your credentials:

```json
{
  "Auth0": {
    "Authority": "https://<your-tenant>.auth0.com/",
    "Audience": "https://<your-audience>"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=golinks;Username=golinks_app_user;Password=YourStrong!Passw0rd;"
  }
}
```

## Running with Docker

Starts the API, PostgreSQL, and Adminer:

```bash
docker-compose up -d
```

| Service | URL |
|---------|-----|
| API | `http://localhost:8080` |
| Adminer | `http://localhost:9090` |
| PostgreSQL | `localhost:5432` |

Migrations are applied automatically on startup.

## Running Locally

```bash
# Start only the database
docker-compose up -d golinks.postgres

# Restore dependencies and run
dotnet restore
dotnet run --project Golinks.WebAPI
```

Swagger documentation is available at `http://localhost:<port>/swagger` in the development environment.

## Feature Structure

Each feature follows the vertical slice pattern inside `Golinks.Application/Features/Links`:

```
Features/Links/
  Commands/
    CreateLink/
    UpdateLink/
    DeleteLink/
    TrackAccess/
  Queries/
    GetAllLinks/
    GetLinkById/
    GetMetrics/
```

## Authentication

The API uses JWT Bearer via Auth0. Protected endpoints require a valid token in the header:

```
Authorization: Bearer <token>
```

Write operations (create, update, delete) additionally require the `golinks:admin` permission in the token.
