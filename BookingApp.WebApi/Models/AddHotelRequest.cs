using System.ComponentModel.DataAnnotations;
using BookingApp.Data.Enums;

namespace ApiProject.Models;

public class AddHotelRequest
{
    [Required] public string Name { get; set; }
    public int Stars { get; set; }
    [Required] public string Address { get; set; }
    public AccomodationType AccomodationType { get; set; }
    public List<int> FeatureIds { get; set; }
}