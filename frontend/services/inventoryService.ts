import axiosInstance from '@/utils/axiosInstance';

export interface ReceiveStockRequest {
    productId: string;
    locationId: string;
    quantityReceived: number;
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
    getStockLevel: async (productId: string) => {
        const response = await axiosInstance.get<StockLevelDto>(`/inventory/stock/${productId}`);
    }
}

export default inventoryService;