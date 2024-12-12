namespace GerPros_Backend_API.Application.Common.Interfaces;

public interface IFileStorageService
{
    /// <summary>
    /// 上傳檔案到遠端儲存（如S3）
    /// </summary>
    /// <param name="fileStream">要上傳的檔案串流</param>
    /// <param name="fileName">檔名</param>
    /// <param name="contentType">Content-Type</param>
    /// <param name="cancellationToken"></param>
    /// <returns>上傳後可供外部存取的檔案URL</returns>
    Task<string?> UploadAsync(Stream fileStream, string fileName, string contentType,
        CancellationToken cancellationToken);
}
