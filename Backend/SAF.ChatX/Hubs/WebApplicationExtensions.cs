namespace SAF.ChatX.Hubs;

public static class WebApplicationExtensions
{
    public static IServiceCollection AddHubs(this IServiceCollection services)
    {
        services.AddSignalR(option => option.EnableDetailedErrors = true);
        return services;
    }
    
    public static WebApplication UseHubs(this WebApplication app)
    {
        app.MapHub<ChatHub>("/chat");
        return app;
    }
}