using System;
using API.Entities;
using API.DTOs;
using API.Helpers;

namespace API.Interfaces;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);

    Task<PagedList<MemeberDTO>> GetUserLikes(LikesParams likes);
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
    void DeleteLike(UserLike like);
    void AddLike(UserLike like);

    Task<bool> SaveChangesAsync();

}
