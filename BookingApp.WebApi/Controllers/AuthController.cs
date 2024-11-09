using ApiProject.Jwt;
using ApiProject.Models;
using BookingApp.Business.Operations.User;
using BookingApp.Business.Operations.User.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var addUserDto = new AddUserDto
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate
        };
        var result =await _userService.AddUserAsync(addUserDto);
        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            return BadRequest(result.Message);
        }
    }
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
     {
         if (!ModelState.IsValid)
         {
             return BadRequest(ModelState);
         }

         var result = _userService.LoginUser(new LoginUserDto{Email = request.Email, Password = request.Password});

         if (!result.IsSuccess)
             return BadRequest(result.Message);
         
         var user = result.Data;

         var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
         var token = JwtHelper.GenerateJwtToken(new JwtDto
         {
             Id = user.Id,
             Email = user.Email,
             FirstName = user.FirstName,
             LastName = user.LastName,
             UserType = user.UserType,
             SecretKey = configuration["Jwt:SecretKey"]!,
             Issuer = configuration["Jwt:Issuer"]!,
             Audience = configuration["Jwt:Audience"]!,
             ExpirationMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
         });
         return Ok(new LoginResponse
         {
             Message = "Login successful",
             Token = token
         });
     }
     [HttpGet("me")]
     [Authorize] // This attribute will check if the request has a valid token

     public IActionResult GetMyUser()
     {
         return Ok();
     }
}




