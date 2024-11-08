using Microsoft.EntityFrameworkCore;
using SenicaWeb.Data;
using SenicaWeb.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigSqlContext(builder.Configuration);

builder.Services.AddConfigIdentity();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
