using ScienceTrack.Models;
using ScienceTrack.Repositories;
using Microsoft.EntityFrameworkCore;
using ScienceTrack.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Repository>();
builder.Services.AddSingleton<RandomService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddTransient<AuthorizationService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

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

app.UseCors(x => x.AllowCredentials()
            .WithExposedHeaders("TotalPages")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000"));

app.Run();
