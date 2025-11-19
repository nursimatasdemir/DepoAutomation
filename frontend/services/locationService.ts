import axiosInstance from "@/utils/axiosInstance";
import {Location, CreateLocationRequest, UpdateLocationRequest} from "@/types/location";


const locationService = {
    getAll: async () => {
        const response = await axiosInstance.get<Location[]>("/locations");
        return response.data;
    },
    getById: async (id: string) => {
        const response = await axiosInstance.get<Location>(`/locations/${id}`);
        return response.data;
    },
    create: async (location: CreateLocationRequest) => {
        const response = await axiosInstance.post('/locations', location)
        return response.data;
    },
    update: async (id:string, location: UpdateLocationRequest) => {
        await axiosInstance.put(`/locations/${id}`, location)
    },
    delete: async (id: string) => {
        await axiosInstance.delete(`/locations/${id}`);
    }
}

export default locationService;