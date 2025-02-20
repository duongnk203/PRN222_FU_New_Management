using Microsoft.EntityFrameworkCore;
using PRN222_Assignment_01.Models;
using PRN222_Assignment_01.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("appsettings.json");

var connectionsString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FunewsManagementContext>(options =>
 options.UseSqlServer(connectionsString));

builder.Services.AddTransient<ISystemAccountRepository, SystemAccountRepository>();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();
