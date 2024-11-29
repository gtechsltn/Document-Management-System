using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authentication;
using DMS.Models;
using System.Text.Json;

namespace DMS.Controllers;

public class AccountController : BaseController
{
    private readonly IStringLocalizer<AccountController> _localizer;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IStringLocalizer<AccountController> localizer, ILogger<AccountController> logger)
    {
        _localizer = localizer;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index", "Account");
        }

        // Dummy authentication (replace with real logic)
        if (model.Username == "admin" && model.Password == "admin")
        {
            // Save user info in session
            var user = new UserInfo();
            user.Username = model.Username;
            user.Role = "Admin";
            user.Department = Enum.GetName(typeof(Department), 0); ;
            user.Division = Enum.GetName(typeof(Faculty), 0);

            HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(user));

            // Redirect to another page after successful login
            return RedirectToAction("Index", "Home");
        }
        else if(model.Username == "admin" && model.Password == "123")
        {
            // Save user info in session
            var user = new UserInfo();
            user.Username = model.Username;
            user.Role = "SAdmin";
            user.Department = Enum.GetName(typeof(Department), 0); ;
            user.Division = Enum.GetName(typeof(Faculty), 0);

            HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(user));

            // Redirect to another page after successful login
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            
            return RedirectToAction("Index", "Account");
        }     


    }


    [HttpGet]
    //[ValidateAntiForgeryToken] // Protect against CSRF attacks
    public IActionResult Logout()
    {
        // Sign the user out
        HttpContext.SignOutAsync("CookieAuth");

        HttpContext.Session.Clear(); // Clear all session data
        // Redirect to the login page or home page
        return RedirectToAction("Index", "Account");

    }

    [HttpGet]
    //[ValidateAntiForgeryToken] // Protect against CSRF attacks
    public IActionResult ChangePass()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AccessDenied(string ReturnUrl)
    {
        var qString = ReturnUrl;
        ViewData["error"] = qString;

        return View();
    }

}
