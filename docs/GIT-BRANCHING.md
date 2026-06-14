# Git Branching Strategy

| Branch | Purpose |
|--------|---------|
| `main` | Production-ready releases |
| `dev` | Integration branch for features |
| `feature/*` | Individual feature work (e.g. `feature/leave-approval`) |
| `feature/DB` | Database schema changes and migrations |

## Workflow

1. Branch from `dev`: `git checkout -b feature/employee-search`
2. Develop and run `dotnet test` before merge
3. Open PR into `dev`
4. After QA, merge `dev` → `main` for release
