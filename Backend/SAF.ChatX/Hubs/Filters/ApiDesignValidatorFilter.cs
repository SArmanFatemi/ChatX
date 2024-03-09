using Microsoft.AspNetCore.SignalR;

namespace SAF.ChatX.Hubs.Filters;

public class ApiDesignValidatorFilter : IHubFilter
{
    public async ValueTask<object?>
        InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
    {
        // Based on the documentation, the hub method should have only one parameter for having backward compatibility with the JavaScript client
        // https://learn.microsoft.com/en-us/aspnet/core/signalr/api-design
        if (invocationContext.HubMethodArguments.Count != 1)
        {
            throw new HubException(
                $"INTERNAL ERROR (DEV): The hub method [${invocationContext.HubMethodName}] should have only one parameter");
        }
        return await next(invocationContext);
    }
}