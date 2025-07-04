import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_DATASOURCE_URL } from './api.config';

@Injectable({ providedIn: 'root' })
export class DataSourceService {
  constructor(private http: HttpClient) {}

  setDataSource(source: string): Observable<void> {
    return this.http.post<void>(API_DATASOURCE_URL, JSON.stringify(source), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  getDataSource(): Observable<string> {
    return this.http.get(API_DATASOURCE_URL, { responseType: 'text'});
  }
}
