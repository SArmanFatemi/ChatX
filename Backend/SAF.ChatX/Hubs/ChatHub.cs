using Microsoft.AspNetCore.SignalR;
using SAF.ChatX.Models;
using SAF.ChatX.Persistence;

namespace SAF.ChatX.Hubs;

public class ChatHub(DatabaseContext databaseContext) : Hub
{
    public async Task JoinSpecificChatRoom(UserConnection connection)
    {
        databaseContext.Connections[Context.ConnectionId] = connection;
        
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        await Clients.Group(connection.ChatRoom)
            .SendAsync(nameof(JoinSpecificChatRoom), connection.Username, $"{connection.Username} joined the {connection.ChatRoom}");
    }

    public async Task SendMessage(string message)
    {
        if (databaseContext.Connections.TryGetValue(Context.ConnectionId, out var connection))
        {
            await Clients.Group(connection.ChatRoom)
                .SendAsync(nameof(SendMessage), connection.Username, message);
        }
    }
}