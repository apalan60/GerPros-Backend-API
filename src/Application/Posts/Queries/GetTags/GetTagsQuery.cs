﻿using GerPros_Backend_API.Application.Common.Interfaces;

namespace GerPros_Backend_API.Application.Posts.Queries.GetTags;

public record GetTagsQuery : IRequest<string[]>;

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, string[]>
{
    private readonly IApplicationDbContext _context;

    public GetTagsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string[]> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags
            .Where(t => t.PostTags != null && t.PostTags.Count > 0)
            .Select(t => t.Name)
            .Distinct()
            .ToArrayAsync(cancellationToken);

        return tags;
    }
}
