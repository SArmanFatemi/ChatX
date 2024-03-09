namespace SAF.ChatX.Dtos;

public record SendMessageRequest(string Message);
public record SendMessageResponse(string Username, string Message);