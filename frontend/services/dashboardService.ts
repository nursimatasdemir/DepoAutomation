import axiosInstance from '@/utils/axiosInstance';
import {CatalogStats, InventoryStats} from '@/types/dashboard';

const dashboardService = {
    getCatalogStats: async () => {
        const res = await axiosInstance.get<CatalogStats>('/catalog/Dashboard/stats');
        return res.data;
    }, 
    getInventoryStats: async () => {
        const res = await axiosInstance.get<InventoryStats>('/inventory/Dashboard/stats');
        return res.data;
    }
}
export default dashboardService;
