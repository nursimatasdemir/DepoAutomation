import axiosInstance from '@/utils/axiosInstance';
import { Category, CreateCategoryRequest, UpdateCategoryRequest } from '@/types/category';

const categoryService = {
    getAll: async () => {
        const response = await axiosInstance.get<Category[]>('/categories');
        return response.data;
    },
    getById: async (id: string) => {
        const response = await axiosInstance.get<Category>(`/categories/${id}`);
        return response.data;
    },
    create: async (data: CreateCategoryRequest) => {
        const response = await axiosInstance.post('/categories', data);
        return response.data;
    },
    update: async (id: string, data: UpdateCategoryRequest) => {
        await axiosInstance.put(`/categories/${id}`, data);
    },
    delete: async (id: string) => {
        await axiosInstance.delete(`/categories/${id}`);
    }
};

export default categoryService;