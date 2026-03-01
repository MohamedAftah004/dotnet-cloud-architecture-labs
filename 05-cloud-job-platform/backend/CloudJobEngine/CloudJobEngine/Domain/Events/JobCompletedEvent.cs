using CloudJobEngine.Domain.Base;

namespace CloudJobEngine.Domain.Events
{
    public class JobCompletedEvent : IDomainEvent
    {
        public Guid JobId { get; }
        public DateTime OccurredOn { get; }

        public JobCompletedEvent(Guid jobId)
        {
            JobId = jobId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
