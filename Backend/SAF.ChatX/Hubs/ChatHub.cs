using Microsoft.AspNetCore.SignalR;
using SAF.ChatX.Dtos.Requests;
using SAF.ChatX.Models;
using SAF.ChatX.Persistence;

namespace SAF.ChatX.Hubs;

public class ChatHub(DatabaseContext databaseContext) : Hub
{
    public async Task JoinSpecificChatRoom(UserConnectionRequest request)
    {

        var usernameAlreadyExistsInChatroom = databaseContext.Connections
            .Where(c => c.ChatRoom == request.ChatRoom)
            .Any(c => c.Username == request.Username);
        if (usernameAlreadyExistsInChatroom)
        {
            // TODO: Return a response to the client indicating that the username already exists in the chatroom
        }
        
        UserConnection connection = new(Context.ConnectionId, request.Username, request.ChatRoom);
        databaseContext.Connections.Add(connection);
        
        await Groups.AddToGroupAsync(connection.ConnectionId, connection.ChatRoom);
        await Clients.Group(connection.ChatRoom)
            .SendAsync(nameof(JoinSpecificChatRoom), request.Username, $"{request.Username} joined the {request.ChatRoom}");
    }

    public async Task SendMessage(string message)
    {
        var existingConnection = databaseContext.Connections.SingleOrDefault(c => c.ConnectionId == Context.ConnectionId);
        if (existingConnection is not null)
        {
            await Clients.Group(existingConnection.ChatRoom)
                .SendAsync(nameof(SendMessage), existingConnection.Username, message);
        }
    }
}