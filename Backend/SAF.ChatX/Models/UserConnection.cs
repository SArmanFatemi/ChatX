namespace SAF.ChatX.Models;

public class UserConnection(string connectionId, string username, string chatRoom)
{
    public string ConnectionId { get; set; } = connectionId;

    public string Username { get; set; } = username;

    public string ChatRoom { get; set; } = chatRoom;
}