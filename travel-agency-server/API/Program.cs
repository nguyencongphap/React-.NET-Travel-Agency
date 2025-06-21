using API.Handlers;
using Application.Abstracts;
using Application.Services;
using Infrastructure;
using Infrastructure.Options;
using Infrastructure.Processors;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using travel_agency_server.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// load jwt env vars into the JwtOptions class and inject it for Infrastructure proj to work with
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

// set up identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<ApplicationDbContext>();

// set up db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString"));
});

// for dependency injection
// AddScoped means that the service will be created once per request and disposed of at the end of the request
builder.Services.AddScoped<IAuthTokenProcessor, AuthTokenProcessor>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddHttpContextAccessor(); // to get cookies from client's request

// add auth middleware
// This scheme means we want our app to use JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // use the jwt env vals in appsettings
    var jwtOptions = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey)
                        .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        // settings for validating jwt
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["ACCESS_TOKEN"];
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorization();

// add exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Use CORS before other middleware
    app.UseCors("AllowReactApp");

    app.MapOpenApi();

    // set up scalar
    app.MapScalarApiReference(opt =>
    {
        opt.WithTitle("JWT + Refresh Token Auth API");
    });
}

// must register the exception handler like this for it to work
app.UseExceptionHandler(
    _ => { } 
);

app.UseHttpsRedirection();

// this order matters
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// To create migrations:
// dotnet ef migrations add MigrationName -s .\API\ -p .\Infrastructure\

// To run migrations:
// dotnet ef database update -s .\API\ -p .\Infrastructure\