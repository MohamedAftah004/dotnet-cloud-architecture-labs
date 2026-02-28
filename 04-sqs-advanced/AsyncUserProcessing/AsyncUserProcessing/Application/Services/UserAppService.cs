using AsyncUserProcessing.Domain.Events;
using AsyncUserProcessing.Domain.Interfaces;

public class UserAppService
{
    private readonly IUserRepository _repository;
    private readonly IImageStorage _imageStorage;
    private readonly IEventPublisher _publisher;

    public UserAppService(
        IUserRepository repository,
        IImageStorage imageStorage,
        IEventPublisher publisher)
    {
        _repository = repository;
        _imageStorage = imageStorage;
        _publisher = publisher;
    }

    public async Task<User> RegisterAsync(
        string name,
        string email,
        Stream imageStream,
        string fileName)
    {
        var imageKey = await _imageStorage.SaveAsync(imageStream, fileName);

        var user = new User(name, email);

        await _repository.AddAsync(user);

        await _publisher.PublishAsync(new UserRegisteredEvent
        {
            UserId = user.Id,
            Email = user.Email,
            ImageKey = imageKey
        });

        return user;
    }


    public async Task<UserDetailsResponse?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

        return new UserDetailsResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Status = user.Status,
            ImageUrl = user.ImageUrl
        };
    }

}