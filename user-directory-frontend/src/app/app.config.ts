
import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { DataSourceInterceptor } from './services/data-source.interceptor';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideHttpClient(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DataSourceInterceptor,
      multi: true
    },
    importProvidersFrom(ToastrModule.forRoot({
      positionClass: 'toast-top-right',
      timeOut: 2500,
      closeButton: true,
      progressBar: true,
    })),
    provideAnimations()
  ]
};
