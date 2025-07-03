import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
  private apiUrl = 'https://localhost:44385';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/users`);
  }

  createUser(user: Partial<User>): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/users`, user);
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/users/${id}`);
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
    return this.http.put<User>(`${this.apiUrl}/users`, payload);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/users/${id}`);
  }
}
