using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(
    IUserRepository userRepository,
    IMapper mapper,
    IPhotoService photoService
) : BaseApiController
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

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        var username = User.GetUsername();

        var user = await userRepository.GetUserByUsername(username);
        if (user == null) return BadRequest("Could not found user");

        var result = await photoService.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);
        var photo = new Photo {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,

        };

        if (user.Photos.Count == 0) photo.IsMain =true;
        user.Photos.Add(photo);

        if (await userRepository.SaveAllAsync()) return CreatedAtAction(
            nameof(getAllUsers),
            new { username = user.UserName}, mapper.Map<PhotoDTO>(photo)
        );

        return BadRequest("Problem adding photo");

    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult<PhotoDTO>> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsername(User.GetUsername());
        if (user == null) return BadRequest("Could not found user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo.IsMain) return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to set main photo");

    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsername(User.GetUsername());
        if (user == null) return BadRequest("Could not found user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain ) return BadRequest("This photo can not be deleted");
        
        var publicId = photo.PublicId;
        if (publicId !=null) {
            var result = await photoService.DeletePhotoAsync(publicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);

        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting Photo from Db");

    }
}
