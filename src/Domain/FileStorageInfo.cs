namespace GerPros_Backend_API.Domain;

public class FileStorageInfo
{
    public required string Name{ get; set; } = null!;
    public required string StorageKey { get; init; } = null!;
}
