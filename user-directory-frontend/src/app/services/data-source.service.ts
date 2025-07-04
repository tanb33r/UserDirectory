import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_DATASOURCE_URL } from './api.config';

@Injectable({ providedIn: 'root' })
export class DataSourceService {
  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const dataSource = localStorage.getItem('selectedDataSource') || 'MSSMS';
    return new HttpHeaders({
      'X-Data-Source': dataSource
    });
  }

  getDataSource(): Observable<string> {
    return this.http.get(API_DATASOURCE_URL, { 
      responseType: 'text',
      headers: this.getHeaders()
    });
  }
}
