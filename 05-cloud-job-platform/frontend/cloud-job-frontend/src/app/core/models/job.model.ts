export interface Job {
  id: string;
  userId: string;
  fileKey: string;
  status: 'Pending' | 'Processing' | 'Completed' | 'Failed';
  createdAt: string;
  processedAt?: string | null;
}
