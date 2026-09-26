# Movies API

A REST API built in .NET (C#) as a hands-on learning project. The goal was to learn how to design and build a production-style REST API in ASP.NET Core - including layering, authentication, validation, versioning, documentation, and consuming the API from a typed client SDK.

## What it does

A simple movies catalog where users can browse movies and rate them.

- CRUD for movies, searchable by id **or** slug, with sorting and pagination
- Users can rate movies (1-5), remove their ratings, and list their own ratings
- Movie rating averages are calculated automatically

## Tech stack

- .NET 8 / ASP.NET Core Web API (controllers)
- PostgreSQL + Dapper (raw SQL, no ORM)
- FluentValidation
- JWT Bearer authentication + API key fallback
- API versioning (Asp.Versioning, media-type based)
- Swagger / OpenAPI
- Refit (typed client SDK)

## Project structure

| Project | Purpose                                                                           |
|---|-----------------------------------------------------------------------------------|
| `Movies.Api` | ASP.NET Core REST API - controllers, auth, mapping, Swagger, health checks        |
| `Movies.Application` | Business layer - services, repositories, validators, database (Dapper/PostgreSQL) |
| `Movies.Contracts` | Shared request/response models used by both the API and clients                   |
| `Movies.Api.Sdk` | Refit-based typed client SDK generated from the API endpoints                     |
| `Movies.Api.Sdk.Consumer` | Console app demonstrating how to consume the API through the SDK                  |

## API overview

Base URL: `http://localhost:5062/api` (versioned via the `api-version` media-type header)

| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/movies` | Create a movie | Trusted member / JWT |
| GET | `/movies/{idOrSlug}` | Get a movie by id or slug | Public |
| GET | `/movies` | List movies (title/year filter, sort, paging) | Public |
| PUT | `/movies/{id}` | Update a movie | Trusted member / JWT |
| DELETE | `/movies/{id}` | Delete a movie | Admin / JWT |
| PUT | `/movies/{movieId}/ratings` | Rate a movie (1–5) | JWT |
| DELETE | `/movies/{movieId}/ratings` | Remove own rating | JWT |
| GET | `/ratings/me` | List own ratings | JWT |
| GET | `_health` | Health check (includes DB check) | Public |

## Authentication

The API supports two schemes:

1. **JWT Bearer** - real bearer tokens validated against configured issuer/audience.
2. **API key** - via the `x-api-key` header, backed by custom authorization policies:
   - `Admin` - full access (delete movies)
   - `Trusted` - create/update movies

This was implemented to learn how multiple authorization sources can coexist behind custom `IAuthorizationRequirement` handlers.

## Running locally

Requirements: .NET 8 SDK and a PostgreSQL database.

1. Set the connection string in `Movies.Api/appsettings.json` under `Database:ConnectionString`, or override it via environment variables / user secrets.
2. Tables (`movies`, `genres`, `ratings`) are created automatically on startup by `DbInitializer`.
3. Run the API:

```bash
dotnet run --project Movies.Api
```

4. Open Swagger UI at `http://localhost:5062/swagger` (dev only).

To try the SDK consumer (client demo), run the API first, then:

```bash
dotnet run --project Movies.Api.Sdk.Consumer
```

## Topics covered

This project was a vehicle to learn and practice:

- Layered architecture (Api → Application → Contracts) and Dependency Injection
- REST conventions: routing, status codes, `CreatedAtAction`, pagination/sorting contracts
- Model validation with FluentValidation and mapping validation errors to a consistent response
- Authentication/authorization: JWT, API keys, custom policies and requirements
- API versioning and version-aware Swagger documentation
- Data access with Dapper and raw SQL (transactions, joins, aggregates)
- Database health checks
- Building and consuming a typed client SDK with Refit and DI

## Notes

This is a learning project - some things (like hardcoded demo user ids/keys in the auth handlers) are intentionally simplified and not production-ready.
