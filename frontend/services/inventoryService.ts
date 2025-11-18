import axiosInstance from '@/utils/axiosInstance';

import { StockLevelDTO, ReceiveStockRequest, TransferStockRequest, PickStockRequest, StockTransaction } from '@/types/inventory'; 


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
        const response = await axiosInstance.get<StockLevelDTO>(`/inventory/stock/${productId}`);
    },
    getAllStockLevels: async () => {
        const response = await axiosInstance.get<StockLevelDTO[]>('/inventory/stock/all');
        return response.data;
    },
    getProductHistory: async (productId: string) => {
        const response = await axiosInstance.get<StockTransaction[]>(`/inventory/stock/history/${productId}`);
        return response.data;
    },
    pickStock: async (data: PickStockRequest) => {
        const response = await axiosInstance.post(`/inventory/pick`, data);
        return response.data;
    }
    
}


export default inventoryService;