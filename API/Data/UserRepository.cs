using System;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.Helpers;

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

        public async Task<PagedList<MemeberDTO?>> GetMembersAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable(); // Queryable it is essential to work with where clause
            query = query.Where( u => u.UserName != userParams.CurrentUsername);

            if (userParams.Gender != null) {
                query = query.Where( u => u.Gender == userParams.Gender);
            }

            var minDob =  DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1 ));
            var maxDob =  DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where( x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch {
                "created" => query.OrderByDescending( x => x.Created),
                _ => query.OrderByDescending( x => x.LastActive),
            };

            return await PagedList<MemeberDTO?>.CreateAsync(query.ProjectTo<MemeberDTO>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
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
