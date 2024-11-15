using BookingApp.Business.Operations.Setting; // Importing the Setting operations from the BookingApp.Business

namespace ApiProject.Middlewares; // Declaring the namespace for the middleware

public class MaintanenceMiddleware // Defining the MaintanenceMiddleware class
{
    private readonly RequestDelegate _next; // Declaring a private readonly field for the next request delegate

    public MaintanenceMiddleware(RequestDelegate next) // Constructor for the MaintanenceMiddleware
    {
        _next = next; // Initializing the _next field with the injected RequestDelegate
    }

    public async Task Invoke(HttpContext context) // Asynchronous method to handle the HTTP context
    {
        var settingService = context.RequestServices.GetService<ISettingService>(); // Getting the ISettingService from the request services
        bool maintanenceMode = settingService.GetMaintanenceStatus(); // Getting the maintenance status from the ISettingService

        if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings")) // Checking if the request path starts with /api/auth/login or /api/settings
        {
            await _next(context); // Calling the next middleware in the pipeline
            return; // Returning from the method
        }

        if (maintanenceMode) // Checking if the maintenance mode is enabled
        {
            context.Response.StatusCode = 503; // Setting the response status code to 503 Service Unavailable
            await context.Response.WriteAsync("The application is under maintenance."); // Writing the maintenance message to the response
            return; // Returning from the method
        }

        await _next(context); // Calling the next middleware in the pipeline
    }
}