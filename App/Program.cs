global using System.Diagnostics;
global using Microsoft.EntityFrameworkCore;
global using App.Models;

using App.utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region repository

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
ConfigObject.DB_SCAFFOLD = settings.scaffoldDbContext;
ConfigObject.MD5_AUTH = settings.MD5AuthenticationEndPoint;
ConfigObject.USERS_API = settings.getUsersEndPoint;
ConfigObject.DEPARTMENT_API = settings.getDepartmentsEndPoint;
ConfigObject.MD5_ENC = settings.GetMD5Encryption;
ConfigObject.CONTENT_TYPE = settings.contentType;

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
