using BUS.Services.Implements;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using BUS.IServices;
using BUS.Services;
using DTO.Utils;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddDbContext<ShoeStoreContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ShoeStoreContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddScoped<IGallaryServices, GallaryServices>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ShoeStoreContext>();
builder.Services.AddTransient<CartItemServices>();
builder.Services.AddTransient<CartServices>();

builder.Services.AddScoped<IRepositoryShoe_BUS,RepositoryShoe_BUS>();
builder.Services.AddScoped<IRepositoryBrand_BUS,RepositoryBrand_BUS>();
builder.Services.AddScoped<IRepositoryFeature_BUS,RepositoryFeature_BUS>();
builder.Services.AddScoped<IRepositoryStyle_BUS,RepositoryStyle_BUS>();
builder.Services.AddScoped<IRepositoryShoeVariant_BUS,RepositoryShoeVariant_BUS>();
builder.Services.AddScoped<IRepositoryColor_BUS,RepositoryColor_BUS>();
builder.Services.AddScoped<IRepositorySize_BUS,RepositorySize_BUS>();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AutoMapperProfile>();
});
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddCors(options => options.AddPolicy(
            "AllowInternal",
            policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowedToAllowWildcardSubdomains()
            ));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowInternal");
app.MapControllers();

app.Run();