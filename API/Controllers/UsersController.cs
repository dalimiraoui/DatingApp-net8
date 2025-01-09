using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemeberDTO>>> getAllUsers()
    {
        var users= await userRepository.GetMembersAsync();
        return Ok(users);
    }
    
    [Authorize]
    [HttpGet("{username}")]
    public async Task<ActionResult<MemeberDTO>> getUserById(string  username)
    {
        var user= await userRepository.GetMemberAsync(username);
        if (user == null) return  NotFound("User with username :"+username+" not found");
        return Ok(user);
    }
}
