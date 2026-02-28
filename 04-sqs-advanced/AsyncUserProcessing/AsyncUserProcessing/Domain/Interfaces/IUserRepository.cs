
namespace AsyncUserProcessing.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task UpdateAsync(User user);
    }
}
