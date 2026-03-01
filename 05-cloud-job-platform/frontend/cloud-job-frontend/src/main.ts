import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

import { App } from './app/app';
import { APP_ROUTES } from './app/app.routes';

bootstrapApplication(App, {
  providers: [provideHttpClient(), provideRouter(APP_ROUTES)],
}).catch((err) => console.error(err));
