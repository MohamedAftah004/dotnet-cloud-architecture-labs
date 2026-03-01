using CloudJobEngine.Domain.Base;
using CloudJobEngine.Domain.Enums;
using CloudJobEngine.Domain.Events;

namespace CloudJobEngine.Domain.Entities
{
    public class Job : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string FileKey { get; private set; } = string.Empty;
        public JobStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }

        private Job() { }


        public Job(Guid userId , string fileKey)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            FileKey = fileKey;
            Status = JobStatus.Pending;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new JobCreatedEvent(Id));
        }


        public void AttachFile(string fileKey)
        {
            if (string.IsNullOrWhiteSpace(fileKey))
                throw new ArgumentException("FileKey cannot be empty.");

            FileKey = fileKey;
        }

        public void StartProcessing()
        {
            if (Status != JobStatus.Pending)
                throw new InvalidOperationException("Only pending jobs can start processing.");

            Status = JobStatus.Processing;
            AddDomainEvent(new JobProcessingStartedEvent(Id));
        }

        public void Complete()
        {
            if (Status != JobStatus.Processing)
                throw new InvalidOperationException("Only processing jobs can be completed.");

            Status = JobStatus.Completed;
            ProcessedAt = DateTime.UtcNow;

            AddDomainEvent(new JobCompletedEvent(Id));
        }

        public void Fail()
        {
            if (Status == JobStatus.Completed)
                throw new InvalidOperationException("Completed job cannot fail.");

            Status = JobStatus.Failed;
            ProcessedAt = DateTime.UtcNow;

            AddDomainEvent(new JobFailedEvent(Id));
        }


    }
}
