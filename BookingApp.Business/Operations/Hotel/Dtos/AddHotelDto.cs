using System.ComponentModel.DataAnnotations;
using BookingApp.Data.Enums;

namespace BookingApp.Business.Operations.Hotel.Dtos;

public class AddHotelDto
{
    public string Name { get; set; }
    public int Stars { get; set; }
    public string Address { get; set; }
    public AccomodationType AccomodationType { get; set; }
    public List<int> FeatureIds { get; set; }
}