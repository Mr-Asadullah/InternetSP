using InternetSP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternetSP
{
    public class Buyer : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string? accessToken = context.HttpContext.Request.Cookies["user-access-token"];
            var db = context.HttpContext.RequestServices.GetRequiredService<InternetSPContext>();
            if (!string.IsNullOrEmpty(accessToken))
            {
                if (!db.Users.Where(x => x.AccessToken.Equals(accessToken) && x.Role.Name.Equals("Buyer")).Any())
                    context.Result = new RedirectResult("/Account/Login");
            }
            else
                context.Result = new RedirectResult("/Account/Login");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
