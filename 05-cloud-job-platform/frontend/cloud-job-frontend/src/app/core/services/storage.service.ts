import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  constructor(private http: HttpClient) {}

  uploadFileToS3(presignedUrl: string, file: File): Observable<void> {
    const headers = new HttpHeaders({
      'Content-Type': file.type || 'application/octet-stream',
    });

    return this.http.put<void>(presignedUrl, file, { headers });
  }
}
