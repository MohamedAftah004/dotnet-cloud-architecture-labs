namespace AsyncUserProcessing.Domain.Interfaces
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(Stream fileStream, string fileName);
    }
}
