using BookingApp.Business.Operations.Setting;

namespace ApiProject.Middlewares;

public class MaintanenceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISettingService _settingService;

    public MaintanenceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var settingService = context.RequestServices.GetService<ISettingService>(); 
        bool maintanenceMode = _settingService.GetMaintanenceStatus();

        if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings"))
        {
            await _next(context);
            return;
        }

        if (maintanenceMode)
        {
            await context.Response.WriteAsync("The application is under maintenance.");
        }
        await _next(context); 
    }
}