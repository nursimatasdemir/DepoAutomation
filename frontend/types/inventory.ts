export interface StockLevelDTO {
    productId: string;
    totalQuantity: number;
}

export interface StockTransaction {
    id: string;
    timestamp: string;
    locationId: string;
    quantityChange: number;
    sourceDocument: string;
}

export interface ReceiveStockRequest {
    productId: string;
    locationId: string;
    quantityReceived: number;
    sourceDocument: string;
}

export interface TransferStockRequest {
    productId: string;
    sourceLocationId: string;
    destinationLocationId: string;
    quantityToTransfer: number;
    sourceDocument: string;
}

export interface PickStockRequest {
    productId: string;
    sourceLocationId: string;
    quantityToPick: number;
    sourceDocument: string;
}