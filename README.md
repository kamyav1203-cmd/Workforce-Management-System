# Workforce Management System (WMS)

Capstone project — Left Shift Program 2026. Full-stack WMS per PDF specification.

## Architecture

```
WMS-Solution/
├── WMS.API/              → ASP.NET Core 8 Web API (JWT, Swagger, global exception middleware)
├── WMS.Application/      → Services, DTOs, FluentValidation, AutoMapper, PDF Reports
├── WMS.Domain/           → Entities, Repository Interfaces
├── WMS.Infrastructure/   → EF Core Code-First, LocalDB, Migrations
├── WMS.Frontend/         → Angular 19 (JavaScript) + Material + Chart.js
├── WMS.Tests/            → xUnit unit tests (6 tests)
├── WMS.DevOps/           → Azure DevOps build + release pipelines
└── docs/                 → HLD, LLD, test cases, git branching guide
```

## Prerequisites

- .NET SDK 8.0
- Node.js LTS
- SQL Server LocalDB `(localdb)\MSSQLLocalDB`

## Run

```bash
# Backend
cd WMS-Solution
dotnet run --project WMS.API
# Swagger: http://localhost:5280/swagger

# Frontend
cd WMS.Frontend
npm install && npm start
# App: http://localhost:4200
```

## Demo Logins

| Username | Password | Role |
|----------|----------|------|
| admin | Admin@123 | Admin |
| manager | Manager@123 | Manager |
| employee | Employee@123 | Employee |

## PDF Requirements Checklist

| Requirement | Status |
|-------------|--------|
| Layered .NET architecture | Done |
| Angular + JavaScript frontend | Done |
| SQL Server LocalDB + EF Code-First | Done |
| All 10 database tables | Done |
| JWT + CORS + HTTPS (prod) | Done |
| Repository + Service pattern | Done |
| DTOs + AutoMapper | Done |
| DataAnnotations + FluentValidation | Done |
| Employee CRUD + search | Done |
| Attendance check-in/out + monthly + PDF timesheet | Done |
| Leave apply/cancel/approve-reject | Done |
| Department CRUD | Done |
| Project CRUD + client CRUD | Done |
| Project assign/cancel/approve-reject | Done |
| Announcements CRUD | Done |
| Dashboard KPIs + Chart.js | Done |
| RxJS BehaviorSubject state | Done |
| Auth guard + interceptors | Done |
| Global exception handling + logging | Done |
| Environment configs (dev/staging/prod) | Done |
| HLD + LLD documentation | Done |
| Module test cases document | Done |
| xUnit tests | Done (6 tests) |
| CI/CD build + release pipelines | Done |
| Git branching strategy doc | Done |

## Timesheet Reports

PDF timesheet reports generated via QuestPDF in Crystal Reports format. Download from Attendance page or `GET /api/attendance/timesheet`.

## Environment Variables

See `WMS.API/.env.example`. Set `WMS_Jwt__Key` and `WMS_ConnectionStrings__DefaultConnection` for production.

## Tests

```bash
dotnet test WMS-Solution/WMS.Tests
```
