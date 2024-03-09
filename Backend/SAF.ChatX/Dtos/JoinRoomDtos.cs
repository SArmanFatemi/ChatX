using SAF.ChatX.Dtos.Common;

namespace SAF.ChatX.Dtos;

public record JoinRoomRequest(string Username, string Room) : IValidatableRequest
{
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Username))
            return false;
        
        if (string.IsNullOrWhiteSpace(Room))
            return false;
        
        return true;
    }
}

public record JoinRequestBaseResponse(string Username, string Room, string Message, ResponseTypeEnum Type = ResponseTypeEnum.Information) 
    : BaseResponse(Message, Type);