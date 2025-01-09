using System;
using API.Entities;
using API.DTOs;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);

    Task<bool> SaveAllAsync();

    Task<IEnumerable<AppUser>> GetUsersAsync();

    Task<AppUser?> GetUserByIdAsync(int id);

    Task<AppUser?> GetUserByUsername( string username);
    Task<IEnumerable<MemeberDTO?>> GetMembersAsync();
    Task<MemeberDTO?> GetMemberAsync(string username);

}
