namespace ApiProject.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMaintanenceMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<MaintanenceMiddleware>();
    }
}