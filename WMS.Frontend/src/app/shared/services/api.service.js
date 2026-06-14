import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getEmployees() { return this.http.get(`${this.baseUrl}/employees`); }
  searchEmployees(params) { return this.http.get(`${this.baseUrl}/employees/search`, { params }); }
  createEmployee(data) { return this.http.post(`${this.baseUrl}/employees`, data); }
  updateEmployee(id, data) { return this.http.put(`${this.baseUrl}/employees/${id}`, data); }
  deleteEmployee(id) { return this.http.delete(`${this.baseUrl}/employees/${id}`); }

  getDepartments() { return this.http.get(`${this.baseUrl}/departments`); }
  createDepartment(data) { return this.http.post(`${this.baseUrl}/departments`, data); }
  updateDepartment(id, data) { return this.http.put(`${this.baseUrl}/departments/${id}`, data); }
  deleteDepartment(id) { return this.http.delete(`${this.baseUrl}/departments/${id}`); }

  getRoles() { return this.http.get(`${this.baseUrl}/roles`); }

  getAttendance() { return this.http.get(`${this.baseUrl}/attendance`); }
  checkIn(data) { return this.http.post(`${this.baseUrl}/attendance/checkin`, data); }
  checkOut(data) { return this.http.post(`${this.baseUrl}/attendance/checkout`, data); }
  getMonthlyAttendance(params) { return this.http.get(`${this.baseUrl}/attendance/monthly`, { params }); }
  downloadTimesheet(empId, month, year) {
    return this.http.get(`${this.baseUrl}/attendance/timesheet`, {
      params: { empId, month, year },
      responseType: 'blob'
    });
  }

  getLeaves() { return this.http.get(`${this.baseUrl}/leaves`); }
  getLeavesByEmployee(empId) { return this.http.get(`${this.baseUrl}/leaves/employee/${empId}`); }
  applyLeave(data) { return this.http.post(`${this.baseUrl}/leaves/apply`, data); }
  cancelLeave(id) { return this.http.delete(`${this.baseUrl}/leaves/${id}`); }
  approveLeave(data) { return this.http.put(`${this.baseUrl}/leaves/approve`, data); }

  getProjects() { return this.http.get(`${this.baseUrl}/projects`); }
  createProject(data) { return this.http.post(`${this.baseUrl}/projects`, data); }
  updateProject(id, data) { return this.http.put(`${this.baseUrl}/projects/${id}`, data); }
  deleteProject(id) { return this.http.delete(`${this.baseUrl}/projects/${id}`); }
  getClients() { return this.http.get(`${this.baseUrl}/projects/clients`); }
  createClient(data) { return this.http.post(`${this.baseUrl}/projects/clients`, data); }
  updateClient(id, data) { return this.http.put(`${this.baseUrl}/projects/clients/${id}`, data); }
  deleteClient(id) { return this.http.delete(`${this.baseUrl}/projects/clients/${id}`); }
  getAllocations() { return this.http.get(`${this.baseUrl}/projects/allocations`); }
  assignEmployee(data) { return this.http.post(`${this.baseUrl}/projects/allocations`, data); }
  approveAllocation(data) { return this.http.put(`${this.baseUrl}/projects/allocations/approve`, data); }
  cancelAllocation(id, updatedBy) {
    return this.http.put(`${this.baseUrl}/projects/allocations/${id}/cancel`, null, { params: { updatedBy } });
  }

  getDashboard() { return this.http.get(`${this.baseUrl}/dashboard`); }
  getAuditLogs() { return this.http.get(`${this.baseUrl}/auditlogs`); }
  getAnnouncements() { return this.http.get(`${this.baseUrl}/announcements`); }
  getAllAnnouncements() { return this.http.get(`${this.baseUrl}/announcements/all`); }
  createAnnouncement(data) { return this.http.post(`${this.baseUrl}/announcements`, data); }
  updateAnnouncement(id, data) { return this.http.put(`${this.baseUrl}/announcements/${id}`, data); }
  deleteAnnouncement(id) { return this.http.delete(`${this.baseUrl}/announcements/${id}`); }
}
