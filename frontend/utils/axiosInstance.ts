import axios from "axios";

const API_BASE_URL = "http://localhost:5195/api";

const axiosInstance = axios.create({
    baseURL: API_BASE_URL,
    headers: {'Content-Type': 'application/json'
    },
});

axiosInstance.interceptors.request.use(
    (config) => {
        console.log("Interceptor çalıştı! istek hazırlanıyor");
        const token = localStorage.getItem("token");
        console.log("LocalStorage'dan okunan token:", token)
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
            console.log("Header Eklendi! Son hali:", config.headers['Authorization']);
        } else {console.warn("UYARI: Token bulunamadı, Header eklenmiyor!");}
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

axiosInstance.interceptors.response.use(
    (response) => response,
        (error) => {
            if (error.response && error.response.status === 401) {
                localStorage.removeItem("token");
                window.location.href = "/login";
            }
            return Promise.reject(error);
        }
);

export default axiosInstance;