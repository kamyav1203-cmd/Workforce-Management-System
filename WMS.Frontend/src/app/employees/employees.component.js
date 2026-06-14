import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
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
  selector: 'app-employees',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule, MatTableModule, MatButtonModule,
    MatIconModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCardModule, MatSnackBarModule
  ],
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.scss'
})
export class EmployeesComponent {
  api = inject(ApiService);
  auth = inject(AuthService);
  fb = inject(FormBuilder);
  snack = inject(MatSnackBar);

  employees = [];
  departments = [];
  roles = [];
  displayedColumns = ['employeeId', 'name', 'email', 'departmentName', 'roleName', 'status', 'actions'];
  showForm = false;
  editId = null;

  // FIX: Search now supports name, departmentId AND roleId
  searchName = '';
  searchDeptId = null;
  searchRoleId = null;

  form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', Validators.required],
    gender: ['M', Validators.required],
    dob: ['', Validators.required],
    doj: ['', Validators.required],
    departmentId: [null, Validators.required],
    roleId: [null, Validators.required],
    status: ['Active']
  });

  ngOnInit() { this.loadData(); }

  loadData() {
    this.api.getEmployees().subscribe({
      next: (e) => { this.employees = e; },
      error: () => this.snack.open('Failed to load employees', 'Close', { duration: 3000 })
    });
    this.api.getDepartments().subscribe((d) => { this.departments = d; });
    this.api.getRoles().subscribe((r) => { this.roles = r; });
  }

  // FIX: Pass all search params including departmentId and roleId
  search() {
    const params = {};
    if (this.searchName?.trim()) params['name'] = this.searchName.trim();
    if (this.searchDeptId) params['departmentId'] = this.searchDeptId;
    if (this.searchRoleId) params['roleId'] = this.searchRoleId;

    if (Object.keys(params).length === 0) {
      this.loadData();
      return;
    }

    this.api.searchEmployees(params).subscribe({
      next: (e) => { this.employees = e; },
      error: () => this.snack.open('Search failed', 'Close', { duration: 3000 })
    });
  }

  clearSearch() {
    this.searchName = '';
    this.searchDeptId = null;
    this.searchRoleId = null;
    this.loadData();
  }

  openAdd() {
    this.editId = null;
    this.form.reset({ gender: 'M', status: 'Active' });
    this.showForm = true;
  }

  openEdit(emp) {
    this.editId = emp.employeeId;
    this.form.patchValue({
      firstName: emp.firstName, lastName: emp.lastName, email: emp.email,
      phoneNumber: emp.phoneNumber, gender: emp.gender,
      dob: emp.dob?.split('T')[0], doj: emp.doj?.split('T')[0],
      departmentId: emp.departmentId, roleId: emp.roleId, status: emp.status
    });
    this.showForm = true;
  }

  save() {
    if (this.form.invalid) return;
    const data = { ...this.form.value };
    const req = this.editId
      ? this.api.updateEmployee(this.editId, data)
      : this.api.createEmployee(data);
    req.subscribe({
      next: () => {
        this.showForm = false;
        this.snack.open(this.editId ? 'Employee updated' : 'Employee added', 'Close', { duration: 3000 });
        this.loadData();
      },
      error: (err) => {
        const msg = err?.error?.message || 'Operation failed';
        this.snack.open(msg, 'Close', { duration: 4000 });
      }
    });
  }

  delete(id) {
    if (confirm('Deactivate this employee?')) {
      this.api.deleteEmployee(id).subscribe({
        next: () => { this.snack.open('Employee deactivated', 'Close', { duration: 3000 }); this.loadData(); },
        error: () => this.snack.open('Failed to deactivate', 'Close', { duration: 3000 })
      });
    }
  }

  getStatusClass(status) { return 'status-badge status-' + (status || '').toLowerCase(); }
  canManage() { return this.auth.hasRole('Admin', 'Manager'); }
}
