import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { ApiService } from '../shared/services/api.service';

@Component({
  selector: 'app-audit-logs',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule, MatIconModule, MatButtonModule, MatChipsModule],
  templateUrl: './audit-logs.component.html',
  styleUrl: './audit-logs.component.scss'
})
export class AuditLogsComponent {
  api = inject(ApiService);
  logs = [];
  displayedColumns = ['createdOn', 'username', 'action', 'entityName', 'recordId'];
  error = '';
  loading = false;

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.error = '';
    this.api.getAuditLogs().subscribe({
      next: (data) => {
        this.logs = data;
        this.loading = false;
      },
      error: (e) => {
        this.error = e?.error?.message || e.message || 'Failed to load audit logs.';
        this.loading = false;
      }
    });
  }
}
