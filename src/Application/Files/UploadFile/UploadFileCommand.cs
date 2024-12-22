using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Files.UploadFile;

public class UploadFileCommand : IRequest<string>
{
    public UploadedFile File { get; init; } = null!;
    
    public required FileCategory Category { get; init; }
    
    public bool IsPublic { get; init; } = false;
}


public class UploadFileCommandHandler(IFileStorageService fileStorageService) : IRequestHandler<UploadFileCommand, string>
{
    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        return await fileStorageService.UploadAsync(
            request.File.Content ?? throw new InvalidOperationException(),
            request.File.FileName ?? throw new InvalidOperationException(), 
            request.File.ContentType ?? "application/octet-stream",
            request.Category,
            cancellationToken,
            request.IsPublic
        );
    }
} 

