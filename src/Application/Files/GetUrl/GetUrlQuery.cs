using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Files.GetUrl;

public record GetUrlQuery(string StorageKey, FileCategory Category, DateTime ExpiresOn, bool UseCDN): IRequest<string>;


public class GetUrlQueryHandler : IRequestHandler<GetUrlQuery, string>
{
    private readonly IFileStorageService _fileStorageService;

    public GetUrlQueryHandler(IFileStorageService fileStorageService )
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<string> Handle(GetUrlQuery request, CancellationToken cancellationToken)
    {
        return await _fileStorageService.GetUrlAsync(request.StorageKey,request.Category, request.ExpiresOn, request.UseCDN) ?? 
               throw new InvalidOperationException("File or Url not found");
    }
}
