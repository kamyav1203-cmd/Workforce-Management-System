import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { ApiService } from '../shared/services/api.service';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatCardModule],
  templateUrl: './departments.component.html',
  styleUrl: './departments.component.scss'
})
export class DepartmentsComponent {
  api = inject(ApiService);
  auth = inject(AuthService);
  fb = inject(FormBuilder);

  departments = [];
  displayedColumns = ['departmentId', 'departmentName', 'description', 'createdOn', 'actions'];
  showForm = false;
  editId = null;

  form = this.fb.group({
    departmentName: ['', Validators.required],
    description: ['']
  });

  ngOnInit() { this.load(); }

  load() { this.api.getDepartments().subscribe((d) => { this.departments = d; }); }

  openAdd() { this.editId = null; this.form.reset(); this.showForm = true; }

  openEdit(d) {
    this.editId = d.departmentId;
    this.form.patchValue({ departmentName: d.departmentName, description: d.description });
    this.showForm = true;
  }

  save() {
    if (this.form.invalid) return;
    const req = this.editId
      ? this.api.updateDepartment(this.editId, this.form.value)
      : this.api.createDepartment(this.form.value);
    req.subscribe(() => { this.showForm = false; this.load(); });
  }

  delete(id) {
    if (confirm('Delete this department?')) {
      this.api.deleteDepartment(id).subscribe(() => this.load());
    }
  }

  isAdmin() { return this.auth.hasRole('Admin'); }
}
