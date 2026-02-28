using Microsoft.AspNetCore.Http;

namespace AsyncUserProcessing.Application.DTOs
{
    public class RegisterUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = null!;
    }
}