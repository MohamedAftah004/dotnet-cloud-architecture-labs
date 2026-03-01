import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { CreateJobRequest } from '../models/create-job.model';
import { Job } from '../models/job.model';

@Injectable({
  providedIn: 'root',
})
export class JobService {
  constructor(private api: ApiService) {}

  createJob(request: CreateJobRequest): Observable<string> {
    return this.api.post<string>('jobs', request);
  }

  getJobById(id: string): Observable<Job> {
    return this.api.get<Job>(`jobs/${id}`);
  }
}
