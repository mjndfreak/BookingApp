using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models;

public class AddFeatureRequest
{
    [Required]
    [Length(5,100)]
    public string Title { get; set; }
}