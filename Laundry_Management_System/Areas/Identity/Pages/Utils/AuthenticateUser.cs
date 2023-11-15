using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication13.Filters
{
    public class AuthenticateUser : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string tempSession = context.HttpContext.Session.GetString("AuthenticationToken");
            string tempAuthCookie = context.HttpContext.Request.Cookies["AuthenticationToken"];

            if (string.IsNullOrEmpty(tempSession) || string.IsNullOrEmpty(tempAuthCookie) || tempSession != tempAuthCookie)
            {
                RedirectResult result = new RedirectResult("~/Identity/Account/Login");
                context.Result = result;
            }
        }
    }
}
