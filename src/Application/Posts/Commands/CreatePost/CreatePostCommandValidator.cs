using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostItemCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public CreatePostCommandValidator(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
        RuleFor(v => v.Title)
            .MaximumLength(30)
            .NotEmpty();

        RuleFor(v => v.CoverImage)
            .NotNull()
            .When(v => v.ImagesKeys == null)
            .WithMessage("ImageKeys is required when CoverImage is not null.");
        
        RuleFor(v => v.Content != null && ContentHasImg(v))
            .NotNull()
            .When(v => v.ImagesKeys == null)
            .WithMessage("ImageKeys is required when CoverImage is not null.");

        RuleFor(v => v.ImagesKeys)
            .MustAsync(ImagesKeysExists);
    }

    private static bool ContentHasImg(CreatePostItemCommand v) => v.Content != null && v.Content.Contains("<img src=");

    private async Task<bool> ImagesKeysExists(string? imageKey, CancellationToken cancellationToken)
    {
        if (imageKey == null)
        {
            return true;
        }

        return await _fileStorageService.ExistsAsync(imageKey, FileCategory.Post);
    }
}
