using BUS.Services.Implements;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using Shoe.Controllers;
using Microsoft.EntityFrameworkCore;
using BUS.IServices;
using BUS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<ShoeStoreContext>();
builder.Services.AddTransient<CartItemServices>();
builder.Services.AddTransient<CartServices>();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepositoryShoe_BUS, RepositoryShoe_BUS>();
builder.Services.AddScoped<IRepositoryBrand_BUS, RepositoryBrand_BUS>();
builder.Services.AddScoped<IRepositoryFeature_BUS, RepositoryFeature_BUS>();
builder.Services.AddScoped<IRepositoryStyle_BUS, RepositoryStyle_BUS>();
builder.Services.AddScoped<IRepositoryShoeVariant_BUS, RepositoryShoeVariant_BUS>();
builder.Services.AddScoped<IRepositoryColor_BUS, RepositoryColor_BUS>();
builder.Services.AddScoped<IRepositorySize_BUS, RepositorySize_BUS>();
builder.Services.AddScoped<IGallaryServices, GallaryServices>();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AutoMapperProfile>();
});


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShoeStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Context")));
builder.Services.AddScoped<IServiceVoucher_Bus, Service_Voucher>();
builder.Services.AddScoped<IServiceVoucherBank, ServiceVoucher_Bank>();
builder.Services.AddTransient<Service_Voucher>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
