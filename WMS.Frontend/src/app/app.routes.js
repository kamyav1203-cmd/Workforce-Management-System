import { authGuard } from './shared/guards/auth.guard';
import { LoginComponent } from './auth/login.component';
import { LayoutComponent } from './layout/layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeesComponent } from './employees/employees.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { LeavesComponent } from './leaves/leaves.component';
import { DepartmentsComponent } from './departments/departments.component';
import { ProjectsComponent } from './projects/projects.component';
import { AnnouncementsComponent } from './announcements/announcements.component';
import { AuditLogsComponent } from './audit-logs/audit-logs.component';

export const routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'employees', component: EmployeesComponent },
      { path: 'attendance', component: AttendanceComponent },
      { path: 'leaves', component: LeavesComponent },
      { path: 'departments', component: DepartmentsComponent },
      { path: 'projects', component: ProjectsComponent },
      { path: 'announcements', component: AnnouncementsComponent },
      { path: 'audit-logs', component: AuditLogsComponent }
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];