using System.ComponentModel.DataAnnotations;
using BookingApp.Data.Enums;

namespace ApiProject.Models;

public class UpdateHotelRequest
{
    [Required]
    public required string Name { get; set; }
    public int Stars { get; set; }
    [Required]
    public required string Location { get; set; }
    [Required]
    public AccomodationType AccomodationType { get; set; }
    public required List<int> FeatureIds { get; set; }
}