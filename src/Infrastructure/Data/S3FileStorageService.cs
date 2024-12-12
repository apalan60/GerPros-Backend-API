using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Infrastructure.Data;

public class S3FileStorageService : IFileStorageService
{
    public Task<string?> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken)
    {
        return Task.FromResult("https://img1.wsimg.com/isteam/ip/e901cef4-3288-4b61-82d3-73973b1db24d/%E6%9C%AA%E5%91%BD%E5%90%8D-1_%E5%B7%A5%E4%BD%9C%E5%8D%80%E5%9F%9F%201-6690cab.png/:/cr=t:0%25,l:21.38%25,w:57.24%25,h:99.99%25/rs=w:400,h:400,cg:true")!;
        // private readonly string _bucketName;
        // private readonly IAmazonS3 _s3Client;
        //
        // public S3FileStorageService(IAmazonS3 s3Client, string bucketName)
        // {
        //     _s3Client = s3Client;
        //     _bucketName = bucketName;
        // }
        //
        // public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
        // {
        //     // 使用 TransferUtility 來簡化上傳流程
        //     var transferUtility = new TransferUtility(_s3Client);
        //
        //     var request = new TransferUtilityUploadRequest
        //     {
        //         InputStream = fileStream,
        //         Key = fileName,
        //         BucketName = _bucketName,
        //         ContentType = contentType
        //     };
        //
        //     await transferUtility.UploadAsync(request, cancellationToken);
        //
        //     // 回傳檔案的 S3 URL，通常是 https://{bucketName}.s3.amazonaws.com/{fileName}
        //     // 視 S3 設定而定，您可能需要整合 CloudFront 或透過預先簽署URL產生檔案連結。
        //     var fileUrl = $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
        //
        //     return fileUrl;
        // }
    }
}
// dotnet add package AWSSDK.S3



//  (Dependency Injection)
//program.cs
// using Amazon;
// using Amazon.Runtime;
// using Amazon.S3;
// using GerPros_Backend_API.Application.Common.Interfaces;
// using GerPros_Backend_API.Infrastructure.Services;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // 假設您已從組態檔取得下列參數
// string awsAccessKeyId = builder.Configuration["AWS:AccessKeyId"];
// string awsSecretAccessKey = builder.Configuration["AWS:SecretAccessKey"];
// string bucketName = builder.Configuration["AWS:BucketName"];
// string region = builder.Configuration["AWS:Region"];
//
// var s3Client = new AmazonS3Client(
//     new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey),
//     RegionEndpoint.GetBySystemName(region)
// );
//
// builder.Services.AddSingleton<IAmazonS3>(s3Client);
// builder.Services.AddSingleton<IFileStorageService>(new S3FileStorageService(s3Client, bucketName));
