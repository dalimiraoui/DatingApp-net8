using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    public required string UserName { get; set; }
    
    [Required]
    public required string Password { get; set; }

}
