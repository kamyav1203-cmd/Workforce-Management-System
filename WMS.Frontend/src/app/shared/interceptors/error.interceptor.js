import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { LoggingService } from '../services/logging.service';
import { AuthService } from '../services/auth.service';

export const errorInterceptor = (req, next) => {
  const logger = inject(LoggingService);
  const authService = inject(AuthService);
  return next(req).pipe(
    catchError((error) => {
      if (error.status === 401) {
        authService.logout();
      }
      const message = error.error?.message || error.message || 'An unexpected error occurred';
      logger.logError(message, error);
      return throwError(() => ({ message, status: error.status }));
    })
  );
};
