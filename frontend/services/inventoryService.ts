import axiosInstance from '@/utils/axiosInstance';

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
export interface StockLevelDto {
    productId: string;
    totalQuantity: number;
}

const inventoryService = {
    receiveStock: async (data: ReceiveStockRequest) => {
        const response = await axiosInstance.post('/inventory/receive', data);
        return response.data;
    },
    transferStock: async (data: TransferStockRequest) => {
        const response = await axiosInstance.post('/inventory/transfer', data);
        return response.data;
    },
    getStockLevel: async (productId: string) => {
        const response = await axiosInstance.get<StockLevelDto>(`/inventory/stock/${productId}`);
    },
    pickStock: async (data: PickStockRequest) => {
        const response = await axiosInstance.post(`/inventory/pick`, data);
        return response.data;
    }
}


export default inventoryService;