public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? ImageUrl { get; private set; }
    public UserProcessingStatus Status { get; private set; }

    private User() { }

    public User(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Status = UserProcessingStatus.Pending;
    }

    public void MarkProcessing()
        => Status = UserProcessingStatus.Processing;

    public void MarkCompleted(string imageUrl)
    {
        ImageUrl = imageUrl;
        Status = UserProcessingStatus.Completed;
    }

    public void MarkFailed()
        => Status = UserProcessingStatus.Failed;
}