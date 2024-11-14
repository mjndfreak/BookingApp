using BookingApp.Business.Operations.Hotel.Dtos;
using BookingApp.Business.Types;

namespace BookingApp.Business.Operations.Hotel;

public interface IHotelService
{
    Task<ServiceMessage> AddHotel(AddHotelDto hotelDto);
    Task<HotelDto> GetHotel(int id);
    Task<List<HotelDto>> GetAllHotels();
    Task<ServiceMessage> AdjustHotelStars(int id, int changeBy);
    Task<ServiceMessage> DeleteHotel(int id);
}