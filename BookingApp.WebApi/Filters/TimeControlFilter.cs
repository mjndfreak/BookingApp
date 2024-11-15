using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiProject.Filters;

public class TimeControlFilter : ActionFilterAttribute
{
    public required string StartTime { get; set; }
    public required string EndTime { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var now = DateTime.UtcNow.TimeOfDay;
        
        StartTime = "23:00:00";
        EndTime = "23:59:59";

        if (now >= TimeSpan.Parse(StartTime) && now <= TimeSpan.Parse(EndTime))
        {
            base.OnActionExecuting(context);
        }

        context.Result = new ContentResult
        {
            Content = "The service is not available at this time.",
            StatusCode = 403
        };

    }
}