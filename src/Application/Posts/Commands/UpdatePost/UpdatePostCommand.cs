using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Domain;
using GerPros_Backend_API.Domain.Entities;
using GerPros_Backend_API.Domain.Enums;

namespace GerPros_Backend_API.Application.Posts.Commands.UpdatePost;

public record UpdatePostCommand : IRequest
{
    public Guid Id { get; init; }
    public required string Title { get; init; }

    public string? Description { get; init; }
    public string? Content { get; init; }
    public string? CoverImage { get; init; }

    public string[]? Tags { get; init; }

    /// <summary>
    /// 當呼叫GET Post API時，會回傳此文章相關的image key / name
    /// 如果Update 進來之後，有新增或刪除的話，會透過此欄位來更新Storage
    /// </summary>
    public FileStorageInfo[]? FileStorageInfo { get; set; }
}

public class UpdatePostCommandHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
    : IRequestHandler<UpdatePostCommand>
{
    public async Task Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Posts
            .FindAsync([request.Id], cancellationToken);
        Guard.Against.NotFound(request.Id.ToString(), entity);

        await DeleteFilesIfNotExisted(request, cancellationToken, entity);

        if (request.Tags != null)
            await RewritePostTags(request, cancellationToken);

        var unusedTags = await context.Tags
            .Where(t => t.PostTags != null && t.PostTags.Count == 0)
            .ToListAsync(cancellationToken);

        context.Tags.RemoveRange(unusedTags);
        
        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Content = request.Content;
        entity.CoverImage = request.CoverImage;
        entity.FileStorageInfo = request.FileStorageInfo;


        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task RewritePostTags(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var existPostTags = await context.PostTags
            .Where(pt => pt.PostId == request.Id)
            .ToListAsync(cancellationToken);
        context.PostTags.RemoveRange(existPostTags);

        foreach (var tag in request.Tags!)
        {
            var tagEntity = await context.Tags
                .Where(t => t.Name == tag)
                .FirstOrDefaultAsync(cancellationToken);

            if (tagEntity == null)
            {
                tagEntity = new Tag { Id = Guid.NewGuid(), Name = tag };
                context.Tags.Add(tagEntity);
            }

            context.PostTags.Add(new PostTag { PostId = request.Id, TagId = tagEntity.Id });
        }
    }

    /// <summary>
    /// 比較新舊的FileStorageInfo，刪除更新文章後，不再使用的檔案
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="entity"></param>
    private async Task DeleteFilesIfNotExisted(UpdatePostCommand request, CancellationToken cancellationToken,
        Post entity)
    {
        var newFileStorageInfo = request.FileStorageInfo ?? [];
        var oldFileStorageInfo = entity.FileStorageInfo ?? [];
        var newFileStorageInfoKeys = newFileStorageInfo.Select(x => x.StorageKey).ToHashSet();
        var removedFileStorageInfo =
            oldFileStorageInfo.Where(x => !newFileStorageInfoKeys.Contains(x.StorageKey)).ToArray();
        if (removedFileStorageInfo.Length != 0)
            await fileStorageService.DeleteAllAsync(removedFileStorageInfo.Select(x => x.StorageKey).ToArray(),
                FileCategory.Post, cancellationToken);
    }
}
