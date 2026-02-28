using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using AsyncUserProcessing.Application.DTOs;
using AsyncUserProcessing.Domain.Events;
using AsyncUserProcessing.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;

public class UserProcessingWorker : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IAmazonS3 _s3;
    private readonly AwsOptions _awsOptions;
    private readonly ILogger<UserProcessingWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EmailOptions _emailOptions;
    public UserProcessingWorker(
        IAmazonSQS sqs,
        IAmazonS3 s3,
        IOptions<AwsOptions> awsOptions,
        ILogger<UserProcessingWorker> logger,
        IServiceScopeFactory scopeFactory,
        IOptions<EmailOptions> emailOptions
        )
    {
        _sqs = sqs;
        _s3 = s3;
        _awsOptions = awsOptions.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;
        _emailOptions = emailOptions.Value;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("User Processing Worker started...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var response = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = _awsOptions.QueueUrl,
                    MaxNumberOfMessages = 1,
                    WaitTimeSeconds = 20
                }, stoppingToken);

                if (response.Messages == null || !response.Messages.Any())
                    continue;
                foreach (var message in response.Messages)
                {
                    await ProcessMessage(message, stoppingToken);

                    await _sqs.DeleteMessageAsync(new DeleteMessageRequest
                    {
                        QueueUrl = _awsOptions.QueueUrl,
                        ReceiptHandle = message.ReceiptHandle
                    }, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error in worker loop");
            }
        }
    }

    private async Task ProcessMessage(Message message, CancellationToken token)
    {
        using var scope = _scopeFactory.CreateScope();

        var repository =
            scope.ServiceProvider.GetRequiredService<IUserRepository>();

        var domainEvent =
            JsonSerializer.Deserialize<UserRegisteredEvent>(message.Body);

        if (domainEvent == null) return;

        var user = await repository.GetByIdAsync(domainEvent.UserId);
        if (user == null) return;

        try
        {
            user.MarkProcessing();
            await repository.UpdateAsync(user);

            var getObject = await _s3.GetObjectAsync(
                _awsOptions.BucketName,
                domainEvent.ImageKey,
                token);

            using var memory = new MemoryStream();
            await getObject.ResponseStream.CopyToAsync(memory, token);

            var resizedBytes = ResizeImage(memory.ToArray());

            var newKey = $"{Guid.NewGuid()}.jpg";

            using var stream = new MemoryStream(resizedBytes);

            await _s3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _awsOptions.BucketName,
                Key = newKey,
                InputStream = stream,
                ContentType = "image/jpeg"
            }, token);

            var imageUrl =
                $"{_awsOptions.ServiceUrl}/{_awsOptions.BucketName}/{newKey}";

            user.MarkCompleted(imageUrl);
            await repository.UpdateAsync(user);

            await SendWelcomeEmail(user.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing user {UserId}", user.Id);
            user.MarkFailed();
            await repository.UpdateAsync(user);
        }
    }

    private byte[] ResizeImage(byte[] imageBytes)
    {
        using var image = Image.Load(imageBytes);

        const int maxWidth = 300;

        var ratio = (double)maxWidth / image.Width;
        var newHeight = (int)(image.Height * ratio);

        image.Mutate(x => x.Resize(maxWidth, newHeight));

        using var output = new MemoryStream();

        image.Save(output, new JpegEncoder
        {
            Quality = 75
        });

        return output.ToArray();
    }
    private async Task SendWelcomeEmail(string email)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            _emailOptions.SenderName,
            _emailOptions.SenderEmail));

        message.To.Add(MailboxAddress.Parse(email));

        message.Subject = "Welcome 🎉";

        message.Body = new TextPart("html")
        {
            Text = $@"
            <h2>Welcome!</h2>
            <p>Your image has been processed successfully.</p>
            <p>Thanks for using our service ❤️</p>"
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _emailOptions.SmtpServer,
            _emailOptions.Port,
            SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            _emailOptions.Username,
            _emailOptions.Password);

        await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }
}