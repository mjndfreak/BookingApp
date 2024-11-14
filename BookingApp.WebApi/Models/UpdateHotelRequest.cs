using System.ComponentModel.DataAnnotations;
using BookingApp.Data.Enums;

namespace ApiProject.Models;

public class UpdateHotelRequest
{
    [Required]
    public string Name { get; set; }
    public int Stars { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public AccomodationType AccomodationType { get; set; }
    public List<int> FeatureIds { get; set; }
}