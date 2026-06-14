# WMS High-Level Design (HLD)

## 1. Overview
Workforce Management System (WMS) centralizes HR operations: employees, attendance, leaves, departments, projects, and reporting.

## 2. Architecture
```
[Angular Frontend] --JWT/HTTPS--> [ASP.NET Core API]
                                        |
                                   [Application Layer]
                                        |
                              [Infrastructure + EF Core]
                                        |
                              [SQL Server LocalDB / Azure SQL]
```

## 3. Layers
| Layer | Responsibility |
|-------|----------------|
| WMS.Frontend | Angular UI, Material, Chart.js, RxJS state |
| WMS.API | REST controllers, JWT, Swagger, middleware |
| WMS.Application | Services, DTOs, validators, reports |
| WMS.Domain | Entities, repository interfaces |
| WMS.Infrastructure | EF Core, repositories, migrations |
| WMS.Tests | xUnit service tests |

## 4. Security
- JWT Bearer authentication
- Role-based authorization (Admin, Manager, Employee)
- CORS restricted to configured origins
- Secrets via environment variables (`WMS_*`)

## 5. Deployment
- API: Azure App Service
- Frontend: Azure Static Web Apps
- CI/CD: Azure DevOps pipelines in `WMS.DevOps/`

## 6. Integrations
- SQL Server (LocalDB dev, Azure SQL prod)
- PDF Timesheet reports (Crystal Reports format via QuestPDF)
