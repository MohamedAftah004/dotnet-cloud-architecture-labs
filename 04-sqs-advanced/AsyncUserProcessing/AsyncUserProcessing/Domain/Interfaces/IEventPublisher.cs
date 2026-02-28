namespace AsyncUserProcessing.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event);
    }
}
