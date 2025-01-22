using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(8, MinimumLength =4)]
    public string Password { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? KnownAs { get; set; }
    public string? Gender { get; set; }
    public string? DateOfBirth { get; set; }

}
