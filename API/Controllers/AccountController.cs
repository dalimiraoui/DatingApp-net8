using System.ComponentModel.Design.Serialization;
using System.Threading;
using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) :BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> RegisterUser( RegisterDTO registerDto)
    {
        if (await IsUserNameExists(registerDto.UserName)) return BadRequest("UserName is taken");
        using var hmac = new HMACSHA512();
        var user = new AppUser{
            UserName = registerDto.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password.ToLower())),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return new UserDTO {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };

    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> loginUser( LoginDTO loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync( x => 
            x.UserName.ToLower() == loginDto.UserName.ToLower());
        if (user == null) return Unauthorized("UserName does not exist!");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password.ToLower()));

        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.PasswordHash[i])  return Unauthorized("Invalid Password User");
            
        }
        return new UserDTO {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };

    }

    private async Task<bool> IsUserNameExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

}
