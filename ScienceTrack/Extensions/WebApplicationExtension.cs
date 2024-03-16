using Microsoft.EntityFrameworkCore;

namespace ScienceTrack.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication TryMigrate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<ScienceTrackContext>();
        if (context != null)
            context.Database.Migrate();
        else
            throw new Exception("Context not found");   
        return app;
    }
}