using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3Advanced.Infrastructure.Repository;
using S3Advanced.Infrastructure.Storage;

namespace S3Advanced.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IFileRepository _fileRepository;

        public FilesController(
            IStorageService storageService,
            IFileRepository fileRepository)
        {
            _storageService = storageService;
            _fileRepository = fileRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var key = await _storageService.UploadFileAsync(file);

            var metadata = new StoredFile
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                S3Key = key,
                Size = file.Length,
                ContentType = file.ContentType,
                UploadedBy = "mohamed abdelfattah",
                CreatedAt = DateTime.UtcNow
            };

            await _fileRepository.AddAsync(metadata);

            return Ok(metadata);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var fileMetadata = await _fileRepository.GetByIdAsync(id);

            if (fileMetadata == null)
                return NotFound("File not found");

            var stream = await _storageService.GetFileAsync(fileMetadata.S3Key);

            return File(
                stream,
                fileMetadata.ContentType,
                fileMetadata.FileName
            );
        }
    }
}
