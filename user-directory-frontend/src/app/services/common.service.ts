import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class CommonService {
  getHeaders(): HttpHeaders {
    const dataSource = localStorage.getItem('selectedDataSource') || '';
    return new HttpHeaders({
      'X-Data-Source': dataSource
    });
  }
} 