using Assignment2.Data;
using Assignment2.Mapping;
using Assignment2.Models;
using Assignment2.Repositories;
using Assignment2.Repositories.Interfaces;
using Assignment2.Services;
using Assignment2.Services.Interfaces;
using Assignment2.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // typeof is used to specify the open generic type, and the DI container will resolve the closed generic types at runtime when needed.
builder.Services.AddScoped<IUserService, UserService>(); // it is resolved per HTTP request, meaning a new instance will be created for each request and shared within that request. This is ideal for services that interact with the database or maintain state that should not be shared across requests.
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSingleton<IAppInfoService, AppInfoService>();
builder.Services.AddTransient<IRequestTracker, RequestTracker>(); // it is created each time it is requested. This is suitable for lightweight, stateless services that do not maintain any shared state and can be instantiated multiple times without issues.
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});


//  this is for JWT authentication, this is for token validation, it is used to validate the JWT token sent by the client in the Authorization header of HTTP requests. The token validation parameters specify how the token should be validated, including the issuer, audience, lifetime, and signing key. The signing key is used to verify the integrity and authenticity of the token, ensuring that it has not been tampered with or forged. The issuer and audience are used to ensure that the token was issued by a trusted authority and is intended for the correct audience. The lifetime validation ensures that the token has not expired and is still valid for use.
builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidateUserMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers();
app.MapRazorPages();
app.UseSwagger();
app.UseSwaggerUI();



app.Run();
