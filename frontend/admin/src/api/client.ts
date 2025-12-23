import axios, { type AxiosResponse } from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5005/api';

export const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Types for API responses
export interface PaginationInfo {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalItems: number;
    totalPages: number;
    hasPrevious: boolean;
    hasNext: boolean;
}

export interface ApiResponse<T> {
    success: boolean;
    data: T;
    message?: string;
    errors: string[];
    pagination?: PaginationInfo;
}

// Helper to extract list data and pagination from API response
export function parseListResponse<T>(response: AxiosResponse<ApiResponse<T[]>>) {
    const { data, pagination } = response.data;
    return {
        items: data || [],
        pagination: pagination || {
            currentPage: 1,
            pageSize: 20,
            totalCount: 0,
            totalItems: 0,
            totalPages: 1,
            hasPrevious: false,
            hasNext: false,
        },
    };
}

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

// Products API - map search -> keyword for backend
export const productsApi = {
    getAll: (params?: { page?: number; pageSize?: number; search?: string }) => {
        const { search, ...rest } = params || {};
        return api.get('/products', { 
            params: { ...rest, keyword: search } 
        });
    },
    getById: (id: number) => api.get(`/products/${id}`),
    create: (data: Record<string, unknown>) => api.post('/products', data),
    update: (id: number, data: Record<string, unknown>) => api.put(`/products/${id}`, data),
    delete: (id: number) => api.delete(`/products/${id}`),
    approve: (id: number) => api.post(`/products/${id}/approve`),
    publish: (id: number) => api.post(`/products/${id}/publish`),
    getCategories: () => api.get('/masterdata/product-categories'),
    getTypes: () => api.get('/masterdata/product-types'),
    getStatuses: () => api.get('/masterdata/product-statuses'),
};

// Articles API - map search -> keyword for backend
export const articlesApi = {
    getAll: (params?: { page?: number; pageSize?: number; search?: string }) => {
        const { search, ...rest } = params || {};
        return api.get('/articles', { 
            params: { ...rest, keyword: search } 
        });
    },
    getById: (id: number) => api.get(`/articles/${id}`),
    create: (data: Record<string, unknown>) => api.post('/articles', data),
    update: (id: number, data: Record<string, unknown>) => api.put(`/articles/${id}`, data),
    delete: (id: number) => api.delete(`/articles/${id}`),
    approve: (id: number) => api.post(`/articles/${id}/approve`),
    publish: (id: number) => api.post(`/articles/${id}/publish`),
    getCategories: () => api.get('/masterdata/article-categories'),
    getStatuses: () => api.get('/masterdata/article-statuses'),
};

