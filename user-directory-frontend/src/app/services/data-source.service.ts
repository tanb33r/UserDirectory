import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_DATASOURCE_URL } from './api.config';
import { CommonService } from './common.service';

@Injectable({ providedIn: 'root' })
export class DataSourceService {
  constructor(private http: HttpClient, private commonService: CommonService) {}

  getDataSource(): Observable<string> {
    return this.http.get(API_DATASOURCE_URL, { 
      responseType: 'text',
      headers: this.commonService.getHeaders()
    });
  }
}
