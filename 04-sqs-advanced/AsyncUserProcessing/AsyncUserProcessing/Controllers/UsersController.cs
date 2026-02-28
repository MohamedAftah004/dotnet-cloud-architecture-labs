using AsyncUserProcessing.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsyncUserProcessing.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly UserAppService _service;

        public UsersController(UserAppService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] RegisterUserRequest request)
        {
            if (request.Image == null || request.Image.Length == 0)
                return BadRequest("Image is required");

            using var stream = request.Image.OpenReadStream();

            var user = await _service.RegisterAsync(
                request.Name,
                request.Email,
                stream,
                request.Image.FileName);

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Status,
                user.ImageUrl
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _service.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }


    }
}
