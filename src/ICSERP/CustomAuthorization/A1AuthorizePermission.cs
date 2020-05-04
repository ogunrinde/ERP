using System.Linq;
using System.Security.Claims;
using ICSERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.CustomAuthorization
{
    public class A1AuthorizePermission : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Permissions { get; set; } //Permission string to get from controller

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Validate if any permissions are passed when using attribute at controller or action level
            if (string.IsNullOrEmpty(Permissions))
            {
                //Validation cannot take place without any permissions so returning unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }
            var userName = context.HttpContext.User.Identity.Name;
            var requiredPermissions = Permissions.Split(","); //Multiple permissiosn can be received from controller, delimiter "," is used to get individual values
            
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserAuthService>();
            var userId = int.Parse(context.HttpContext.User.Identity.Name);
            var user = userService.GetById(userId, Permissions);
            if(user.Status == true)
            {
                context.HttpContext.Session.SetInt32("documentAccessLevel", user.DocumentAccessLevel);
                return;
            } 
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}