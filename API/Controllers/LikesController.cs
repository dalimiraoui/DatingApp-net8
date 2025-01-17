using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class LikesController(ILikesRepository likesRepository) : BaseApiController
{

    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId)
    {
        var sourceUserId = User.GetUserId();

        if (sourceUserId == targetUserId) return BadRequest("You can not like yourself");

        var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserId);
        if (existingLike == null) {
            var userLike = new UserLike {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId
            };
            likesRepository.AddLike(userLike);
        } else {
            likesRepository.DeleteLike(existingLike);
        }

        if (await likesRepository.SaveChangesAsync()) return Ok();

        return BadRequest("Problem Toggling like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
    {
        return Ok( await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<int>>> GetUserLikes([FromQuery]LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await likesRepository.GetUserLikes(likesParams);
        Response.AddPaginationHeader(users);
        return Ok(users);
    }
}
