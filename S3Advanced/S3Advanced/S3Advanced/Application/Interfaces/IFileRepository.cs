public interface IFileRepository
{
    Task AddAsync(StoredFile file);
    Task<StoredFile?> GetByIdAsync(Guid id);
}