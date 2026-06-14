import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { BaseChartDirective } from 'ng2-charts';
import { ApiService } from '../shared/services/api.service';
import { DashboardStateService } from '../shared/services/dashboard-state.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, BaseChartDirective],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  api = inject(ApiService);
  dashboardState = inject(DashboardStateService);
  data = null;
  announcements = [];

  attendanceChart = {
    labels: [],
    datasets: [{ data: [], label: 'Attendance', backgroundColor: '#3b82f6' }]
  };

  leaveChart = {
    labels: [],
    datasets: [{ data: [], label: 'Leaves', backgroundColor: ['#f59e0b', '#22c55e', '#ef4444'] }]
  };

  chartOptions = { responsive: true, maintainAspectRatio: false };

  ngOnInit() {
    this.dashboardState.load();
    this.dashboardState.dashboard$.subscribe((d) => {
      if (!d) return;
      this.data = d;
      this.attendanceChart = {
        labels: d.attendanceChart.map((x) => x.label),
        datasets: [{ data: d.attendanceChart.map((x) => x.value), label: 'Daily Attendance', backgroundColor: '#3b82f6' }]
      };
      this.leaveChart = {
        labels: d.leaveChart.map((x) => x.label),
        datasets: [{ data: d.leaveChart.map((x) => x.value), label: 'Leave Status', backgroundColor: ['#f59e0b', '#22c55e', '#ef4444'] }]
      };
    });
    this.api.getAnnouncements().subscribe((a) => { this.announcements = a; });
  }
}
