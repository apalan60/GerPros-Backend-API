using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.Files.GetUrl;
using GerPros_Backend_API.Application.Files.UploadFile;
using GerPros_Backend_API.Application.Posts.Commands.CreatePost;
using GerPros_Backend_API.Application.Posts.Commands.DeletePost;
using GerPros_Backend_API.Application.Posts.Commands.UpdatePost;
using GerPros_Backend_API.Application.Posts.Queries.GetPost;
using GerPros_Backend_API.Application.Posts.Queries.GetPostsWithPagination;
using GerPros_Backend_API.Application.Posts.Queries.GetTags;
using GerPros_Backend_API.Domain;
using GerPros_Backend_API.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GerPros_Backend_API.Web.Endpoints;

public class Posts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreatePost)
            .MapPut(UpdatePost, "{id}")
            .MapDelete(DeletePost, "{id}");

        app.MapGroup(this)
            .AllowAnonymous()
            .MapGet(GetPostDetail, "{id}")
            .MapGet(GetPostItemWithPagination);

        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(UploadFile, "/image-upload");

        app.MapGroup(this)
            .AllowAnonymous()
            .MapGet(GetTagLists, "/tags");
    }

    public async Task<UploadPostFileResponse> UploadFile(
        ISender sender,
        [FromForm] IFormFile file)
    {
        UploadedFile? uploadedFile = null;
        if (file is
            {
                Length: > 0
            })
        {
            uploadedFile = new UploadedFile(
                file.OpenReadStream(),
                file.FileName,
                file.ContentType
            );
        }

        var fileStorageInfo = await sender.Send(new UploadFileCommand
        {
            File = uploadedFile ?? throw new InvalidOperationException(), Category = FileCategory.Post
        });
        
        //warning: ExpiresOn is set to DateTime.MaxValue due to the content of post need to be permanent 
        var url = await sender.Send(new GetUrlQuery(fileStorageInfo.StorageKey, FileCategory.Post, DateTime.MaxValue, true));
        
        return new UploadPostFileResponse
        {
            FileStorageInfo = fileStorageInfo,
            Url = url
        };
    }

    public class UploadPostFileResponse
    {
        public required FileStorageInfo FileStorageInfo{ get; set; }

        public required string Url { get; set; }
    }

    public Task<string[]> GetTagLists(ISender sender) => sender.Send(new GetTagsQuery());

    public Task<PostDto> GetPostDetail(ISender sender, Guid id) => sender.Send(new GetPostDetailQuery(id));

    public Task<PaginatedList<PostItemDto>> GetPostItemWithPagination(ISender sender,
        [AsParameters] GetPostWithPaginationQuery query) =>
        sender.Send(query);

    public async Task<Guid> CreatePost(
        ISender sender,
        CreatePostCommand command) =>
        await sender.Send(command);


    public async Task<IResult> UpdatePost(ISender sender, UpdatePostCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeletePost(ISender sender, Guid id)
    {
        await sender.Send(new DeletePostCommand(id));
        return Results.NoContent();
    }
}
