export interface BaseResponse {
    type: 'Information' | 'Success' | 'Warning' | 'Error';
    message: string;
}
export type ResponseType = 'Information' | 'Success' | 'Warning' | 'Error';

function GetColor(response: BaseResponse): string {
    switch (response.type) {
        case 'Information':
            return "info";
        case 'Success':
            return "positive";
        case 'Warning':
            return "warning";
        case 'Error':
            return "negative";
        default:
            return "black";
    }
}

function IsSuccessful(response: BaseResponse): boolean {
    return response.type === 'Success' || response.type === 'Information';
}

export const ResponseUtilities = {
    GetColor,
    IsSuccessful
};