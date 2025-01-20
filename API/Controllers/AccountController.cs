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
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers;

public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) :BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> RegisterUser(RegisterDTO registerDto)
    {
        if (await IsUserNameExists(registerDto.UserName)) return BadRequest("UserName is taken");

        //using var hmac = new HMACSHA512();

        var user = mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.UserName.ToLower();
        // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password.ToLower()));
        // user.PasswordSalt = hmac.Key;
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);
        return new UserDTO {
            Username = user.UserName,
            Gender = user.Gender,
            Token = await tokenService.CreateToken(user),
            KnownAs = user.KnownAs
        };

    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> loginUser(LoginDTO loginDto)
    {
        var user = await userManager.Users
           .Include(u => u.Photos)
           .FirstOrDefaultAsync( x => 
             x.NormalizedUserName == loginDto.UserName.ToUpper()
            );
        if (user == null || user.UserName ==null ) return Unauthorized("UserName does not exist!");
        // using var hmac = new HMACSHA512(user.PasswordSalt);
        // var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password.ToLower()));

        // for (int i = 0; i < computeHash.Length; i++)
        // {
        //     if (computeHash[i] != user.PasswordHash[i])  return Unauthorized("Invalid Password User");
            
        // }
        return new UserDTO {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            Token = await tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
        };

    }

    private async Task<bool> IsUserNameExists(string username)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
    }

}
