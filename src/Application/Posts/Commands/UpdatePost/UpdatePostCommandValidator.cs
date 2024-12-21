using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public UpdatePostCommandValidator(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
        RuleFor(v => v.Title)
            .MaximumLength(30)
            .NotEmpty();

        RuleFor(v => v.CoverImage)
            .NotNull()
            .When(v => v.FileStorageInfo == null)
            .WithMessage("ImageKeys is required when CoverImage is not null.");
        
        RuleFor(v => v.Content != null && ContentHasImg(v))
            .NotNull()
            .When(v => v.FileStorageInfo == null)
            .WithMessage("ImageKeys is required when CoverImage is not null.");

        RuleFor(v => v.FileStorageInfo)
            .MustAsync(ImagesKeysExists)
            .WithMessage("可能原因為: 1. 檔案上傳中，請稍後再試 2. 檔案不存在");
    }

    private static bool ContentHasImg(UpdatePostCommand v) => v.Content != null && v.Content.Contains("<img src=");

    private async Task<bool> ImagesKeysExists(FileStorageInfo[]? fileStorageInfo, CancellationToken cancellationToken)
    {
        if (fileStorageInfo == null || fileStorageInfo.Length == 0)
        {
            return false;
        }
        
        foreach (var file in fileStorageInfo)
        {
            if (!await _fileStorageService.ExistsAsync(file.ImageKey, FileCategory.Post))
            {
                return false;
            }
        }
        return true;
    }
}
