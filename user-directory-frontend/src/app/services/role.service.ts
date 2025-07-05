import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ROLES_URL } from './api.config';
import { CommonService } from './common.service';

export interface Role {
  id: number;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class RoleService {
  constructor(private http: HttpClient, private commonService: CommonService) {}

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(API_ROLES_URL, { headers: this.commonService.getHeaders() });
  }
}
