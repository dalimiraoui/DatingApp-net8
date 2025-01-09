using System;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemeberDTO?> GetMemberAsync(string username)
        {
            return await context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemeberDTO>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemeberDTO>> GetMembersAsync()
        {
            return await context.Users 
                .ProjectTo<MemeberDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return  await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsername(string username)
    {
        return await context.Users
                    .Include(x => x.Photos)
                    .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
                .Include(x => x.Photos)
                .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
        
    }
}
