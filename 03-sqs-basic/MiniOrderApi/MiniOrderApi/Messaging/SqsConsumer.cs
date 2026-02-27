using Amazon.SQS;
using Amazon.SQS.Model;

namespace MiniOrderApi.Messaging;

public class SqsConsumer
{
    private readonly IAmazonSQS _sqs;
    private readonly string _queueUrl;

    public SqsConsumer(IAmazonSQS sqs, IConfiguration config)
    {
        _sqs = sqs;
        _queueUrl = config["AWS:QueueUrl"]!;
    }

    public async Task<List<Message>> ReceiveAsync()
    {
        var response = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = _queueUrl,
            MaxNumberOfMessages = 5,
            WaitTimeSeconds = 10
        });

        return response.Messages;
    }

    public async Task DeleteAsync(string receiptHandle)
    {
        await _sqs.DeleteMessageAsync(_queueUrl, receiptHandle);
    }
}