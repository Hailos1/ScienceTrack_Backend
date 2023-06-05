using ScienceTrack.Models;
using ScienceTrack.Repositories;
using Microsoft.EntityFrameworkCore;
using ScienceTrack.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Connections;
using ScienceTrack.Hubs;
using Microsoft.Extensions.FileProviders;

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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Files")),
    RequestPath = "/Files",
    OnPrepareResponse = (context) =>
    {
        // Disable caching of all static files.
        context.Context.Response.Headers["Cache-Control"] = "no-cache, no-store";
        context.Context.Response.Headers["Pragma"] = "no-cache";
        context.Context.Response.Headers["Expires"] = "-1";
        context.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    }
});

app.Run();
