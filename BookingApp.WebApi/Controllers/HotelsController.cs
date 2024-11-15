using ApiProject.Models;
using BookingApp.Business.Operations.Hotel;
using BookingApp.Business.Operations.Hotel.Dtos;
using BookingApp.Business.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ApiProject.Controllers;


[Route("api/[controller]")] // Defining the route for the controller
[ApiController] // Indicating that this is an API controller
public class HotelsController : ControllerBase // Defining the HotelsController class which inherits from ControllerBase
{
    private readonly IHotelService _hotelService; // Declaring a private readonly field for the IHotelService

    public HotelsController(IHotelService hotelService) // Constructor for the HotelsController
    {
        _hotelService = hotelService; // Initializing the _hotelService field with the injected IHotelService
    }

    [HttpGet("{id}")] // Defining an HTTP GET endpoint with a route parameter 'id'
    public async Task<IActionResult> GetHotel(int id) // Asynchronous method to get a hotel by its ID
    {
        var hotel = await _hotelService.GetHotel(id); // Calling the GetHotel method of the IHotelService
        if (hotel == null) // Checking if the hotel is null
        {
            return NotFound(); // Returning a 404 Not Found response if the hotel is not found
        }

        return Ok(hotel); // Returning a 200 OK response with the hotel data
    }
    
    [HttpGet] // Defining an HTTP GET endpoint to get all hotels
    public async Task<IActionResult> GetAllHotels() // Asynchronous method to get all hotels
    {
        var hotels = await _hotelService.GetAllHotels(); // Calling the GetAllHotels method of the IHotelService
        return Ok(hotels); // Returning a 200 OK response with the list of hotels
    }

    [HttpPost] // Defining an HTTP POST endpoint to add a new hotel
    [Authorize(Roles = "Admin")] // Applying authorization to allow only Admin role
    public async Task<IActionResult> AddHotel(AddHotelRequest request) // Asynchronous method to add a new hotel
    {
        var addHotelDto = new AddHotelDto // Creating a new AddHotelDto object
        {
            Name = request.Name, // Setting the Name property
            Stars = request.Stars, // Setting the Stars property
            Address = request.Address, // Setting the Address property
            AccomodationType = request.AccomodationType, // Setting the AccomodationType property
            FeatureIds = request.FeatureIds // Setting the FeatureIds property
        };
        var result = await _hotelService.AddHotel(addHotelDto); // Calling the AddHotel method of the IHotelService
        
        if (!result.IsSuccess) // Checking if the result is not successful
        {
            return BadRequest(result.Message); // Returning a 400 Bad Request response with the error message
        }

        return Ok(); // Returning a 200 OK response if the hotel is added successfully
    }
    
    [HttpPatch("{id}/stars")] // Defining an HTTP PATCH endpoint to adjust the stars of a hotel
    public async Task<IActionResult> AdjustHotelStars(int id, int changeBy) // Asynchronous method to adjust the stars of a hotel
    {
        var result = await _hotelService.AdjustHotelStars(id, changeBy); // Calling the AdjustHotelStars method of the IHotelService
        if (!result.IsSuccess) // Checking if the result is not successful
        {
            return NotFound(); // Returning a 404 Not Found response if the hotel is not found
        }
        return Ok(); // Returning a 200 OK response if the stars are adjusted successfully
    }
    
    [HttpDelete("{id}")] // Defining an HTTP DELETE endpoint to delete a hotel
    public async Task<IActionResult> DeleteHotel(int id) // Asynchronous method to delete a hotel
    {
        var result = await _hotelService.DeleteHotel(id); // Calling the DeleteHotel method of the IHotelService
        if (!result.IsSuccess) // Checking if the result is not successful
        {
            return NotFound(); // Returning a 404 Not Found response if the hotel is not found
        }
        return Ok(); // Returning a 200 OK response if the hotel is deleted successfully
    }

    [HttpPut("{id}")] // Defining an HTTP PUT endpoint to update a hotel
    [Authorize(Roles = "Admin")] // Applying authorization to allow only Admin role
    public async Task<IActionResult>
        UpdateHotel(int id, UpdateHotelRequest request) // Asynchronous method to update a hotel
    {
        var updateHotelDto = new UpdateHotelDto // Creating a new UpdateHotelDto object
        {
            Id = id, // Setting the Id property
            Name = request.Name, // Setting the Name property
            Stars = request.Stars, // Setting the Stars property
            Address = request.Address, // Setting the Address property
            AccomodationType = request.AccomodationType, // Setting the AccomodationType property
            FeatureIds = request.FeatureIds // Setting the FeatureIds property
        };
        var result =
            await _hotelService.UpdateHotel(updateHotelDto); // Calling the UpdateHotel method of the IHotelService

        if (!result.IsSuccess) // Checking if the result is not successful
            return NotFound(result.Message); // Returning a 404 Not Found response with the error message
        return Ok();// Returning a 200 OK response if the hotel is updated successfully
    }
}
        