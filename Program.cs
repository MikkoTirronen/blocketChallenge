using System.Text;
using BlocketChallenge.Endpoints;
using BlocketChallenge.Project.ConnectionFactories;
using BlocketChallenge.Project.Core.Interfaces;
using BlocketChallenge.Project.Core.Services;
using BlocketChallenge.Project.Data.Interfaces;
using BlocketChallenge.Project.Data.Repositories;
using BlocketChallenge.Services;
using BlocketClallenge.Project.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
Log.Information("Program.cs using key: {Key}", builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("Jwt:Key missing in configuration.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddAuthorization();

builder.Services.AddSingleton(_ =>
    new DbConnectionFactory(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new ArgumentNullException("DefaultConnection", "Database connection string is missing from configuration.")
    )
);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite",
        policy => policy
            .WithOrigins("http://localhost:5173") // Vite dev server
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowVite");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapAdvertisementEndpoints();
app.MapUserEndpoints();


app.Run();

