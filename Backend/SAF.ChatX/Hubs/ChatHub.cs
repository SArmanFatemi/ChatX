using Microsoft.AspNetCore.SignalR;
using SAF.ChatX.Dtos.Requests;
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
            // TODO: Return a response to the client indicating that the username already exists in the room
        }
        
        UserConnection connection = new(Context.ConnectionId, request.Username, request.Room);
        databaseContext.Connections.Add(connection);
        
        await Groups.AddToGroupAsync(connection.ConnectionId, connection.Room);
        await Clients.Group(connection.Room)
            .SendAsync(nameof(JoinRoom), request.Username, $"{request.Username} joined the {request.Room}");
    }

    public async Task SendMessage(SendMessageRequest request)
    {
        var existingConnection = databaseContext.Connections.SingleOrDefault(c => c.ConnectionId == Context.ConnectionId);
        if (existingConnection is not null)
        {
            await Clients.Group(existingConnection.Room)
                .SendAsync(nameof(SendMessage), existingConnection.Username, request.Message);
        }
    }
}