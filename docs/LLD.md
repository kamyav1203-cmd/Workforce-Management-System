# WMS Low-Level Design (LLD)

## 1. Database Schema
10 tables per specification: Employee, Department, Role, Attendance, Leave, Announcement, Project, Client, EmployeeProjectAllocation, UserLogin, AuditLog.

Key relationships:
- Employee → Department, Role
- Attendance/Leave → Employee
- Project → Client
- EmployeeProjectAllocation → Employee, Project (with ApprovalStatus: Pending/Approved/Rejected)

## 2. API Endpoints

| Module | Endpoints |
|--------|-----------|
| Auth | POST `/api/auth/login` |
| Employees | GET/POST/PUT/DELETE `/api/employees`, GET `/search` |
| Departments | Full CRUD `/api/departments` |
| Attendance | checkin, checkout, monthly, timesheet PDF |
| Leaves | apply, cancel, approve |
| Projects | CRUD, clients CRUD, allocations assign/approve/cancel |
| Announcements | CRUD `/api/announcements` |
| Dashboard | GET `/api/dashboard` |

## 3. Patterns
- Repository + Unit of Work
- Service layer with DTOs + AutoMapper
- FluentValidation + DataAnnotations
- Global exception middleware
- Audit log on all mutations

## 4. Frontend Modules
`auth`, `employees`, `attendance`, `leaves`, `dashboard`, `departments`, `projects`, `announcements`, `shared`

## 5. State Management
RxJS `BehaviorSubject` in `AuthService`, `EmployeeStateService`, `DashboardStateService`.
