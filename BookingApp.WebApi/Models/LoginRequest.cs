using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}