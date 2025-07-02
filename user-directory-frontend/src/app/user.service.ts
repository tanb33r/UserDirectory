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
}
