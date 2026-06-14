import { Injectable, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class DashboardStateService {
  api = inject(ApiService);
  dashboardSubject = new BehaviorSubject(null);
  dashboard$ = this.dashboardSubject.asObservable();

  load() {
    this.api.getDashboard().subscribe({
      next: (data) => this.dashboardSubject.next(data),
      error: (e) => console.error('Failed to load dashboard', e)
    });
  }

  refresh() { this.load(); }
}
