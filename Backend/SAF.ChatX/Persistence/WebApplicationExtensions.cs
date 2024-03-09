namespace SAF.ChatX.Persistence;

public static class WebApplicationExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseContext>();
        return services;
    } 
}