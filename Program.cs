using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using DMS.Data;
using DMS.Helpers;

//CultureInfo.DefaultThreadCurrentCulture
//= CultureInfo.DefaultThreadCurrentUICulture
//= PersianDateExtensionMethods.GetPersianCulture();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DemoDMSContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DemoDMSContext") ?? throw new InvalidOperationException("Connection string 'DemoDMScnt' not found.")));

builder.Services.AddPortableObjectLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options => options
        .AddSupportedCultures("en")
        .AddSupportedUICultures("en"));

builder.Services.AddMvc().AddViewLocalization();

builder.Services.AddDbContext<DemoDMSContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("DemoDMSContext")
        ?? throw new InvalidOperationException("Connection string 'DemoDMSContext' not found.")));

// Add services to the container.
builder.Services.AddHttpContextAccessor();

//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new RoleAccessFilterAttribute("DefaultConfig"));
});

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout
    options.Cookie.HttpOnly = true; // Protect session cookie
    options.Cookie.IsEssential = true; // Required for GDPR compliance
});

// Add distributed memory cache (required for session)
builder.Services.AddDistributedMemoryCache();


builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Index";
        options.LogoutPath = "/Account/Logout";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Use session middleware
app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.UseRequestLocalization();

app.MapRazorPages();

app.Run();
