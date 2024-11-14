using ApiProject.Models;
using BookingApp.Business.Operations.Hotel;
using BookingApp.Business.Operations.Hotel.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ApiProject.Controllers;


[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotel(int id)
    {
        var hotel = await _hotelService.GetHotel(id);
        if (hotel == null)
        {
            return NotFound();
        }

        return Ok(hotel);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllHotels()
    {
        var hotels = await _hotelService.GetAllHotels();
        return Ok(hotels);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddHotel(AddHotelRequest request)
    {
        var addHotelDto = new AddHotelDto
        {
            Name = request.Name,
            Stars = request.Stars,
            Location = request.Location,
            AccomodationType = request.AccomodationType,
            FeatureIds = request.FeatureIds
        };
        var result = await _hotelService.AddHotel(addHotelDto);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }
}