import { Injectable, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class EmployeeStateService {
  api = inject(ApiService);
  employeesSubject = new BehaviorSubject([]);
  employees$ = this.employeesSubject.asObservable();

  load() {
    this.api.getEmployees().subscribe({
      next: (data) => this.employeesSubject.next(data),
      error: (e) => console.error('Failed to load employees', e)
    });
  }

  refresh() { this.load(); }
  getValue() { return this.employeesSubject.value; }
}
