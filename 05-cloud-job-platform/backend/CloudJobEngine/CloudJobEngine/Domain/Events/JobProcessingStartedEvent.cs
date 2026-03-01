using CloudJobEngine.Domain.Base;

namespace CloudJobEngine.Domain.Events
{
    public class JobProcessingStartedEvent : IDomainEvent
    {
        public Guid JobId { get; }
        public DateTime OccurredOn { get; }

        public JobProcessingStartedEvent(Guid jobId)
        {
            JobId = jobId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
