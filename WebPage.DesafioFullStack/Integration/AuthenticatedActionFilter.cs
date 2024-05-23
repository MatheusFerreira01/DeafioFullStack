using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebPage.DesafioFullStack.Integration;
public class AuthenticatedActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();

        if (controller != "Authentication" || action != "Login")
        {
            if (context.HttpContext.Session.GetString("AuthenticationToken") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
            }
        }
    }    

    public void OnActionExecuted(ActionExecutedContext context)
    {   
    }
}
