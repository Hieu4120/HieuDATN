using BlazorStrap;
using DATN;
using DATN.Data;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static DATN.Services.NotificationServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, Authentication>();
builder.Services.AddScoped<IRedirectSevices, RedirectSevices>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<INotificationService, NotiService>();

builder.Services.AddTransient<IGenreServices, GenreServices>();
builder.Services.AddTransient<IBookServices, BookServices>();
builder.Services.AddTransient<ISupplierServices, SupplierServices>();
builder.Services.AddTransient<IReviewServices, ReviewServices>();
builder.Services.AddTransient<ICartServices, CartServices>();
builder.Services.AddTransient<ICarouselSevices, CaroselServices>();
builder.Services.AddTransient<IImportOrderDetailServices, ImportOrderDetailServices>();
builder.Services.AddTransient<ISaleOrderDetailServices, SaleOrderDetailServices>();
builder.Services.AddTransient<IImportOrderServices, ImportOrderServices>();
builder.Services.AddTransient<ISaleOrderServices, SaleOrderServices>();
builder.Services.AddTransient<IFeedBackServices, FeedBackServices>();



builder.Services.AddBlazorStrap();

//var app = builder.Build();
builder.Services.AddDbContextFactory<BookDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectString")));

builder.Services.AddTransient<IAccountServices, AccountServices>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
