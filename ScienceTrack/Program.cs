using ScienceTrack.Models;
using ScienceTrack.Repositories;
using Microsoft.EntityFrameworkCore;
using ScienceTrack.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Connections;
using ScienceTrack.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Repository>();
builder.Services.AddSingleton<RandomService>();
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<AuthorizationService>();
builder.Services.AddSingleton<RoundTimerService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/GameHub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
    options.WebSockets.CloseTimeout = new TimeSpan(24, 0, 0);
});

app.UseCors(x => x.AllowCredentials()
            .WithExposedHeaders("TotalPages")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000"));

app.Run();
