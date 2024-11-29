using DMS.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Helpers
{
    public class RoleAccessFilterAttribute : ActionFilterAttribute
    {
        private readonly string _requiredConfig;

        public RoleAccessFilterAttribute(string requiredConfig)
        {
            _requiredConfig = requiredConfig;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (_requiredConfig != "DefaultConfig")
            {

                // Retrieve user role from session or claims
                var userJson = context.HttpContext.Session.GetString("UserInfo");
                if (string.IsNullOrEmpty(userJson))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var userInfo = System.Text.Json.JsonSerializer.Deserialize<UserInfo>(userJson);

                // Access role-config mapping (e.g., from database or in-memory list)
                var roleConfigMap = new[] // Example in-memory mapping
                {
                    new { Role = "SAdmin", AllowedConfigs = new[] { "ConfigA", "ConfigB", "ConfigC" } },
                    new { Role = "Admin", AllowedConfigs = new[] { "ConfigA", "ConfigB" } },
                    new { Role = "User", AllowedConfigs = new[] { "ConfigA" } }
                };

                var userRole = userInfo.Role;
                var hasAccess = roleConfigMap
                    .Where(r => r.Role == userRole)
                    .SelectMany(r => r.AllowedConfigs)
                    .Contains(_requiredConfig);

                if (!hasAccess)
                {
                    // Return Forbidden if the user lacks access
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
