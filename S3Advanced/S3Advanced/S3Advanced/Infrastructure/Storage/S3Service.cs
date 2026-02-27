using Amazon.S3;
using Amazon.S3.Model;

namespace S3Advanced.Infrastructure.Storage
{
    public class S3Service : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IAmazonS3 s3Client, IConfiguration config)
        {
            _s3Client = s3Client;
            _bucketName = config["AWS:BucketName"];
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var key = $"{Guid.NewGuid()}-{file.FileName}";

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(request);

            return key;
        }

        public async Task<Stream> GetFileAsync(string key)
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            return response.ResponseStream;
        }
    }
}
