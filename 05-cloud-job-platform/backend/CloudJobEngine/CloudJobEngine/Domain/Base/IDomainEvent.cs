namespace CloudJobEngine.Domain.Base
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
