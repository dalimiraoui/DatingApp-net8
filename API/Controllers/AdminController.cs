using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AdminController() : BaseApiController
{
    
    [Authorize(Policy ="RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public  ActionResult GetUsersWithRoles()
    {
        return Ok("Only admins can see this");
    }
    
    [Authorize(Policy ="ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public  ActionResult GetPhotosForModeration()
    {
        return Ok("Admin or moderators can see this");
    }
    
}
