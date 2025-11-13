import axiosInstance from "../utils/axiosInstance";
import {Product, CreateProductRequest} from "../types/product";

const productService = {
    getAll: async () => {
        const response = await axiosInstance.get<Product[]>('/Products');
        return response.data;
    },
    
    getById: async (id: string) => {
        const response = await axiosInstance.get<Product[]>(`/Products/${id}`);
        return response.data;
    },
    
    create: async (data: CreateProductRequest) => {
        const response = await axiosInstance.post(`/Products`, data);
        return response.data;
    },
    
    delete: async (id: string) => {
        await axiosInstance.delete(`/Products/${id}`);
    }
};

export default productService;