using Microsoft.AspNetCore.SignalR;
using SAF.ChatX.Dtos;
using SAF.ChatX.Dtos.Common;
using SAF.ChatX.Models;
using SAF.ChatX.Persistence;

namespace SAF.ChatX.Hubs;

public class ChatHub(DatabaseContext databaseContext) : Hub
{
    public async Task JoinRoom(JoinRoomRequest request)
    {
        var usernameAlreadyExistsInRoom = databaseContext.Connections
            .Where(c => c.Room == request.Room)
            .Any(c => c.Username == request.Username);
        if (usernameAlreadyExistsInRoom)
        {
            await Clients.Caller
                .SendAsync(nameof(JoinRoom), new JoinRequestBaseResponse(string.Empty, "Username already exists in the room", ResponseTypeEnum.Error));
            return;
        }
        
        UserConnection connection = new(Context.ConnectionId, request.Username, request.Room);
        databaseContext.Connections.Add(connection);
        
        await Groups.AddToGroupAsync(connection.ConnectionId, connection.Room);
        // Send welcome message to caller
        await Clients.Caller
            .SendAsync(nameof(JoinRoom), new JoinRequestBaseResponse(request.Username, $"Dear {connection.Username}, You joined the {connection.Room}"));
        // Send notification to other users in the room
        await Clients.GroupExcept(connection.Room, connection.ConnectionId)
            .SendAsync(nameof(JoinRoom), new JoinRequestBaseResponse(request.Username, $"{request.Username} joined the {request.Room}"));
    }

    public async Task SendMessage(SendMessageRequest request)
    {
        var existingConnection = databaseContext.Connections.SingleOrDefault(c => c.ConnectionId == Context.ConnectionId);
        if (existingConnection is not null)
        {
            await Clients.Group(existingConnection.Room)
                .SendAsync(nameof(SendMessage), new SendMessageResponse(existingConnection.Username, request.Message));
        }
    }
}