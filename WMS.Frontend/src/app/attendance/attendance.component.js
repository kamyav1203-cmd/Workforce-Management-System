import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { ApiService } from '../shared/services/api.service';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCardModule],
  templateUrl: './attendance.component.html',
  styleUrl: './attendance.component.scss'
})
export class AttendanceComponent {
  api = inject(ApiService);
  auth = inject(AuthService);
  fb = inject(FormBuilder);

  records = [];
  employees = [];
  displayedColumns = ['attendanceDate', 'employeeName', 'checkIn', 'checkOut', 'totalHours', 'workMode', 'actions'];
  openRecord = null;
  hasCheckedInToday = false;

  checkInForm = this.fb.group({ empId: [null], workMode: ['WFO'] });
  filterForm = this.fb.group({
    empId: [null],
    month: [new Date().getMonth() + 1],
    year: [new Date().getFullYear()]
  });

  ngOnInit() {
    const user = this.auth.getUser();
    if (user?.employeeId) {
      this.checkInForm.patchValue({ empId: user.employeeId });
      this.filterForm.patchValue({ empId: user.employeeId });
    }
    this.api.getEmployees().subscribe((e) => { this.employees = e; });
    this.loadAll();

    this.checkInForm.get('empId').valueChanges.subscribe(() => {
      this.updateOpenRecord();
    });
  }

  updateOpenRecord() {
    const today = new Date().toISOString().split('T')[0];
    const selectedEmpId = this.checkInForm.value.empId;

    if (selectedEmpId) {
      this.openRecord = this.records.find((a) => 
        a.empId === selectedEmpId && !a.checkOut && a.attendanceDate?.startsWith(today)
      );
      this.hasCheckedInToday = this.records.some((a) => 
        a.empId === selectedEmpId && a.attendanceDate?.startsWith(today)
      );
    } else {
      this.openRecord = null;
      this.hasCheckedInToday = false;
    }
  }

  loadAll() {
    this.api.getAttendance().subscribe((r) => {
      this.records = r;
      this.updateOpenRecord();
    });
  }

  checkIn() {
    this.api.checkIn(this.checkInForm.value).subscribe(() => this.loadAll());
  }

  checkOut() {
    if (!this.openRecord) return;
    this.api.checkOut({ attendanceId: this.openRecord.attendanceId }).subscribe(() => this.loadAll());
  }

  loadMonthly() {
    const v = this.filterForm.value;
    this.api.getMonthlyAttendance({ empId: v.empId, month: v.month, year: v.year })
      .subscribe((r) => { this.records = r; });
  }

  downloadReport() {
    const v = this.filterForm.value;
    this.api.downloadTimesheet(v.empId, v.month, v.year).subscribe((blob) => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `timesheet_${v.empId}_${v.month}_${v.year}.pdf`;
      a.click();
    });
  }
}
