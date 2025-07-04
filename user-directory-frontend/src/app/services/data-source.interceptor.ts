import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class DataSourceInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const dataSource = localStorage.getItem('selectedDataSource');
    if (dataSource) {
      const cloned = req.clone({
        setHeaders: {
          'X-Data-Source': dataSource
        }
      });
      return next.handle(cloned);
    }
    return next.handle(req);
  }
}
