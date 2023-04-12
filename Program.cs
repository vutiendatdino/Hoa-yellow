using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Services.Common_Services;
using SP23_G21_SHSMS.Models.Campuses;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfigurationRoot>(configuration);
builder.Services.AddScoped<ConfigurationService>();
builder.Services.AddScoped<DbShsmsContext>();
builder.Services.AddScoped<DbCampusContext>();
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//    options.JsonSerializerOptions.MaxDepth = 0;
//});
builder.Services.AddControllers().AddNewtonsoftJson(x =>
{
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    x.SerializerSettings.MaxDepth = 2;
});
//builder.Services.AddControllersWithViews().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//    options.JsonSerializerOptions.MaxDepth = 0;
//});
builder.Services.AddControllersWithViews().AddNewtonsoftJson(x =>
{
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    x.SerializerSettings.MaxDepth = 2;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}
    ).AddCookie(
    //options =>
    //         {
    //             options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    //             options.SlidingExpiration = true;
    //             options.AccessDeniedPath = "/Forbidden/";
    //             options.Cookie.IsEssential = true;
    //             options.Cookie.SameSite = SameSiteMode.Strict;
    //             options.Cookie.HttpOnly = true;
    //             options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    //         }
    ).AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = configuration["Authentication:Google:ClientId"];
        options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
