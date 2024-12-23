namespace GerPros_Backend_API.Infrastructure.Files;

public sealed class CloudFrontSettings
{
    public string DomainName { get; set; } = string.Empty;
    public string KeyPairId { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;

    public string? PrivateKeyPath { get; set; }
}
