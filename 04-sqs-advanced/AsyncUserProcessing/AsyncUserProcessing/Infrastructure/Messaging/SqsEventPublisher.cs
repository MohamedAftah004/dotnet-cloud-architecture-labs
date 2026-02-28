using Amazon.SQS;
using Amazon.SQS.Model;
using AsyncUserProcessing.Domain.Interfaces;
using System.Text.Json;

namespace AsyncUserProcessing.Infrastructure.Messaging
{
    public class SqsEventPublisher : IEventPublisher
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _queueUrl;

        public SqsEventPublisher(IAmazonSQS sqs, IConfiguration config)
        {
            _sqs = sqs;
            _queueUrl = config["AWS:QueueUrl"]!;

        }

        public async Task PublishAsync<T>(T @event)
        {
            var body = JsonSerializer.Serialize(@event);

            await _sqs.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = body
            });
        }
    }
}