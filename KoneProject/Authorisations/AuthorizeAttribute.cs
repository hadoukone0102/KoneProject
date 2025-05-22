
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace KoneProject.Authorisations;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string? Roles { get; set; } = "";
        public AuthorizeAttribute() { }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;


            string user = (string?)context.HttpContext.Items["userId"] ?? "";
            string email = (string?)context.HttpContext.Items["userEmail"] ?? "";

            List<string> listUserRoles = (List<string>)context.HttpContext.Items["roles"];
            Boolean notFindRequiredRole = true;
            String[] listRequiredRole = this.Roles.Split(',');

            if (listRequiredRole.Length == 1 && listRequiredRole[0] == "")
            {
                notFindRequiredRole = false;
            }
            else
            {
                foreach (string requiredRole in listRequiredRole)
                {
                    if (listUserRoles == null)
                    {
                        break;
                    }
                    if (listUserRoles.Contains(requiredRole))
                    {
                        notFindRequiredRole = false;
                        break;
                    }
                }
            }



            if (user == "" || notFindRequiredRole || listUserRoles == null || listUserRoles.Count == 0)
            {
                // dont have authorization
                context.Result = new JsonResult(new { message = "Non authorisé" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }


        }
    }


