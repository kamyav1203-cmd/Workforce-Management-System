import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { ApiService } from '../shared/services/api.service';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-announcements',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatCardModule, MatCheckboxModule, MatChipsModule],
  templateUrl: './announcements.component.html',
  styleUrl: './announcements.component.scss'
})
export class AnnouncementsComponent {
  api = inject(ApiService);
  auth = inject(AuthService);
  fb = inject(FormBuilder);

  announcements = [];
  displayedColumns = ['title', 'message', 'createdOn', 'isActive', 'actions'];
  showForm = false;
  editId = null;
  error = '';
  loading = false;

  get isAdmin() {
    return this.auth.getUser()?.role === 'Admin';
  }

  form = this.fb.group({
    title: ['', Validators.required],
    message: ['', Validators.required],
    isActive: [true]
  });

  ngOnInit() {
    if (!this.isAdmin) {
      this.displayedColumns = ['title', 'message', 'createdOn'];
    }
    this.load();
  }

  load() {
    this.loading = true;
    this.error = '';
    const request$ = this.isAdmin ? this.api.getAllAnnouncements() : this.api.getAnnouncements();
    request$.subscribe({
      next: (a) => { this.announcements = a; this.loading = false; },
      error: (e) => { this.error = e?.error?.message || e.message || 'Failed to load announcements.'; this.loading = false; }
    });
  }

  openAdd() {
    this.editId = null;
    this.form.reset({ isActive: true });
    this.showForm = true;
  }

  openEdit(a) {
    this.editId = a.announcementId;
    this.form.patchValue({ title: a.title, message: a.message, isActive: a.isActive });
    this.showForm = true;
  }

  save() {
    if (this.form.invalid) return;
    const user = this.auth.getUser();
    const data = this.editId ? this.form.value : { ...this.form.value, createdBy: user.employeeId };
    const req = this.editId
      ? this.api.updateAnnouncement(this.editId, this.form.value)
      : this.api.createAnnouncement(data);
    req.subscribe({
      next: () => { this.showForm = false; this.load(); },
      error: (e) => { this.error = e?.error?.message || e.message || 'Failed to save announcement.'; }
    });
  }

  delete(id) {
    if (confirm('Deactivate this announcement?')) {
      this.api.deleteAnnouncement(id).subscribe({
        next: () => this.load(),
        error: (e) => { this.error = e?.error?.message || e.message || 'Failed to delete announcement.'; }
      });
    }
  }
}
