// Re-export types from API for convenience
export type {
  Product,
  Article,
  Category,
  Brand,
  CartItem,
  Pagination,
  ApiResponse,
  ApiListResponse,
} from "@/lib/api";

export interface User {
  id: string;
  email: string;
  fullName: string;
  phoneNumber?: string;
  avatar?: string;
  address?: string;
}

export interface Order {
  id: number;
  orderCode: string;
  userId?: string;
  items: OrderItem[];
  total: number;
  status: OrderStatus;
  statusName?: string;
  shippingAddress: string;
  customerPhone: string;
  customerName: string;
  customerEmail?: string;
  note?: string;
  createDate: string;
}

export interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  productImage?: string;
  productCode?: string;
  price: number;
  quantity: number;
  total: number;
}

export type OrderStatus =
  | "pending"
  | "confirmed"
  | "processing"
  | "shipping"
  | "delivered"
  | "completed"
  | "cancelled";
