const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000/api";

type RequestOptions = {
  method?: "GET" | "POST" | "PUT" | "DELETE";
  body?: unknown;
  headers?: Record<string, string>;
  cache?: RequestCache;
  next?: { revalidate?: number | false; tags?: string[] };
};

async function fetchApi<T>(
  endpoint: string,
  options: RequestOptions = {}
): Promise<T> {
  const { method = "GET", body, headers = {}, cache, next } = options;

  const config: RequestInit & { next?: { revalidate?: number | false; tags?: string[] } } = {
    method,
    headers: {
      "Content-Type": "application/json",
      ...headers,
    },
    cache,
    next,
  };

  if (body) {
    config.body = JSON.stringify(body);
  }

  const res = await fetch(`${API_URL}${endpoint}`, config);

  if (!res.ok) {
    throw new Error(`API Error: ${res.status} ${res.statusText}`);
  }

  return res.json();
}

// Types matching backend DTOs
export interface Product {
  id: number;
  code?: string;
  name: string;
  subTitle?: string;
  image?: string;
  description?: string;
  price: number;
  priceOld?: number;
  quantity?: number;
  quantitySold?: number;
  url?: string;
  productStatusId?: number;
  productStatusName?: string;
  productBrandName?: string;
  counter?: number;
  active?: boolean;
  averageRating?: number;
  totalReview?: number;
  createDate?: string;
  // For detail view
  content?: string;
  images?: string[];
  categoryId?: number;
  categoryName?: string;
  brandId?: number;
  brandName?: string;
  tags?: string;
  metaTitle?: string;
  metaDescription?: string;
  metaKeywords?: string;
}

export interface Article {
  id: number;
  name: string;
  subTitle?: string;
  description?: string;
  content?: string;
  image?: string;
  author?: string;
  url?: string;
  articleStatusId?: number;
  articleStatusName?: string;
  counter?: number;
  active?: boolean;
  createDate?: string;
  startDate?: string;
}

export interface Category {
  id: number;
  name: string;
  description?: string;
  parentId?: number;
  image?: string;
  url?: string;
  productCount?: number;
  active?: boolean;
}

export interface Brand {
  id: number;
  name: string;
  code?: string;
  tradingName?: string;
  description?: string;
  image?: string;
  url?: string;
  active?: boolean;
}

export interface Pagination {
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalItems?: number;
  totalPages: number;
  hasPrevious?: boolean;
  hasNext?: boolean;
}

export interface ApiListResponse<T> {
  success: boolean;
  data: T[];
  message: string | null;
  errors: string[];
  pagination: Pagination;
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string | null;
  errors: string[];
}

// Product search params matching backend ProductSearchRequest
export interface ProductSearchParams {
  keyword?: string;
  productTypeId?: number;
  productCategoryId?: number;
  productStatusId?: number;
  productBrandId?: number;
  productManufactureId?: number;
  minPrice?: number;
  maxPrice?: number;
  active?: boolean;
  inStock?: boolean;
  fromDate?: string;
  toDate?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDescending?: boolean;
}

// Products API
export const productsApi = {
  getAll: (params?: ProductSearchParams) => {
    const searchParams = new URLSearchParams();
    if (params?.page) searchParams.set("Page", params.page.toString());
    if (params?.pageSize) searchParams.set("PageSize", params.pageSize.toString());
    if (params?.productCategoryId) searchParams.set("ProductCategoryId", params.productCategoryId.toString());
    if (params?.productBrandId) searchParams.set("ProductBrandId", params.productBrandId.toString());
    if (params?.keyword) searchParams.set("Keyword", params.keyword);
    if (params?.minPrice) searchParams.set("MinPrice", params.minPrice.toString());
    if (params?.maxPrice) searchParams.set("MaxPrice", params.maxPrice.toString());
    if (params?.sortBy) searchParams.set("SortBy", params.sortBy);
    if (params?.sortDescending !== undefined) searchParams.set("SortDescending", params.sortDescending.toString());
    if (params?.active !== undefined) searchParams.set("Active", params.active.toString());

    return fetchApi<ApiListResponse<Product>>(
      `/products?${searchParams.toString()}`,
      { next: { revalidate: 60, tags: ["products"] } }
    );
  },

  getById: (id: number) =>
    fetchApi<ApiResponse<Product>>(`/products/${id}`, {
      next: { revalidate: 60, tags: [`product-${id}`] },
    }),

  getByUrl: (url: string) =>
    fetchApi<ApiResponse<Product>>(`/products/url/${url}`, {
      next: { revalidate: 60, tags: [`product-url-${url}`] },
    }),

  getFeatured: (count = 8) =>
    fetchApi<ApiListResponse<Product>>(`/products?PageSize=${count}&Active=true`, {
      next: { revalidate: 300, tags: ["featured-products"] },
    }),

  getByCategory: (categoryId: number, params?: { page?: number; pageSize?: number }) => {
    const searchParams = new URLSearchParams();
    searchParams.set("ProductCategoryId", categoryId.toString());
    if (params?.page) searchParams.set("Page", params.page.toString());
    if (params?.pageSize) searchParams.set("PageSize", params.pageSize.toString());

    return fetchApi<ApiListResponse<Product>>(
      `/products?${searchParams.toString()}`,
      { next: { revalidate: 60, tags: ["products", `category-${categoryId}`] } }
    );
  },

  getBestSellers: (count = 10) =>
    fetchApi<ApiListResponse<Product>>(`/products?PageSize=${count}&SortBy=QuantitySold&SortDescending=true`, {
      next: { revalidate: 300, tags: ["best-sellers"] },
    }),

  getRelated: (productId: number, count = 5) =>
    fetchApi<ApiListResponse<Product>>(`/products/${productId}/related?count=${count}`, {
      next: { revalidate: 300, tags: [`related-${productId}`] },
    }),
};

