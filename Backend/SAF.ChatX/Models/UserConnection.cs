namespace SAF.ChatX.Models;

public class UserConnection(string connectionId, string username, string room)
{
    public string ConnectionId { get; set; } = connectionId;

    public string Username { get; set; } = username;

    public string Room { get; set; } = room;
}