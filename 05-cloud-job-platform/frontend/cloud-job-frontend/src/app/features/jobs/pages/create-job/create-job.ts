import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { JobService } from '../../../../core/services/job.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-job',
  templateUrl: './create-job.html',
  styleUrls: ['./create-job.css'],
  imports: [CommonModule],
  standalone: true,
})
export class CreateJobComponent {
  selectedFile!: File;
  isLoading = false;

  constructor(
    private jobService: JobService,
    private router: Router,
  ) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  createJob() {
    if (!this.selectedFile) return;

    this.isLoading = true;

    const fileKey = `${crypto.randomUUID()}-${this.selectedFile.name}`;

    this.jobService
      .createJob({
        userId: crypto.randomUUID(),
        fileKey: fileKey,
      })
      .subscribe({
        next: (jobId) => {
          // this.router.navigateByUrl(`/jobs/${jobId}`);
          this.router.navigate(['/jobs', jobId]);
        },
        error: () => {
          this.isLoading = false;
        },
      });
  }
}
