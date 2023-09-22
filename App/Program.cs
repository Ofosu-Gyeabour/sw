global using System.Diagnostics;
global using Microsoft.EntityFrameworkCore;
global using App.Models;

using App.utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region repository

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
var eventsConfiguration = builder.Configuration.GetSection("Events").Get<Events>();

ConfigObject.API_STUB = settings.apiStub;
ConfigObject.DB_SCAFFOLD = settings.scaffoldDbContext;
ConfigObject.DB_CONN = settings.dbConnectionString;
ConfigObject.MD5_AUTH = settings.MD5AuthenticationEndPoint;
ConfigObject.USERS_API = settings.getUsersEndPoint;
ConfigObject.DEPARTMENT_API = settings.getDepartmentsEndPoint;
ConfigObject.MD5_ENC = settings.GetMD5Encryption;
ConfigObject.LOGIN_FLAG = settings.SetLoggedInFlag;
ConfigObject.LOGOUT_FLAG = settings.SetLoggedOutFlag;
ConfigObject.CONTENT_TYPE = settings.contentType;
ConfigObject.WRITE_LOG = settings.WriteLog;

ConfigObject.PROFILE_CREATE = settings.CreateProfile;
ConfigObject.PROFILES_GET = settings.GetProfiles;

ConfigObject.USER_PROFILE_GET = settings.GetUserProfile;
ConfigObject.USER_PROFILE_MODULES = settings.GetProfileModules;
ConfigObject.USER_PROFILE_AMEND = settings.AmendUserProfile;
ConfigObject.USER_CREATE = settings.CreateUser;

ConfigObject.AUTH_OPERATION = eventsConfiguration.auth;
ConfigObject.EXIT_OPERATION = eventsConfiguration.exit;

ConfigObject.PWD_CHANGE = settings.changePassword;

#endregion

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

//app.UseHttpsRedirection();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=AltLogin}/{id?}");

app.Run();
