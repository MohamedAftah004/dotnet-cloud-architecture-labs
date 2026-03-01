using Amazon.SQS;
using Amazon.SQS.Model;
using CloudJobEngine.Application.Commands.CompleteJob;
using CloudJobEngine.Application.Commands.StartJobProcessing;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CloudJobEngine.Infrastructure.Messaging;

public class SqsWorkerBackgroundService : BackgroundService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SqsWorkerBackgroundService> _logger;
    private readonly string _queueUrl;

    public SqsWorkerBackgroundService(
        IAmazonSQS sqsClient,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<SqsWorkerBackgroundService> logger)
    {
        _sqsClient = sqsClient;
        _scopeFactory = scopeFactory;
        _logger = logger;

        _queueUrl = configuration["AWS:SQS:QueueUrl"]
            ?? throw new ArgumentNullException("AWS:SQS:QueueUrl is missing in configuration");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("********************** SQS Worker Started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var response = await _sqsClient.ReceiveMessageAsync(
                    new ReceiveMessageRequest
                    {
                        QueueUrl = _queueUrl,
                        MaxNumberOfMessages = 5,
                        WaitTimeSeconds = 10
                    },
                    stoppingToken);

                if (response.Messages == null || response.Messages.Count == 0)
                    continue;

                foreach (var message in response.Messages)
                {
                    await ProcessMessage(message, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "********************** Error inside SQS Worker");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    private async Task ProcessMessage(
        Message message,
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider
            .GetRequiredService<IMediator>();

        var payload = JsonSerializer.Deserialize<JobMessage>(message.Body);

        if (payload == null)
        {
            _logger.LogWarning("**********************  Invalid message format");
            return;
        }

        var jobId = payload.JobId;

        _logger.LogInformation("**********************  Processing Job {JobId}", jobId);


        await mediator.Send(
            new StartJobProcessingCommand(jobId),
            cancellationToken);

        await Task.Delay(2000, cancellationToken);

        await mediator.Send(
            new CompleteJobCommand(jobId),
            cancellationToken);

        await _sqsClient.DeleteMessageAsync(
            _queueUrl,
            message.ReceiptHandle,
            cancellationToken);

        _logger.LogInformation("**********************  Job {JobId} Completed & Message Deleted", jobId);
    }

    private record JobMessage(Guid JobId);
}