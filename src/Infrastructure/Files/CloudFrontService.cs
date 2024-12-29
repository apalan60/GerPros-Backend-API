using Amazon.CloudFront;
using GerPros_Backend_API.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace GerPros_Backend_API.Infrastructure.Files;

public class CloudFrontService : ICDNService
{
    private readonly string _domainName;
    private readonly string _keyPairId;
    private readonly string _privateKey;

    public CloudFrontService(IOptions<CloudFrontSettings> settings)
    {
        _domainName = settings.Value.DomainName;
        _keyPairId = settings.Value.KeyPairId;
        _privateKey = settings.Value.PrivateKey;
#if DEBUG
        string? privateKeyPath = settings.Value.PrivateKeyPath;
        if (!string.IsNullOrEmpty(privateKeyPath))
        {
            _privateKey = File.ReadAllText($"{privateKeyPath}");
        }
#endif
    }


    /// <summary>
    /// Get CloudFront Signed Url 
    /// </summary>
    /// <param name="fileName">e.g. products/key </param>
    /// <param name="expiresOn">過期時間</param>
    /// <returns></returns>
    public string GenerateSignedUrl(string fileName, DateTime expiresOn)
    {
        string url = $"{_domainName}/{fileName}";

        using var privateKeyReader = new StringReader(_privateKey);

        try
        {
            string signedUrl = AmazonCloudFrontUrlSigner.GetCannedSignedURL(
                url,
                privateKeyReader,
                _keyPairId,
                expiresOn
            );

            return signedUrl;
        }
        catch (Exception e)
        {
            Console.WriteLine("exception:" + e);
            Console.WriteLine("privateKey:" + _privateKey);
            throw;
        }
    }
}
