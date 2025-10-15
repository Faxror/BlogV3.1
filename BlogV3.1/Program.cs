using BlogV3._1.Models;
using BusinesssLayer.Abstrack;
using BusinesssLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayerr.Abstrack;
using DataAccessLayerr.Concrete;
using EntityLayerr.Concrate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<DBContext>()
    .AddDefaultTokenProviders().AddErrorDescriber<AuthorIdentityValidator>();
builder.Services.AddDbContext<DBContext>();
builder.Services.AddScoped<IBlogServices, BlogManager>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.LoginPath = "/Account/Login"; 
    });


var app = builder.Build();
    
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Account/StatusCode", "?code={0}");


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
