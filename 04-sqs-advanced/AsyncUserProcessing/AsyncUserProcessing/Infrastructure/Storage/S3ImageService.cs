using Amazon.S3;
using Amazon.S3.Model;
using AsyncUserProcessing.Application.DTOs;
using AsyncUserProcessing.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace AsyncUserProcessing.Infrastructure.Storage
{
    public class S3ImageStorage : IImageStorage
    {
        private readonly IAmazonS3 _s3;
        private readonly AwsOptions _options;

        public S3ImageStorage(
            IAmazonS3 s3,
            IOptions<AwsOptions> options)
        {
            _s3 = s3;
            _options = options.Value;
        }

        public async Task<string> SaveAsync(Stream fileStream, string fileName)
        {
            var key = $"{Guid.NewGuid()}_{fileName}";

            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = "image/jpeg"
            };

            await _s3.PutObjectAsync(request);

            return key;
        }
    }
}