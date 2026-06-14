import { Component, inject } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../shared/services/auth.service';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, DatePipe, RouterOutlet, RouterLink, RouterLinkActive, MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule, MatButtonModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  auth = inject(AuthService);
  user = this.auth.getUser();
  today = new Date();

  get userInitial() {
    return this.user?.username?.charAt(0)?.toUpperCase() ?? 'U';
  }

  peopleItems = [
    { path: '/employees', icon: 'badge', label: 'Employees', roles: ['Admin', 'Manager'] },
    { path: '/departments', icon: 'corporate_fare', label: 'Departments', roles: ['Admin', 'Manager'] },
  ];

  opsItems = [
    { path: '/attendance', icon: 'fingerprint', label: 'Attendance', roles: ['Admin', 'Manager', 'Employee'] },
    { path: '/leaves', icon: 'beach_access', label: 'Leave Management', roles: ['Admin', 'Manager', 'Employee'] },
    { path: '/projects', icon: 'rocket_launch', label: 'Projects', roles: ['Admin', 'Manager'] },
  ];

  adminItems = [
    { path: '/announcements', icon: 'campaign', label: 'Announcements', roles: ['Admin'] },
    { path: '/audit-logs', icon: 'history', label: 'Audit Logs', roles: ['Admin', 'Manager'] }
  ];

  isAdmin() { return this.auth.hasRole('Admin', 'Manager'); }
  hasAccess(roles) { return !roles || this.auth.hasRole(...roles); }
  logout() { this.auth.logout(); }
}
