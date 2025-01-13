using System.Security.Claims;
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
    
    [HttpGet("{username}")]
    public async Task<ActionResult<MemeberDTO>> getUserById(string  username)
    {
        var user= await userRepository.GetMemberAsync(username);
        if (user == null) return  NotFound("User with username :"+username+" not found");
        return Ok(user);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO  memberUpdateDTO)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(username == null) return BadRequest("No username found in token");
        var user = await userRepository.GetUserByUsername(username);

        if(user ==null) return BadRequest("Could not found user");
        
        mapper.Map(memberUpdateDTO, user);
        
        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }
}
