using BookingApp.Data.Enums;

namespace BookingApp.Business.Operations.Hotel.Dtos;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Stars { get; set; }
    public string Location { get; set; }
    public AccomodationType AccomodationType { get; set; }
    public List<HotelFeatureDto> Features { get; set; }
}