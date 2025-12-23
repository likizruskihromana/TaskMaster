import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7015/",
});

// Interceptor koji automatski dodaje token u svaki zahtjev
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default api;