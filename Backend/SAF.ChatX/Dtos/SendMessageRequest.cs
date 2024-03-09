using SAF.ChatX.Dtos.Common;

namespace SAF.ChatX.Dtos;

public record SendMessageRequest(string Message) : IValidatableRequest
{
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Message))
            return false;
        
        return true;
    }
}

public record SendMessageResponse(string Username, string Message);