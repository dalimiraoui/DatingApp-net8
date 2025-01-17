using System;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.Helpers;

namespace API.Data;

public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
{
    public void AddLike(UserLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(UserLike like)
    {
        context.Likes.Remove(like);
    }

    public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
    {
        return await context.Likes
                    .Where(x => x.SourceUserId == currentUserId)
                    .Select(x => x.TargetUserId)
                    .ToListAsync();
    }

    public async Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId)
    {
        return await context.Likes.FindAsync(sourceUserId, targetUserId);
    }

    public async Task<PagedList<MemeberDTO>> GetUserLikes(LikesParams likesParams)
    {
        var likes = context.Likes.AsQueryable();

        IQueryable query;
        switch(likesParams.Predicate) 
        {
            case "liked":
                query= likes
                    .Where(x => x.SourceUserId ==likesParams.UserId)
                    .Select(x => x.TargetUser);
                    break;
            case "likedBy":
                query= likes
                    .Where(x => x.TargetUserId ==likesParams.UserId)
                    .Select(x => x.SourceUser);
                break;
            default :
                var LikeIds = await GetCurrentUserLikeIds(likesParams.UserId);
                query= likes
                    .Where(x => x.TargetUserId == likesParams.UserId && LikeIds.Contains(x.SourceUserId))
                    .Select(x => x.SourceUser);
                break;
        }
        return await PagedList<MemeberDTO>.CreateAsync(
            query.ProjectTo<MemeberDTO>(mapper.ConfigurationProvider),
            likesParams.PageNumber,
            likesParams.PageSize
        );
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() >0 ;
    }
}
