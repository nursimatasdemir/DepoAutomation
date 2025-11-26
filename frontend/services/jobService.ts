import axiosInstance from '@/utils/axiosInstance'
import {Job, CreateJobRequest} from '@/types/job'


const jobService = {
    getPendingJobs: async () => {
        const response = await axiosInstance.get<Job[]>('jobs/pending')
        return response.data;
    },
    createJob: async (data: CreateJobRequest) => {
        const response = await axiosInstance.post('jobs/', data);
        return response.data;
    },
    completeJob: async (id: string) => {
        await axiosInstance.put(`/jobs/${id}/complete`);
    }
    
    
}
export default jobService;