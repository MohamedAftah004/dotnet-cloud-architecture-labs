using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;

namespace MiniOrderApi.Messaging
{
    public class SqsPublisher
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _queueUrl;

        public SqsPublisher(IAmazonSQS sqs, IConfiguration config)
        {
            _sqs = sqs;
            _queueUrl = config["AWS:QueueUrl"]!;
        }

        public async Task PublishAsync<T>(T message)
        {
            var body = JsonSerializer.Serialize(message);

            await _sqs.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = body,
                DelaySeconds = 10
            });
        }

    }
}
