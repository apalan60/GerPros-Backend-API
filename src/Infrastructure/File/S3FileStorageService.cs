using Amazon.S3;
using Amazon.S3.Model;
using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Enums;
using Microsoft.Extensions.Options;

namespace GerPros_Backend_API.Infrastructure.File;

public class S3FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Settings _s3Settings;

    public S3FileStorageService(IAmazonS3 s3Client, IOptions<S3Settings> s3Setting)
    {
        _s3Client = s3Client;
        _s3Settings = s3Setting.Value;
    }

    
    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType,
        FileCategory fileCategory,
        CancellationToken cancellationToken)
    {
        if (fileStream.Length == 0)
            throw new ArgumentException("File is empty.", fileName);

        var key = Guid.NewGuid();
        var request = new PutObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = $"{fileCategory.ToString()}/{key}",
            InputStream = fileStream,
            ContentType = contentType,
        };

        await CreateBucketIfNotExisted();

        try
        {
            await _s3Client.PutObjectAsync(request, cancellationToken);
            return key.ToString();
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            throw new Exception($"Error uploading file: {amazonS3Exception.Message}");
        }
    }

    public Task<string> GetUploadPreSignedUrl(string fileName, string contentType, FileCategory fileCategory,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetUrlAsync(string key, FileCategory fileCategory)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = $"{fileCategory.ToString()}/{key}",
            Protocol = Protocol.HTTPS,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(5),
        };

        try
        {
            var url = await _s3Client.GetPreSignedURLAsync(request);
            return url;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Error getting URL: {e.Message}");
            return null;
        }
    }

    public Task<bool> DeleteAsync(string key, FileCategory fileCategory, CancellationToken cancellationToken)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _s3Settings.BucketName, Key = $"{fileCategory.ToString()}/{key}",
        };

        return _s3Client.DeleteObjectAsync(request, cancellationToken)
            .ContinueWith(task => task.Result.HttpStatusCode == System.Net.HttpStatusCode.NoContent, cancellationToken);
    }

    public Task<bool> DeleteAllAsync(ICollection<string> key, FileCategory fileCategory, CancellationToken cancellationToken)
    {
        var request = new DeleteObjectsRequest
        {
            BucketName = _s3Settings.BucketName,
            Objects = key.Select(k => new KeyVersion { Key = $"{fileCategory.ToString()}/{k}" }).ToList(),
        };

        return _s3Client.DeleteObjectsAsync(request, cancellationToken)
            .ContinueWith(task => task.Result.HttpStatusCode == System.Net.HttpStatusCode.OK, cancellationToken);
    }


    public async Task<bool> ExistsAsync(string key, FileCategory fileCategory)
    {
        var fileKey = $"{fileCategory.ToString()}/{key}";
        var request = new GetObjectMetadataRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = fileKey,
        };
        
        try
        {
            await _s3Client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            throw new Exception($"Error checking file existence: '{ex.Message}'");
        }
    }

    private async Task CreateBucketIfNotExisted()
    {
        var bucketExists = await BucketExistsAsync(_s3Client, _s3Settings.BucketName);
        if (!bucketExists)
        {
            var result = await CreateBucketAsync(_s3Client, _s3Settings.BucketName);
            if (!result)
            {
                throw new Exception("Error creating bucket.");
            }
        }
    }


    /// <summary>
    /// Shows how to download an object from an Amazon S3 bucket to the
    /// local computer.
    /// </summary>
    /// <param name="client">An initialized Amazon S3 client object.</param>
    /// <param name="bucketName">The name of the bucket where the object is
    /// currently stored.</param>
    /// <param name="objectName">The name of the object to download.</param>
    /// <param name="filePath">The path, including filename, where the
    /// downloaded object will be stored.</param>
    /// <returns>A boolean value indicating the success or failure of the
    /// download process.</returns>
    public static async Task<bool> DownloadObjectFromBucketAsync(
        IAmazonS3 client,
        string bucketName,
        string objectName,
        string filePath)
    {
        // Create a GetObject request
        var request = new GetObjectRequest { BucketName = bucketName, Key = objectName, };

        // Issue request and remember to dispose of the response
        using GetObjectResponse response = await client.GetObjectAsync(request);

        try
        {
            // Save object to local file
            await response.WriteResponseStreamToFileAsync($"{filePath}\\{objectName}", true, CancellationToken.None);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error saving {objectName}: {ex.Message}");
            return false;
        }
    }


    private static async Task<bool> BucketExistsAsync(IAmazonS3 client, string bucketName)
    {
        try
        {
            var response = await client.ListBucketsAsync();
            return response.Buckets.Any(
                b => string.Equals(b.BucketName, bucketName, StringComparison.OrdinalIgnoreCase));
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Error checking bucket existence: '{ex.Message}'");
        }
    }

    private static async Task<bool> CreateBucketAsync(IAmazonS3 client, string bucketName)
    {
        try
        {
            var request = new PutBucketRequest { BucketName = bucketName, UseClientRegion = true, };

            var response = await client.PutBucketAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Error creating bucket: '{ex.Message}'");
        }
    }

    public static async Task<bool> DeleteBucketAsync(IAmazonS3 client, string bucketName)
    {
        try
        {
            var request = new DeleteBucketRequest { BucketName = bucketName, };

            var response = await client.DeleteBucketAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error deleting bucket: '{ex.Message}'");
            return false;
        }
    }
}
