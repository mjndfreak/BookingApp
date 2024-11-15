using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required] 
    public required string Password  { get; set; }
    [Required] 
    public required string FirstName { get; set; }
    [Required] 
    public required string LastName { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    
}