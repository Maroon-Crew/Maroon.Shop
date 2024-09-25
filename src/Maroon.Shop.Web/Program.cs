using Maroon.Shop.Api.Client;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient<ProductClient>(httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:5113/api/");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddDbContext<ShopContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

// Add Kiota handlers to the dependency injection container
builder.Services.AddKiotaHandlers();

// Register the factory for the GitHub client
builder.Services.AddHttpClient<MaroonClientFactory>((sp, client) => {
    // Set the base address and accept header
    // or other settings on the http client
    client.BaseAddress = new Uri("https://localhost:7282/");
}).AttachKiotaHandlers(); // Attach the Kiota handlers to the http client, this is to enable all the Kiota features.

builder.Services.AddTransient(sp => sp.GetRequiredService<MaroonClientFactory>().GetClient());

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    // make all controllers require authorization (i.e. user to be logged in)
    .RequireAuthorization();

app.Run();
