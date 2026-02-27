using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using MiniOrderApi.Models;
using MiniOrderApi.Services;

namespace MiniOrderApi.BackgroundWorkers;

public class OrderPaymentWorker : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IConfiguration _config;
    private readonly OrderService _orderService;

    public OrderPaymentWorker(
        IAmazonSQS sqs,
        IConfiguration config,
        OrderService orderService)
    {
        _sqs = sqs;
        _config = config;
        _orderService = orderService;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrl = _config["AWS:QueueUrl"];

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 1,
                WaitTimeSeconds = 10
            });

            if (response.Messages == null || !response.Messages.Any())
                continue;

            foreach (var message in response.Messages)
            {
                var orderEvent =
                    JsonSerializer.Deserialize<OrderCreatedEvent>(message.Body);

                Console.WriteLine($"Processing payment for {orderEvent!.OrderId}");

                await Task.Delay(3000);

                _orderService.MarkAsPaid(orderEvent.OrderId);

                await _sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
            }
        }
    }

}