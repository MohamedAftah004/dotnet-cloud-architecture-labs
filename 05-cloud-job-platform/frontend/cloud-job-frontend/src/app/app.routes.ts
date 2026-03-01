import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: '',
    redirectTo: '/jobs',
    pathMatch: 'full',
  },
  {
    path: 'jobs',
    loadChildren: () => import('./features/jobs/jobs.routes.ts').then((m) => m.JOBS_ROUTES),
  },
  {
    path: '**',
    redirectTo: '/jobs',
  },
];
