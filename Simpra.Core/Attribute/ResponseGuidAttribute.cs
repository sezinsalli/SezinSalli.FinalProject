using Microsoft.AspNetCore.Mvc.Filters;

namespace Simpra.Core.Attribute;

public class ResponseGuidAttribute : ActionFilterAttribute
{

    public ResponseGuidAttribute()
    {

    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {

    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        context.HttpContext.Response.Headers.Add("ResponseGuid", Guid.NewGuid().ToString());
        base.OnActionExecuted(context);
    }
}
