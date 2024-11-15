using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models;

public class AddFeatureRequest
{
    [Required]
    [Length(5,30)]
    public required string Title { get; set; }
}