export interface Location {
    id: string;
    code: string;
    type: string;
}

export interface CreateLocationRequest {
    code: string;
    type: string;
}
