using BookingApp.Business.Operations.Setting;

namespace ApiProject.Middlewares;

public class MaintanenceMiddleware
{
    private readonly RequestDelegate _next;

    public MaintanenceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var settingService = context.RequestServices.GetService<ISettingService>(); 
        bool maintanenceMode = settingService.GetMaintanenceStatus();

        if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings"))
        {
            await _next(context);
            return;
        }

        if (maintanenceMode)
        {
            context.Response.StatusCode = 503;                              
            await context.Response.WriteAsync("The application is under maintenance.");
            return;
        }
        
        await _next(context); 
    }
}