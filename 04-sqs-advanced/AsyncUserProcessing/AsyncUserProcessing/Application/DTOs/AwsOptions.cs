namespace AsyncUserProcessing.Application.DTOs
{
    public class AwsOptions
    {
        public string ServiceUrl { get; set; } = default!;
        public string BucketName { get; set; } = default!;
        public string QueueUrl { get; set; } = default!;
        public string AccessKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
    }
}
