public class StoredFile
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string S3Key { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; }
    public string UploadedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}