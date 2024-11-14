using ApiProject.Filters;
using BookingApp.Business.Operations.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ISettingService _settingService;
    
    public SettingsController(ISettingService settingService)
    {
        _settingService = settingService;
    }
    
    [HttpPatch]
    [Authorize(Roles = "Admin")]
    [TimeControlFilter]
    public async Task<IActionResult> ToggleMaintenanceMode()
    {
        await _settingService.ToggleMaintenanceMode();
        return Ok();
    }
}