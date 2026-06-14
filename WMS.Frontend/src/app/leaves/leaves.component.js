import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ApiService } from '../shared/services/api.service';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-leaves',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, MatTableModule, MatButtonModule,
    MatIconModule, MatFormFieldModule, MatInputModule, MatSelectModule,
    MatCardModule, MatSnackBarModule
  ],
  templateUrl: './leaves.component.html',
  styleUrl: './leaves.component.scss'
})
export class LeavesComponent {
  api = inject(ApiService);
  auth = inject(AuthService);
  fb = inject(FormBuilder);
  snack = inject(MatSnackBar);

  leaves = [];
  employees = [];
  displayedColumns = ['leaveType', 'employeeName', 'fromDate', 'toDate', 'status', 'reason', 'actions'];
  showForm = false;
  submitting = false;

  form = this.fb.group({
    empId: [null, Validators.required],
    leaveType: ['Casual', Validators.required],
    reason: [''],
    // FIX: date fields stored as strings, converted to ISO on submit
    fromDate: ['', Validators.required],
    toDate: ['', Validators.required]
  });

  ngOnInit() {
    const user = this.auth.getUser();
    // Pre-fill employee id for non-admin users
    if (user?.employeeId) {
      this.form.patchValue({ empId: user.employeeId });
    }
    // Disable employee selection for regular employees
    if (!this.auth.hasRole('Admin', 'Manager')) {
      this.form.get('empId')?.disable();
    }
    this.api.getEmployees().subscribe({
      next: (e) => { this.employees = e; },
      error: () => this.snack.open('Failed to load employees', 'Close', { duration: 3000 })
    });
    this.loadLeaves();
  }

  loadLeaves() {
    if (this.auth.hasRole('Admin', 'Manager')) {
      this.api.getLeaves().subscribe({
        next: (l) => { this.leaves = l; },
        error: () => this.snack.open('Failed to load leaves', 'Close', { duration: 3000 })
      });
    } else {
      const empId = this.auth.getUser()?.employeeId;
      if (empId) {
        this.api.getLeavesByEmployee(empId).subscribe({
          next: (l) => { this.leaves = l; },
          error: () => this.snack.open('Failed to load leaves', 'Close', { duration: 3000 })
        });
      }
    }
  }

  apply() {
    if (this.form.invalid || this.submitting) return;

    const val = this.form.getRawValue(); // Use getRawValue to include disabled controls

    const payload = {
      empId: val.empId,
      leaveType: val.leaveType,
      reason: val.reason || '',
      fromDate: val.fromDate || null, // Pass raw yyyy-MM-dd date directly to prevent timezone shifting
      toDate: val.toDate || null
    };

    if (new Date(payload.fromDate) > new Date(payload.toDate)) {
      this.snack.open('From Date cannot be after To Date', 'Close', { duration: 3000, panelClass: 'snack-error' });
      return;
    }

    this.submitting = true;
    this.api.applyLeave(payload).subscribe({
      next: () => {
        this.submitting = false;
        this.showForm = false;
        this.snack.open('Leave applied successfully!', 'Close', { duration: 3000, panelClass: 'snack-success' });
        
        const user = this.auth.getUser();
        this.form.reset({ empId: user?.employeeId ?? null, leaveType: 'Casual', reason: '', fromDate: '', toDate: '' });
        if (!this.auth.hasRole('Admin', 'Manager')) {
          this.form.get('empId')?.disable();
        }
        this.loadLeaves();
      },
      error: (err) => {
        this.submitting = false;
        const msg = err?.error?.message || 'Failed to apply leave. Please try again.';
        this.snack.open(msg, 'Close', { duration: 4000, panelClass: 'snack-error' });
      }
    });
  }

  cancel(id) {
    if (confirm('Are you sure you want to cancel this leave request?')) {
      this.api.cancelLeave(id).subscribe({
        next: () => {
          this.snack.open('Leave cancelled', 'Close', { duration: 3000 });
          this.loadLeaves();
        },
        error: () => this.snack.open('Failed to cancel leave', 'Close', { duration: 3000 })
      });
    }
  }

  approve(leave, status) {
    const user = this.auth.getUser();
    this.api.approveLeave({ leaveId: leave.leaveId, status, approvedBy: user.employeeId }).subscribe({
      next: () => {
        this.snack.open(`Leave ${status.toLowerCase()}`, 'Close', { duration: 3000 });
        this.loadLeaves();
      },
      error: () => this.snack.open('Failed to update leave status', 'Close', { duration: 3000 })
    });
  }

  openForm() {
    const user = this.auth.getUser();
    this.form.reset({ empId: user?.employeeId ?? null, leaveType: 'Casual', reason: '', fromDate: '', toDate: '' });
    if (!this.auth.hasRole('Admin', 'Manager')) {
      this.form.get('empId')?.disable();
    }
    this.showForm = true;
  }

  leaveCount(status) { return this.leaves.filter(l => l.status === status).length; }
  isManager() { return this.auth.hasRole('Admin', 'Manager'); }
  getStatusClass(status) { return 'status-badge status-' + (status || '').toLowerCase(); }
}
