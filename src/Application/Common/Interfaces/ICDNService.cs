namespace GerPros_Backend_API.Application.Common.Interfaces;

public interface ICDNService
{
    public string GenerateSignedUrl(string fileName, DateTime expiresOn);
}