// Articles API
export const articlesApi = {
  getAll: (params?: { page?: number; pageSize?: number; typeId?: number; keyword?: string }) => {
    const searchParams = new URLSearchParams();
    if (params?.page) searchParams.set("Page", params.page.toString());
    if (params?.pageSize) searchParams.set("PageSize", params.pageSize.toString());
    if (params?.typeId) searchParams.set("ArticleTypeId", params.typeId.toString());
    if (params?.keyword) searchParams.set("Keyword", params.keyword);

    return fetchApi<ApiListResponse<Article>>(
      `/articles?${searchParams.toString()}`,
      { next: { revalidate: 60, tags: ["articles"] } }
    );
  },

  getById: (id: number) =>
    fetchApi<ApiResponse<Article>>(`/articles/${id}`, {
      next: { revalidate: 60, tags: [`article-${id}`] },
    }),

  getByUrl: (url: string) =>
    fetchApi<ApiResponse<Article>>(`/articles/url/${url}`, {
      next: { revalidate: 60, tags: [`article-url-${url}`] },
    }),

  getLatest: (count = 6) =>
    fetchApi<ApiListResponse<Article>>(`/articles?PageSize=${count}&SortBy=CreateDate&SortDescending=true`, {
      next: { revalidate: 300, tags: ["latest-articles"] },
    }),
};

// Categories API
export const categoriesApi = {
  getAll: () =>
    fetchApi<ApiListResponse<Category>>("/masterdata/product-categories", {
      next: { revalidate: 3600, tags: ["categories"] },
    }),

  getById: (id: number) =>
    fetchApi<ApiResponse<Category>>(`/masterdata/product-categories/${id}`, {
      next: { revalidate: 3600, tags: [`category-${id}`] },
    }),
};

// Brands API
export const brandsApi = {
  getAll: () =>
    fetchApi<ApiListResponse<Brand>>("/brands", {
      next: { revalidate: 3600, tags: ["brands"] },
    }),

  getById: (id: number) =>
    fetchApi<ApiResponse<Brand>>(`/brands/${id}`, {
      next: { revalidate: 3600, tags: [`brand-${id}`] },
    }),
};

// Cart Item type
export interface CartItem {
  productId: number;
  productName: string;
  productImage?: string;
  productCode?: string;
  price: number;
  quantity: number;
}

// Orders API (client-side with auth)
export const ordersApi = {
  create: async (data: {
    customerName: string;
    customerPhone: string;
    customerEmail?: string;
    shippingAddress: string;
    note?: string;
    items: { productId: number; quantity: number; price: number }[];
  }, token?: string) => {
    const headers: Record<string, string> = {};
    if (token) headers.Authorization = `Bearer ${token}`;

    return fetchApi<ApiResponse<{ id: number; orderCode: string }>>("/orders", {
      method: "POST",
      body: data,
      headers,
      cache: "no-store",
    });
  },

  getMyOrders: async (token: string) => {
    return fetchApi<ApiListResponse<unknown>>("/orders/my", {
      headers: { Authorization: `Bearer ${token}` },
      cache: "no-store",
    });
  },

  getByCode: async (orderCode: string, token?: string) => {
    const headers: Record<string, string> = {};
    if (token) headers.Authorization = `Bearer ${token}`;

    return fetchApi<ApiResponse<unknown>>(`/orders/code/${orderCode}`, {
      headers,
      cache: "no-store",
    });
  },
};

// Auth API
export const authApi = {
  login: (data: { email: string; password: string }) =>
    fetchApi<ApiResponse<{ token: string; refreshToken: string; user: { id: string; email: string; fullName: string } }>>(
      "/auth/login",
      { method: "POST", body: data, cache: "no-store" }
    ),

  register: (data: { email: string; password: string; fullName: string; phoneNumber?: string }) =>
    fetchApi<ApiResponse<{ token: string; refreshToken: string; user: { id: string; email: string; fullName: string } }>>(
      "/auth/register",
      { method: "POST", body: data, cache: "no-store" }
    ),

  me: (token: string) =>
    fetchApi<ApiResponse<{ id: string; email: string; fullName: string; phoneNumber?: string; avatar?: string }>>(
      "/auth/me",
      { headers: { Authorization: `Bearer ${token}` }, cache: "no-store" }
    ),

  refreshToken: (refreshToken: string) =>
    fetchApi<ApiResponse<{ token: string; refreshToken: string }>>(
      "/auth/refresh-token",
      { method: "POST", body: { refreshToken }, cache: "no-store" }
    ),
};
