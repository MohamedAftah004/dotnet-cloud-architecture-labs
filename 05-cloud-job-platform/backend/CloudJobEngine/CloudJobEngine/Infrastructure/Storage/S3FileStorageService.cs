using Amazon.S3;
using Amazon.S3.Model;
using CloudJobEngine.Application.Interfaces;

namespace CloudJobEngine.Infrastructure.Storage
{
    public class S3FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3FileStorageService(
            IAmazonS3 s3Client,
            IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:S3:BucketName"]
                ?? throw new ArgumentNullException("AWS:S3:BucketName missing");
        }

        public Task<string> GeneratePresignedUploadUrlAsync(
            string fileKey,
            CancellationToken cancellationToken)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(15),
                ContentType = "application/octet-stream"
            };

            var url = _s3Client.GetPreSignedURL(request);

            return Task.FromResult(url);
        }
    }
}
