using AsyncUserProcessing.Domain.Interfaces;

namespace AsyncUserProcessing.Infrastructure.Persistence
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static readonly List<User> _users = new();

        public Task AddAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_users.FirstOrDefault(x => x.Id == id));
        }

        public Task UpdateAsync(User user)
        {
            return Task.CompletedTask;
        }
    }
}