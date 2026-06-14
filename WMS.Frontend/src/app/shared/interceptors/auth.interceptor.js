import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor = (req, next) => {
  const auth = inject(AuthService);
  const token = auth.getToken();
  if (token) {
    req = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }
  return next(req);
};
