﻿using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public CreatePostCommandValidator(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
        RuleFor(v => v.Title)
            .MaximumLength(30)
            .NotEmpty();
        
        RuleFor(v => v.Description)
            .MaximumLength(500)
            .WithMessage("簡介最多500字，或連絡管理員協助調整");

        RuleFor(v => v.CoverImage)
            .NotNull()
            .WithMessage("CoverImage is required.")
            .MaximumLength(500)
            .WithMessage("Maximum length of CoverImage is 500 characters.")
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

    private static bool ContentHasImg(CreatePostCommand v) => v.Content != null && v.Content.Contains("<img src=");

    private async Task<bool> ImagesKeysExists(FileStorageInfo[]? fileStorageInfo, CancellationToken cancellationToken)
    {
        if (fileStorageInfo == null || fileStorageInfo.Length == 0)
        {
            return true;
        }
        
        foreach (var file in fileStorageInfo)
        {
            if (!await _fileStorageService.ExistsAsync(file.StorageKey, FileCategory.Post))
            {
                return false;
            }
        }
        return true;
    }
}
