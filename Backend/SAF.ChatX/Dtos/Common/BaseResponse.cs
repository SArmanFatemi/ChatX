namespace SAF.ChatX.Dtos.Common;

public record BaseResponse(string Message, ResponseTypeEnum Type);

public enum ResponseTypeEnum
{
    Information = 10,
    Success = 20,
    Warning = 30,
    Error = 40
}