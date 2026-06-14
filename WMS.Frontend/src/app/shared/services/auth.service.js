import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  http = inject(HttpClient);
  router = inject(Router);

  currentUserSubject = new BehaviorSubject(this.getStoredUser());
  currentUser$ = this.currentUserSubject.asObservable();

  login(username, password) {
    return this.http.post(`${environment.apiUrl}/auth/login`, { username, password }).pipe(
      tap((response) => {
        localStorage.setItem('wms_token', response.token);
        localStorage.setItem('wms_user', JSON.stringify(response));
        this.currentUserSubject.next(response);
      })
    );
  }

  logout() {
    localStorage.removeItem('wms_token');
    localStorage.removeItem('wms_user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  getToken() {
    return localStorage.getItem('wms_token');
  }

  getUser() {
    return this.currentUserSubject.value;
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  hasRole(...roles) {
    const user = this.getUser();
    return user && roles.includes(user.role);
  }

  getStoredUser() {
    const data = localStorage.getItem('wms_user');
    return data ? JSON.parse(data) : null;
  }
}
