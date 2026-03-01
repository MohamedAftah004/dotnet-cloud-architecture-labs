using CloudJobEngine.Domain.Base;

namespace CloudJobEngine.Domain.Events;

public class JobCreatedEvent : IDomainEvent
{
    public Guid JobId { get; }
    public DateTime OccurredOn { get; }

    public JobCreatedEvent(Guid jobId)
    {
        JobId = jobId;
        OccurredOn = DateTime.UtcNow;
    }
}