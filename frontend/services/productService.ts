import axiosInstance from "../utils/axiosInstance";
import {Product, CreateProductRequest} from "../types/product";

const productService = {
    getAll: async (searchTerm: string = '', includeArchived: boolean = false) => {
        const response = await axiosInstance.get<Product[]>('/products', {
            params: {
                searchTerm,
                includeArchived
            }
        });
        return response.data;
    },
    
    getById: async (id: string) => {
        const response = await axiosInstance.get<Product[]>(`/products/${id}`);
        return response.data;
    },
    
    create: async (data: CreateProductRequest) => {
        const response = await axiosInstance.post(`/products`, data);
        return response.data;
    },
    
    delete: async (id: string) => {
        await axiosInstance.delete(`/products/${id}`);
    }
};

export default productService;