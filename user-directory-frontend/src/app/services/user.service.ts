import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_USERS_URL } from './api.config';

export interface User {
  id: number;
  firstName: string;
  lastName: string;
  active: boolean;
  company: string;
  sex: string;
  contact: {
    id: number;
    phone: string;
    address: string;
    city: string;
    country: string;
  };
  role: {
    id: number;
    name: string;
  };
}

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const dataSource = localStorage.getItem('selectedDataSource') || 'MSSMS';
    return new HttpHeaders({
      'X-Data-Source': dataSource
    });
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(API_USERS_URL, { headers: this.getHeaders() });
  }

  createUser(user: Partial<User>): Observable<User> {
    return this.http.post<User>(API_USERS_URL, user, { headers: this.getHeaders() });
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${API_USERS_URL}/${id}`, { headers: this.getHeaders() });
  }

  updateUser(user: User): Observable<User> {
    // Prepare payload to match UpdateUserDto
    const payload = {
      id: user.id,
      firstName: user.firstName,
      lastName: user.lastName,
      active: user.active,
      company: user.company,
      sex: user.sex,
      contact: {
        phone: user.contact.phone,
        address: user.contact.address,
        city: user.contact.city,
        country: user.contact.country
      },
      roleId: user.role.id
    };
    return this.http.put<User>(API_USERS_URL, payload, { headers: this.getHeaders() });
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${API_USERS_URL}/${id}`, { headers: this.getHeaders() });
  }
  
  searchUsers(query: string): Observable<User[]> {
    return this.http.get<User[]>(`${API_USERS_URL}/search`, { 
      params: { q: query },
      headers: this.getHeaders()
    });
  }
}
