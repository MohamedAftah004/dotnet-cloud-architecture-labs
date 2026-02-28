using Amazon.SQS;
using Amazon.SQS.Model;
using AsyncUserProcessing.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[ApiController]
[Route("queue")]
public class QueueController : ControllerBase
{
    private readonly IAmazonSQS _sqs;
    private readonly AwsOptions _awsOptions;

    public QueueController(
        IAmazonSQS sqs,
        IOptions<AwsOptions> awsOptions)
    {
        _sqs = sqs;
        _awsOptions = awsOptions.Value;
    }

    [HttpGet("info")]
    public async Task<IActionResult> GetQueueInfo()
    {
        var response = await _sqs.GetQueueAttributesAsync(
            new GetQueueAttributesRequest
            {
                QueueUrl = _awsOptions.QueueUrl,
                AttributeNames = new List<string>
                {
                    "ApproximateNumberOfMessages",
                    "ApproximateNumberOfMessagesNotVisible"
                }
            });

        return Ok(new QueueInfoResponse
        {
            QueueUrl = _awsOptions.QueueUrl,
            ApproximateMessages =
                response.Attributes["ApproximateNumberOfMessages"],
            MessagesInFlight =
                response.Attributes["ApproximateNumberOfMessagesNotVisible"]
        });
    }
}