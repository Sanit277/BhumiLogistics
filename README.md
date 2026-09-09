# BhumiLogistics

Agri-Logistics & Land Management Web API — Clean Architecture (.NET 9).

## Layers
- **BhumiLogistics.Domain** — entities, value objects, domain events, exceptions. Zero dependencies.
- **BhumiLogistics.Application** — CQRS commands/queries via MediatR, FluentValidation.
- **BhumiLogistics.Infrastructure** — EF Core + PostgreSQL, repositories, JWT/password services.
- **BhumiLogistics.WebApi** — Minimal API endpoints, global exception handling, Swagger, JWT auth.

## Setup

1. Restore and build:
   ```
   dotnet restore
   dotnet build
   ```

2. Set your real PostgreSQL password in `src/BhumiLogistics.WebApi/appsettings.json`
   under `ConnectionStrings:DefaultConnection`.

3. Apply migrations (from the solution root):
   ```
   dotnet ef database update --project src/BhumiLogistics.Infrastructure --startup-project src/BhumiLogistics.WebApi
   ```

4. Run:
   ```
   cd src/BhumiLogistics.WebApi
   dotnet run
   ```

5. Open the printed URL + `/swagger`.

## Auth flow
1. `POST /api/auth/register` — create an account.
2. `POST /api/auth/login` — get a JWT.
3. Click **Authorize** in Swagger, paste the token.
4. `POST /api/land-plots` and `POST /api/lease-offers` now work — `OwnerId`/`TenantUserId`
   are derived from the token, not the request body.
5. `PUT /api/lease-offers/{leaseOfferId}/accept` accepts an offer and marks the plot leased.

## Note on secrets
`appsettings.json` contains a placeholder `Jwt:Key` and database password for local development
only. Replace both before deploying anywhere real, and never commit real secrets to source control.
