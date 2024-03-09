import { BaseResponse } from "../Common.responses";

export interface JoinRoomResponse extends BaseResponse {
    username: string;
    room: string;
}