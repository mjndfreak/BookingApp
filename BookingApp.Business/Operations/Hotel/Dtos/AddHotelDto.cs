using System.ComponentModel.DataAnnotations;
using BookingApp.Data.Enums;

namespace BookingApp.Business.Operations.Hotel.Dtos;

public class AddHotelDto
{
    public required string Name { get; set; }
    public int Stars { get; set; }
    public required string Location { get; set; }
    public AccomodationType AccomodationType { get; set; }
    public required List<int> FeatureIds { get; set; }
}