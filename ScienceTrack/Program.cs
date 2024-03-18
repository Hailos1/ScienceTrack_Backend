using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.FileProviders;
using ScienceTrack.Hubs;
using ScienceTrack.Repositories;
using ScienceTrack.Services;
using System.Text.Json.Serialization;
using ScienceTrack;
using ScienceTrack.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
builder.Services.AddDbContext<ScienceTrackContext>();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<Repository>();
builder.Services.AddSingleton<RandomService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<ArchiveService>();
builder.Services.AddTransient<AuthorizationService>();
builder.Services.AddTransient<ImportService>();
builder.Services.AddSingleton<RoundTimerService>();
builder.Services.AddLogging();
builder.Services.AddAuthorization();
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
                                .WithOrigins("http://127.0.0.1:3000", "http://127.0.0.1:5000", "http://127.0.0.1:80", "http://109.71.242.39:5000");
                      });
});
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
var app = builder.Build();
app.TryMigrate();
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
});
app.Run();
