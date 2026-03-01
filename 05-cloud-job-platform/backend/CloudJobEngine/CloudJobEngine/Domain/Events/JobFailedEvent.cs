using CloudJobEngine.Domain.Base;

namespace CloudJobEngine.Domain.Events
{
    public class JobFailedEvent : IDomainEvent
    {
        public Guid JobId { get; }
        public DateTime OccurredOn { get; }

        public JobFailedEvent(Guid jobId)
        {
            JobId = jobId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