// Orders API - fixed to match backend contract
export const ordersApi = {
    getAll: (params?: { page?: number; pageSize?: number; productOrderStatusId?: number }) => {
        return api.get('/orders', { params });
    },
    getById: (id: number) => api.get(`/orders/${id}`),
    // Backend expects: { productOrderStatusId: number, note?: string }
    updateStatus: (id: number, productOrderStatusId: number, note?: string) =>
        api.put(`/orders/${id}/status`, { productOrderStatusId, note }),
    // Backend expects: { productOrderPaymentStatusId: number, transactionId?: string, note?: string }
    updatePaymentStatus: (id: number, productOrderPaymentStatusId: number, transactionId?: string, note?: string) =>
        api.put(`/orders/${id}/payment-status`, { productOrderPaymentStatusId, transactionId, note }),
    confirm: (id: number) => api.post(`/orders/${id}/confirm`),
    ship: (id: number) => api.post(`/orders/${id}/ship`),
    deliver: (id: number) => api.post(`/orders/${id}/deliver`),
    complete: (id: number) => api.post(`/orders/${id}/complete`),
    cancel: (id: number, cancelReason: string) =>
        api.post(`/orders/${id}/cancel`, { cancelReason }),
    // Lookup data
    getStatuses: () => api.get('/orders/statuses'),
    getPaymentMethods: () => api.get('/orders/payment-methods'),
    getPaymentStatuses: () => api.get('/orders/payment-statuses'),
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

// Files API - for GCS upload
export const filesApi = {
    uploadImage: (file: File, folder: string = 'images') => {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('folder', folder);
        return api.post('/files/image', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
    },
    uploadFile: (file: File, folder: string = 'files') => {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('folder', folder);
        return api.post('/files/upload', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
    },
    delete: (objectPath: string) => api.delete('/files', { data: { objectPath } }),
};

// Master Data API
export const masterDataApi = {
    getArticleCategories: () => api.get('/masterdata/article-categories'),
    getArticleStatuses: () => api.get('/masterdata/article-statuses'),
    getArticleTypes: () => api.get('/masterdata/article-types'),
    getProductCategories: () => api.get('/masterdata/product-categories'),
    getProductStatuses: () => api.get('/masterdata/product-statuses'),
    getProductTypes: () => api.get('/masterdata/product-types'),
    getProductManufactures: () => api.get('/masterdata/product-manufactures'),
    getUnits: () => api.get('/masterdata/units'),
    getLocations: () => api.get('/masterdata/locations'),
};

// Product Brands API
export const productBrandsApi = {
    getAll: (params?: { page?: number; pageSize?: number; keyword?: string; active?: boolean }) =>
        api.get('/productbrands', { params }),
    getById: (id: number) => api.get(`/productbrands/${id}`),
    create: (data: Record<string, unknown>) => api.post('/productbrands', data),
    update: (id: number, data: Record<string, unknown>) => api.put(`/productbrands/${id}`, data),
    delete: (id: number) => api.delete(`/productbrands/${id}`),
    toggleStatus: (id: number) => api.post(`/productbrands/${id}/toggle-status`),
};

// Advertising API
export const advertisingApi = {
    // Blocks
    getBlocks: (params?: { page?: number; pageSize?: number; keyword?: string; active?: boolean }) =>
        api.get('/advertisingadmin/blocks', { params }),
    getBlockById: (id: number) => api.get(`/advertisingadmin/blocks/${id}`),
    createBlock: (data: Record<string, unknown>) => api.post('/advertisingadmin/blocks', data),
    updateBlock: (id: number, data: Record<string, unknown>) => api.put(`/advertisingadmin/blocks/${id}`, data),
    deleteBlock: (id: number) => api.delete(`/advertisingadmin/blocks/${id}`),
    toggleBlockStatus: (id: number) => api.post(`/advertisingadmin/blocks/${id}/toggle-status`),
    // Items
    getItems: (blockId: number, params?: { page?: number; pageSize?: number }) =>
        api.get(`/advertisingadmin/blocks/${blockId}/items`, { params }),
    createItem: (blockId: number, data: Record<string, unknown>) => 
        api.post(`/advertisingadmin/blocks/${blockId}/items`, data),
    updateItem: (blockId: number, itemId: number, data: Record<string, unknown>) =>
        api.put(`/advertisingadmin/blocks/${blockId}/items/${itemId}`, data),
    deleteItem: (blockId: number, itemId: number) =>
        api.delete(`/advertisingadmin/blocks/${blockId}/items/${itemId}`),
};

// Blocks API (Article/Product blocks)
export const blocksApi = {
    // Article blocks
    getArticleBlocks: (params?: { page?: number; pageSize?: number; keyword?: string; active?: boolean }) =>
        api.get('/blocks/articles', { params }),
    getArticleBlockById: (id: number) => api.get(`/blocks/articles/${id}`),
    createArticleBlock: (data: Record<string, unknown>) => api.post('/blocks/articles', data),
    updateArticleBlock: (id: number, data: Record<string, unknown>) => api.put(`/blocks/articles/${id}`, data),
    deleteArticleBlock: (id: number) => api.delete(`/blocks/articles/${id}`),
    addArticleToBlock: (blockId: number, articleId: number) =>
        api.post(`/blocks/articles/${blockId}/items/${articleId}`),
    removeArticleFromBlock: (blockId: number, articleId: number) =>
        api.delete(`/blocks/articles/${blockId}/items/${articleId}`),
    // Product blocks
    getProductBlocks: (params?: { page?: number; pageSize?: number; keyword?: string; active?: boolean }) =>
        api.get('/blocks/products', { params }),
    getProductBlockById: (id: number) => api.get(`/blocks/products/${id}`),
    createProductBlock: (data: Record<string, unknown>) => api.post('/blocks/products', data),
    updateProductBlock: (id: number, data: Record<string, unknown>) => api.put(`/blocks/products/${id}`, data),
    deleteProductBlock: (id: number) => api.delete(`/blocks/products/${id}`),
    addProductToBlock: (blockId: number, productId: number) =>
        api.post(`/blocks/products/${blockId}/items/${productId}`),
    removeProductFromBlock: (blockId: number, productId: number) =>
        api.delete(`/blocks/products/${blockId}/items/${productId}`),
};

// Users Admin API
export const usersAdminApi = {
    getAll: (params?: { page?: number; pageSize?: number; keyword?: string; roleId?: string }) =>
        api.get('/usersadmin', { params }),
    getById: (id: string) => api.get(`/usersadmin/${id}`),
    create: (data: Record<string, unknown>) => api.post('/usersadmin', data),
    update: (id: string, data: Record<string, unknown>) => api.put(`/usersadmin/${id}`, data),
    delete: (id: string) => api.delete(`/usersadmin/${id}`),
    setRoles: (id: string, roles: string[]) => api.put(`/usersadmin/${id}/roles`, { roles }),
    lockUser: (id: string) => api.post(`/usersadmin/${id}/lock`),
    unlockUser: (id: string) => api.post(`/usersadmin/${id}/unlock`),
    resetPassword: (id: string, newPassword: string) =>
        api.post(`/usersadmin/${id}/reset-password`, { newPassword }),
    getRoles: () => api.get('/usersadmin/roles'),
};

// Settings API
export const settingsApi = {
    get: () => api.get('/settings'),
    update: (data: Record<string, unknown>) => api.put('/settings', data),
};

// Categories Admin API
export const categoriesAdminApi = {
    // Article categories
    getArticleCategories: (params?: { includeInactive?: boolean }) =>
        api.get('/admin/categories/articles', { params }),
    getArticleCategoryById: (id: number) => api.get(`/admin/categories/articles/${id}`),
    createArticleCategory: (data: Record<string, unknown>) => api.post('/admin/categories/articles', data),
    updateArticleCategory: (id: number, data: Record<string, unknown>) =>
        api.put(`/admin/categories/articles/${id}`, data),
    deleteArticleCategory: (id: number) => api.delete(`/admin/categories/articles/${id}`),
    toggleArticleCategoryStatus: (id: number) => api.post(`/admin/categories/articles/${id}/toggle-status`),
    // Product categories
    getProductCategories: (params?: { includeInactive?: boolean }) =>
        api.get('/admin/categories/products', { params }),
    getProductCategoryById: (id: number) => api.get(`/admin/categories/products/${id}`),
    createProductCategory: (data: Record<string, unknown>) => api.post('/admin/categories/products', data),
    updateProductCategory: (id: number, data: Record<string, unknown>) =>
        api.put(`/admin/categories/products/${id}`, data),
    deleteProductCategory: (id: number) => api.delete(`/admin/categories/products/${id}`),
    toggleProductCategoryStatus: (id: number) => api.post(`/admin/categories/products/${id}/toggle-status`),
};

// Comments Admin API
export const commentsAdminApi = {
    // Article comments
    getArticleComments: (params?: { page?: number; pageSize?: number; articleId?: number; active?: boolean }) =>
        api.get('/admin/comments/articles', { params }),
    approveArticleComment: (id: number) => api.post(`/admin/comments/articles/${id}/approve`),
    rejectArticleComment: (id: number) => api.post(`/admin/comments/articles/${id}/reject`),
    deleteArticleComment: (id: number) => api.delete(`/admin/comments/articles/${id}`),
    // Product comments
    getProductComments: (params?: { page?: number; pageSize?: number; productId?: number; active?: boolean }) =>
        api.get('/admin/comments/products', { params }),
    approveProductComment: (id: number) => api.post(`/admin/comments/products/${id}/approve`),
    rejectProductComment: (id: number) => api.post(`/admin/comments/products/${id}/reject`),
    deleteProductComment: (id: number) => api.delete(`/admin/comments/products/${id}`),
    // Product reviews
    getProductReviews: (params?: { page?: number; pageSize?: number; productId?: number; active?: boolean }) =>
        api.get('/admin/reviews/products', { params }),
    approveProductReview: (id: number) => api.post(`/admin/reviews/products/${id}/approve`),
    rejectProductReview: (id: number) => api.post(`/admin/reviews/products/${id}/reject`),
    deleteProductReview: (id: number) => api.delete(`/admin/reviews/products/${id}`),
};
