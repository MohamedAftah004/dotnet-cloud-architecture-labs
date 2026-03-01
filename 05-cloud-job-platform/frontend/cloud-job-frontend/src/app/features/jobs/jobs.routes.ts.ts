import { Routes } from '@angular/router';
import { CreateJobComponent } from './pages/create-job/create-job';
import { JobDetailsComponent } from './pages/job-details/job-details';

export const JOBS_ROUTES: Routes = [
  { path: '', redirectTo: 'create', pathMatch: 'full' },
  { path: 'create', component: CreateJobComponent },
  { path: ':id', component: JobDetailsComponent },
];
