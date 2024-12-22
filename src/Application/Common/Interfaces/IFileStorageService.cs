﻿using GerPros_Backend_API.Domain;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Common.Interfaces;

public interface IFileStorageService
{
    /// <summary>
    /// get pre-signed url
    /// if the file is not existed, return null
    /// </summary>
    /// <param name="key"></param>
    /// <param name="fileCategory"></param>
    /// <param name="isPublic"></param>
    /// <returns></returns>
    Task<string?> GetUrlAsync(string key, FileCategory fileCategory, bool isPublic = false);

    /// <summary>
    /// 上傳檔案到遠端儲存（如S3）
    /// </summary>
    /// <param name="fileStream">要上傳的檔案串流</param>
    /// <param name="fileName">檔名</param>
    /// <param name="contentType">Content-Type</param>
    /// <param name="fileCategory"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="isPublic"></param>
    /// <returns>上傳後可供外部存取的檔案URL</returns>
    Task<FileStorageInfo> UploadAsync(Stream fileStream, string fileName, string contentType,
        FileCategory fileCategory,
        CancellationToken cancellationToken,
        bool isPublic = false);

    Task<string> GetUploadPreSignedUrl(string fileName, string contentType, FileCategory fileCategory, CancellationToken cancellationToken);
    
    Task<bool> DeleteAsync(string key, FileCategory fileCategory, CancellationToken cancellationToken);
    Task<bool> DeleteAllAsync(ICollection<string> key, FileCategory fileCategory, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string key, FileCategory fileCategory);
}
