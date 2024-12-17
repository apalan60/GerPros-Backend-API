using Amazon.S3;
using Amazon.S3.Model;
using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Enums;
using Microsoft.Extensions.Options;

namespace GerPros_Backend_API.Infrastructure.File;

public class S3FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Setting _s3Setting;

    public S3FileStorageService(IAmazonS3 s3Client, IOptions<S3Setting> s3Setting)
    {
        _s3Client = s3Client;
        _s3Setting = s3Setting.Value;
    }

    public async Task<string?> UploadAsync(Stream fileStream, string fileName, string contentType,
        FileCategory fileCategory,
        CancellationToken cancellationToken)
    {
        if (fileStream.Length == 0)
        {
            throw new ArgumentException("File is empty.", fileName);
        }
        
        var key = Guid.NewGuid();
        var request = new PutObjectRequest
        {
            BucketName = _s3Setting.BucketName,
            Key = $"{fileCategory.ToString()}/{key}",
            InputStream = fileStream,
            ContentType = contentType,
            Metadata =
            {
                ["fileName"] = fileName,
            }
        };
        
        PutObjectResponse? response = await _s3Client.PutObjectAsync(request, cancellationToken);
        // todo get presigned url
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


    public static async Task<bool> BucketExistsAsync(IAmazonS3 client, string bucketName)
    {
        try
        {
            var response = await client.ListBucketsAsync();
            return response.Buckets.Any(
                b => string.Equals(b.BucketName, bucketName, StringComparison.OrdinalIgnoreCase));
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error checking bucket existence: '{ex.Message}'");
            return false;
        }
    }

    public static async Task<bool> CreateBucketAsync(IAmazonS3 client, string bucketName)
    {
        try
        {
            var request = new PutBucketRequest { BucketName = bucketName, UseClientRegion = true, };

            var response = await client.PutBucketAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error creating bucket: '{ex.Message}'");
            return false;
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
