import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface HealthStatus {
  status: string;
  database: string;
  timestamp: string;
  error?: string;
}

@Injectable({
  providedIn: 'root'
})
export class HealthService {
  private apiUrl = `${environment.apiUrl}/HealthCheck`;

  constructor(private http: HttpClient) { }

  checkHealth(): Observable<HealthStatus> {
    return this.http.get<HealthStatus>(this.apiUrl);
  }
} 