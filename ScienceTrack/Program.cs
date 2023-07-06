using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using ScienceTrack.Hubs;
using ScienceTrack.Repositories;
using ScienceTrack.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Repository>();
builder.Services.AddSingleton<RandomService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<ArchiveService>();
builder.Services.AddTransient<AuthorizationService>();
builder.Services.AddSingleton<RoundTimerService>();
builder.Services.AddAuthorization();
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/";
        options.AccessDeniedPath = "/";
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "cors",
                      policy =>
                      {
                          policy.WithExposedHeaders("TotalPages", "TotalCount")
                            .AllowCredentials()
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .WithOrigins("http://localhost:3000");
                      });
});
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => c.RouteTemplate = "api/swagger/{documentname}/swagger.json");
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "API");
        c.RoutePrefix = "api/swagger";
    });
}
app.UseCookiePolicy();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/api/GameHub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
    options.WebSockets.CloseTimeout = new TimeSpan(24, 0, 0);
});

app.UseCors("cors");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Files")),
    RequestPath = "/api/Files"
    //OnPrepareResponse = (context) =>
    //{
    //    // Disable caching of all static files.
    //    context.Context.Response.Headers["Cache-Control"] = "no-cache, no-store";
    //    context.Context.Response.Headers["Pragma"] = "no-cache";
    //    context.Context.Response.Headers["Expires"] = "-1";
    //    context.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    //}
});

app.Run();
