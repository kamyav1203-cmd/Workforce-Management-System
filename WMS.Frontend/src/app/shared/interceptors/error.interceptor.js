import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { LoggingService } from '../services/logging.service';

export const errorInterceptor = (req, next) => {
  const logger = inject(LoggingService);
  return next(req).pipe(
    catchError((error) => {
      const message = error.error?.message || error.message || 'An unexpected error occurred';
      logger.logError(message, error);
      return throwError(() => ({ message, status: error.status }));
    })
  );
};
