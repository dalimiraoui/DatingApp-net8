using API.Entities;
using API.DTOs;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsername(string username);
    Task<PagedList<MemeberDTO?>> GetMembersAsync(UserParams userParams);
    Task<MemeberDTO?> GetMemberAsync(string username);

}
