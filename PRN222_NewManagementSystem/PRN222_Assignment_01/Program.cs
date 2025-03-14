using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;
using PRN222_Assignment_01.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("appsettings.json");

var connectionsString = builder.Configuration.GetConnectionString("FUNewsManagement");

builder.Services.AddDbContext<FUNewsManagementContext>(options =>
 options.UseSqlServer(connectionsString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login";  // Trang login
        options.LogoutPath = "/Authentication/Logout"; // Trang logout
        options.AccessDeniedPath = "/Authentication/AccessDenied"; // Trang bị từ chối
    });

builder.Services.AddAuthorization();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<RoleService>();
builder.Services.AddTransient<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<INewsArticalRepository, NewsArticalRepository>();
builder.Services.AddTransient<ITagRepository, TagRepository>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IReportRepository, ReportRepository>();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NewsArticle}/{action=Index}/{id?}");

app.Run();
