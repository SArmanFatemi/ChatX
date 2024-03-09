using Microsoft.AspNetCore.SignalR;
using SAF.ChatX.Hubs.Filters;
using System.Text.Json.Serialization;

namespace SAF.ChatX.Hubs;

public static class WebApplicationExtensions
{
    public static IServiceCollection AddHubs(this IServiceCollection services)
    {
        services
            .AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.AddFilter<ApiDesignValidatorFilter>();
                options.AddFilter<RequestValidatorFilter>();
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        return services;
    }

    public static WebApplication UseHubs(this WebApplication app)
    {
        app.MapHub<ChatHub>("/chat");
        return app;
    }
}