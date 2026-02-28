public sealed class UserDetailsResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public UserProcessingStatus Status { get; init; }
    public string? ImageUrl { get; init; }
}