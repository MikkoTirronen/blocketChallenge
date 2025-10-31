using BlocketChallenge.ConnectionFactories;
using BlocketChallenge.Endpoints;
using BlocketChallenge.Repositories;
using BlocketChallenge.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(_ =>
    new DbConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapAdvertisementEndpoints();

app.Run();