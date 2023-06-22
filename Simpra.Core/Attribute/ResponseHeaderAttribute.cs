using Microsoft.AspNetCore.Mvc.Filters;

namespace Simpra.Core.Attribute;

public class ResponseHeaderAttribute : ActionFilterAttribute
{
    private readonly string _name;
    private readonly string _value;

    public ResponseHeaderAttribute(string name, string value)
    {
        _name = name;
        _value = value;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
  
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        context.HttpContext.Response.Headers.Add(_name, _value);
        base.OnActionExecuted(context);
    }
}
