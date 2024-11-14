using BookingApp.Business.Operations.Setting;
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
    public async Task<IActionResult> ToggleMaintenanceMode()
    {
        await _settingService.ToggleMaintenanceMode();
        return Ok();
    }
}