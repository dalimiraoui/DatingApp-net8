using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(DataContext context) : ControllerBase
{
    private DataContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> getAllUsers()
    {
        var users= await _context.Users.ToListAsync();
        return users;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> getUserById(int id)
    {
        var user= await _context.Users.FindAsync(id);
        if (user == null) return  NotFound("User with id :"+id+" not found");
        return user;
    }
}
