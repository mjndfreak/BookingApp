using ApiProject.Models;
using BookingApp.Business.Operations.Feature;
using BookingApp.Business.Operations.Feature.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers;
[Route("api/[controller]")]
[ApiController]

public class FeaturesController : ControllerBase
{
    private readonly IFeatureService _featureService;

    public FeaturesController(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddFeature(AddFeatureRequest request)
    {
        var addFeatureDto = new AddFeatureDto
        {
            Title = request.Title
        };
        var result = await _featureService.AddFeature(addFeatureDto);
        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Message);
    }
}