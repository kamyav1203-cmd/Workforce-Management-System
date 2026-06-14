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
import { MatTabsModule } from '@angular/material/tabs';
import { ApiService } from '../shared/services/api.service';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCardModule, MatTabsModule],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.scss'
})
export class ProjectsComponent {
  api = inject(ApiService);
  auth = inject(AuthService);
  fb = inject(FormBuilder);

  projects = [];
  clients = [];
  employees = [];
  allocations = [];
  editProjectId = null;
  editClientId = null;
  projectColumns = ['projectName', 'clientName', 'startDate', 'status', 'actions'];
  clientColumns = ['clientName', 'clientLocation', 'clientPhoneNumber', 'status', 'actions'];
  allocColumns = ['employeeName', 'projectName', 'assignedOn', 'approvalStatus', 'actions'];

  projectForm = this.fb.group({
    projectName: ['', Validators.required],
    clientId: [null],
    startDate: [''],
    status: ['Active']
  });

  clientForm = this.fb.group({
    clientName: ['', Validators.required],
    clientAdress: [''],
    clientPhoneNumber: [null],
    clientLocation: [''],
    status: [true]
  });

  allocForm = this.fb.group({
    empId: [null, Validators.required],
    projectId: [null, Validators.required],
    assignedOn: [new Date().toISOString().split('T')[0], Validators.required],
    createdBY: ['']
  });

  ngOnInit() {
    this.allocForm.patchValue({ createdBY: this.auth.getUser()?.username || 'system' });
    this.loadAll();
  }

  loadAll() {
    this.api.getProjects().subscribe({ next: (p) => { this.projects = p; } });
    this.api.getClients().subscribe({ next: (c) => { this.clients = c; } });
    this.api.getEmployees().subscribe({ next: (e) => { this.employees = e; } });
    this.api.getAllocations().subscribe({ next: (a) => { this.allocations = a; } });
  }

  saveProject() {
    if (this.projectForm.invalid) return;
    const req = this.editProjectId
      ? this.api.updateProject(this.editProjectId, this.projectForm.value)
      : this.api.createProject(this.projectForm.value);
    req.subscribe({ next: () => { this.editProjectId = null; this.projectForm.reset({ status: 'Active' }); this.loadAll(); } });
  }

  editProject(p) {
    this.editProjectId = p.projectId;
    this.projectForm.patchValue({
      projectName: p.projectName, clientId: p.clientId,
      startDate: p.startDate?.split('T')[0], status: p.status
    });
  }

  deleteProject(id) {
    if (confirm('Mark project as completed?')) {
      this.api.deleteProject(id).subscribe({ next: () => this.loadAll() });
    }
  }

  saveClient() {
    if (this.clientForm.invalid) return;
    const req = this.editClientId
      ? this.api.updateClient(this.editClientId, this.clientForm.value)
      : this.api.createClient(this.clientForm.value);
    req.subscribe({ next: () => { this.editClientId = null; this.clientForm.reset({ status: true }); this.loadAll(); } });
  }

  editClient(c) {
    this.editClientId = c.clientId;
    this.clientForm.patchValue(c);
  }

  deleteClient(id) {
    if (confirm('Deactivate this client?')) {
      this.api.deleteClient(id).subscribe({ next: () => this.loadAll() });
    }
  }

  assignEmployee() {
    if (this.allocForm.invalid) return;
    this.api.assignEmployee(this.allocForm.value).subscribe({
      next: () => {
        this.allocForm.patchValue({
          assignedOn: new Date().toISOString().split('T')[0],
          createdBY: this.auth.getUser()?.username
        });
        this.loadAll();
      }
    });
  }

  approveAllocation(a, status) {
    const updatedBy = this.auth.getUser()?.username || 'system';
    this.api.approveAllocation({ allocationId: a.allocationId, approvalStatus: status, updatedBy })
      .subscribe({ next: () => this.loadAll() });
  }

  cancelAllocation(id) {
    const updatedBy = this.auth.getUser()?.username || 'system';
    this.api.cancelAllocation(id, updatedBy).subscribe({ next: () => this.loadAll() });
  }

  canManage() { return this.auth.hasRole('Admin', 'Manager'); }
}
