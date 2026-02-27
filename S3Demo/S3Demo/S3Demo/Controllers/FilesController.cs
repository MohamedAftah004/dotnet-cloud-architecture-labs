using Microsoft.AspNetCore.Mvc;
using S3Demo.Services;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IS3Service _s3Service;

    public FilesController(IS3Service s3Service)
    {
        _s3Service = s3Service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        await _s3Service.UploadFileAsync(file);
        return Ok("Uploaded Successfully");
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> Get(string fileName)
    {
        var fileBytes = await _s3Service.GetFileAsync(fileName);
        return File(fileBytes, "application/octet-stream");
    }
}