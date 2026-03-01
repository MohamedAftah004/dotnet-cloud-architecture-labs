using CloudJobEngine.Application.Interfaces;

namespace CloudJobEngine.Infrastructure.Notifications;

public class FakeNotificationService : INotificationService
{
    private readonly ILogger<FakeNotificationService> _logger;

    public FakeNotificationService(ILogger<FakeNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(
        Guid userId,
        string message,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "🔔 Fake Notification to User {UserId}: {Message}",
            userId,
            message);

        return Task.CompletedTask;
    }
}