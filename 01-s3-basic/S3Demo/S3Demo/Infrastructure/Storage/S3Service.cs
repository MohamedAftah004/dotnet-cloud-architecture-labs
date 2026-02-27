using Amazon.S3;
using Amazon.S3.Model;
using S3Demo.Services;

namespace S3Demo.Infrastructure.Storage
{
    public class S3Service : IS3Service
    {

        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _config;

        public S3Service(IConfiguration config)
        {
            _config = config;

            var awsConfig = new AmazonS3Config
            {
                ServiceURL = "http://localhost:4566",
                ForcePathStyle = true,
                UseHttp = true
            };
            _s3Client = new AmazonS3Client(
                _config["AWS:AccessKey"],
                _config["AWS:SecretKey"],
                awsConfig);

        }


        public async Task UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _config["AWS:BucketName"],
                Key = file.FileName,
                InputStream = stream,
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(request);
        }


        public async Task<byte[]> GetFileAsync(string fileName)
        {
            var response = await _s3Client.GetObjectAsync(
                _config["AWS:BucketName"],
                fileName);

            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

    }
}
