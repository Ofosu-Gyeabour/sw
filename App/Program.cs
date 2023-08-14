global using System.Diagnostics;
using App.utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

#region repository

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
ConfigObject.DB_SCAFFOLD = settings.scaffoldDbContext;
ConfigObject.MD5_AUTH = settings.MD5AuthenticationEndPoint;
ConfigObject.USERS_API = settings.getUsersEndPoint;
ConfigObject.DEPARTMENT_API = settings.getDepartmentsEndPoint;

#endregion

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=IDChallenge}/{id?}");

app.Run();
