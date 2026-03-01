using Amazon.S3;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudJobEngine.Application.Interfaces;
using System.Text.Json;

namespace CloudJobEngine.Infrastructure.Messaging;

public class SqsMessageQueueService : IMessageQueueService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl;

    public SqsMessageQueueService(
        IAmazonSQS sqsClient,
        IConfiguration configuration)
    {
        _sqsClient = sqsClient;
        _queueUrl = configuration["AWS:SQS:QueueUrl"]!;
    }

    public async Task PublishAsync(
     JobMessage message,
     CancellationToken cancellationToken)
    {
        var body = JsonSerializer.Serialize(message);

        var request = new SendMessageRequest
        {
            QueueUrl = _queueUrl,
            MessageBody = body
        };

        await _sqsClient.SendMessageAsync(request, cancellationToken);
    }
}