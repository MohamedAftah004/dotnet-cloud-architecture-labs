namespace CloudJobEngine.Application.Interfaces;

public interface INotificationService
{
    Task SendAsync(
        Guid userId,
        string message,
        CancellationToken cancellationToken);
}