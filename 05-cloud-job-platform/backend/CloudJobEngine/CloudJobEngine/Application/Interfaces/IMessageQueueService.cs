namespace CloudJobEngine.Application.Interfaces;

public interface IMessageQueueService
{
    Task PublishAsync(JobMessage message, CancellationToken cancellationToken);
}