import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Request interceptor to add auth token
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Response interceptor for error handling
api.interceptors.response.use(
    (response) => response,
    async (error) => {
        if (error.response?.status === 401) {
            localStorage.removeItem('token');
            localStorage.removeItem('user');
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

// Auth API
export const authApi = {
    login: (data: { email: string; password: string }) =>
        api.post('/auth/login', data),
    register: (data: { email: string; password: string; fullName: string }) =>
        api.post('/auth/register', data),
    logout: () => api.post('/auth/logout'),
    me: () => api.get('/auth/me'),
    changePassword: (data: { currentPassword: string; newPassword: string }) =>
        api.post('/auth/change-password', data),
};

// Products API
export const productsApi = {
    getAll: (params?: { page?: number; pageSize?: number; search?: string }) =>
        api.get('/products', { params }),
    getById: (id: number) => api.get(`/products/${id}`),
    create: (data: FormData) => api.post('/products', data),
    update: (id: number, data: FormData) => api.put(`/products/${id}`, data),
    delete: (id: number) => api.delete(`/products/${id}`),
    approve: (id: number) => api.post(`/products/${id}/approve`),
    publish: (id: number) => api.post(`/products/${id}/publish`),
};

// Articles API
export const articlesApi = {
    getAll: (params?: { page?: number; pageSize?: number; search?: string }) =>
        api.get('/articles', { params }),
    getById: (id: number) => api.get(`/articles/${id}`),
    create: (data: FormData) => api.post('/articles', data),
    update: (id: number, data: FormData) => api.put(`/articles/${id}`, data),
    delete: (id: number) => api.delete(`/articles/${id}`),
    approve: (id: number) => api.post(`/articles/${id}/approve`),
    publish: (id: number) => api.post(`/articles/${id}/publish`),
};

// Orders API
export const ordersApi = {
    getAll: (params?: { page?: number; pageSize?: number; status?: string }) =>
        api.get('/orders', { params }),
    getById: (id: number) => api.get(`/orders/${id}`),
    updateStatus: (id: number, status: string) =>
        api.put(`/orders/${id}/status`, { status }),
    confirm: (id: number) => api.post(`/orders/${id}/confirm`),
    ship: (id: number) => api.post(`/orders/${id}/ship`),
    deliver: (id: number) => api.post(`/orders/${id}/deliver`),
    cancel: (id: number, reason: string) =>
        api.post(`/orders/${id}/cancel`, { reason }),
};

// Dashboard API
export const dashboardApi = {
    getSummary: () => api.get('/dashboard/summary'),
    getToday: () => api.get('/dashboard/today'),
    getTopProducts: (count?: number) =>
        api.get('/dashboard/top-products', { params: { count } }),
    getRecentOrders: (count?: number) =>
        api.get('/dashboard/recent-orders', { params: { count } }),
};
