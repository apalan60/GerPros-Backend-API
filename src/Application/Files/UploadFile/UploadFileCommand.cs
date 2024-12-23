using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Domain;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Files.UploadFile;

public class UploadFileCommand : IRequest<FileStorageInfo>
{
    public UploadedFile File { get; init; } = null!;
    
    public required FileCategory Category { get; init; }
}


public class UploadFileCommandHandler(IFileStorageService fileStorageService) : IRequestHandler<UploadFileCommand, FileStorageInfo>
{
    public async Task<FileStorageInfo> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        return await fileStorageService.UploadAsync(
            request.File.Content ?? throw new InvalidOperationException(),
            request.File.FileName ?? throw new InvalidOperationException(), 
            request.File.ContentType ?? "application/octet-stream",
            request.Category,
            cancellationToken
        );
    }
} 

