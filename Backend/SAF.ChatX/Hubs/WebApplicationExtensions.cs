using Microsoft.AspNetCore.SignalR;
using SAF.ChatX.Hubs.Filters;

namespace SAF.ChatX.Hubs;

public static class WebApplicationExtensions
{
    public static IServiceCollection AddHubs(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.AddFilter<RequestValidatorFilter>();
        });
        return services;
    }
    
    public static WebApplication UseHubs(this WebApplication app)
    {
        app.MapHub<ChatHub>("/chat");
        return app;
    }
}