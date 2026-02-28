public sealed class QueueInfoResponse
{
    public string QueueUrl { get; init; } = string.Empty;
    public string ApproximateMessages { get; init; } = string.Empty;
    public string MessagesInFlight { get; init; } = string.Empty;
}