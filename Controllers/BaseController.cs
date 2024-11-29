using DMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace DMS.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userJson = HttpContext.Session.GetString("UserInfo");
            if (userJson != null)
            {
                if (!string.IsNullOrEmpty(userJson))
                {
                    var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson);
                    ViewData["UserInfo"] = userInfo;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
