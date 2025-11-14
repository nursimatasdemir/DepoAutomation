import axiosInstance from "../utils/axiosInstance";
import {Location} from "../types/location";

const locationService = {
    getAll: async () => {
        const response = await axiosInstance.get<Location[]>("/locations");
        return response.data;
    },
}

export default locationService;