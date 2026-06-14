import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoggingService {
  logError(message, error) {
    console.error('[WMS Error]', message, error);
  }

  logInfo(message) {
    console.info('[WMS Info]', message);
  }
}
