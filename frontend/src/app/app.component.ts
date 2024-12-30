import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { WeatherService, WeatherForecast } from './services/weather.service';
import { HealthService, HealthStatus } from './services/health.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, DatePipe],
  providers: [
    WeatherService,
    HealthService
  ],
  template: `
    <div class="container">
      <h1>Weather Forecasts</h1>
      
      <!-- แสดงสถานะการเชื่อมต่อ -->
      <div class="health-status" *ngIf="healthStatus">
        <p>
          Database Status: 
          <span [class]="healthStatus.status === 'Healthy' ? 'status-ok' : 'status-error'">
            {{ healthStatus.database }}
          </span>
        </p>
        <p *ngIf="healthStatus.error" class="error-message">
          Error: {{ healthStatus.error }}
        </p>
      </div>

      <!-- แสดงข้อมูล Weather -->
      <div *ngIf="forecasts" class="forecasts">
        <div *ngFor="let forecast of forecasts" class="forecast-item">
          <h3>{{ forecast.summary }}</h3>
          <p>Temperature: {{ forecast.temperatureC }}°C / {{ forecast.temperatureF }}°F</p>
          <p>Date: {{ forecast.date | date }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .container { padding: 20px; }
    .health-status { margin-bottom: 20px; padding: 10px; border-radius: 4px; }
    .status-ok { color: green; font-weight: bold; }
    .status-error { color: red; font-weight: bold; }
    .error-message { color: red; }
    .forecast-item { 
      margin-bottom: 20px;
      padding: 15px;
      border: 1px solid #ddd;
      border-radius: 4px;
    }
  `]
})
export class AppComponent implements OnInit {
  forecasts: WeatherForecast[] = [];
  healthStatus?: HealthStatus;

  constructor(
    private weatherService: WeatherService,
    private healthService: HealthService
  ) {}

  ngOnInit() {
    // ตรวจสอบสถานะการเชื่อมต่อ
    this.healthService.checkHealth().subscribe({
      next: (status) => {
        this.healthStatus = status;
        if (status.status === 'Healthy') {
          this.loadForecasts();
        }
      },
      error: (error) => {
        this.healthStatus = {
          status: 'Unhealthy',
          database: 'Disconnected',
          timestamp: new Date().toISOString(),
          error: 'Cannot connect to the server'
        };
      }
    });
  }

  private loadForecasts() {
    this.weatherService.getWeatherForecasts().subscribe({
      next: (data) => this.forecasts = data,
      error: (error) => console.error('Error loading forecasts:', error)
    });
  }
}
