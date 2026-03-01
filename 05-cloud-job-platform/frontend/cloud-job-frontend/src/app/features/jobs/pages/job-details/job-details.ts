import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JobService } from '../../../../core/services/job.service';
import { Job } from '../../../../core/models/job.model';
import { StatusBadgeComponent } from '../../../../shared/components/status-badge/status-badge';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-job-details',
  templateUrl: './job-details.html',
  styleUrls: ['./job-details.css'],
  imports: [StatusBadgeComponent, CommonModule],
  standalone: true,
})
export class JobDetailsComponent implements OnInit {
  job?: Job;

  constructor(
    private route: ActivatedRoute,
    private jobService: JobService,
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadJob(id);
    }
  }

  loadJob(id: string) {
    console.log('Loading job with id:', id);

    this.jobService.getJobById(id).subscribe({
      next: (data) => {
        this.job = data;
      },
    });
  }

  getStatusColor(status?: string): string {
    switch (status) {
      case 'Pending':
        return 'orange';
      case 'Processing':
        return 'blue';
      case 'Completed':
        return 'green';
      case 'Failed':
        return 'red';
      default:
        return 'black';
    }
  }
}
