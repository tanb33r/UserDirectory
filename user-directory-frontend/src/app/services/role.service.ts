import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ROLES_URL } from './api.config';

export interface Role {
  id: number;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class RoleService {
  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const dataSource = localStorage.getItem('selectedDataSource') || 'MSSMS';
    return new HttpHeaders({
      'X-Data-Source': dataSource
    });
  }

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(API_ROLES_URL, { headers: this.getHeaders() });
  }
}
