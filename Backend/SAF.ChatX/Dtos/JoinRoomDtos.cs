namespace SAF.ChatX.Dtos;

public record JoinRoomRequest(string Username, string Room);
public record JoinRequestResponse(string Username, string Message);