namespace GerPros_Backend_API.Infrastructure.File;

public sealed class S3Setting
{
    public string Region { get; init; } = string.Empty;

    public string BucketName { get; init; } = string.Empty;
}
