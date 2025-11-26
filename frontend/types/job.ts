export interface JobItem {
    id: string;
    productId: string;
    productName: string;
    sourceLocationId?: string;
    targetLocationId?: string;
    quantity: number;
}

export interface Job {
    id: string;
    type: string;
    status: string;
    sourceDocument: string;
    createdAt: string;
    items: JobItem[];
}

export enum JobType {
    Yerleştirme = 0,
    Toplama = 1,
    Transfer = 2,
    Sayım = 3
}

export interface CreateJobItemRequest {
    productId: string;
    quantity: number;
    sourceLocationId?: string;
    targetLocationId?: string;
}

export interface CreateJobRequest {
    type: JobType;
    sourceDocument: string;
    items: CreateJobItemRequest[];
}