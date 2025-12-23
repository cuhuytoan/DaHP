"use client";

import { useEffect } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { ArrowLeft, Package, Eye } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { useAuth } from "@/hooks/use-auth";
import { formatPrice, formatDate } from "@/lib/utils";

// Mock orders data
const mockOrders = [
  {
    id: 1,
    code: "DH20241219001",
    status: "delivered",
    statusText: "Đã giao hàng",
    totalAmount: 2500000,
    itemCount: 2,
    createdAt: "2024-12-15",
  },
  {
    id: 2,
    code: "DH20241219002",
    status: "shipping",
    statusText: "Đang giao hàng",
    totalAmount: 1500000,
    itemCount: 1,
    createdAt: "2024-12-17",
  },
  {
    id: 3,
    code: "DH20241219003",
    status: "pending",
    statusText: "Chờ xác nhận",
    totalAmount: 3500000,
    itemCount: 3,
    createdAt: "2024-12-19",
  },
];

const statusVariant: Record<string, "default" | "success" | "warning" | "secondary"> = {
  pending: "warning",
  confirmed: "secondary",
  shipping: "default",
  delivered: "success",
  cancelled: "destructive" as "default",
};

export default function OrdersPage() {
  const router = useRouter();
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push("/dang-nhap?redirect=/tai-khoan/don-hang");
    }
  }, [isAuthenticated, router]);

  if (!isAuthenticated) {
    return null;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Header */}
      <div className="flex items-center gap-4 mb-8">
        <Link href="/tai-khoan">
          <Button variant="ghost" size="icon">
            <ArrowLeft className="h-4 w-4" />
          </Button>
        </Link>
        <h1 className="text-3xl font-bold text-gray-900">Đơn hàng của tôi</h1>
      </div>

      {/* Orders list */}
      {mockOrders.length > 0 ? (
        <div className="space-y-4">
          {mockOrders.map((order) => (
            <div
              key={order.id}
              className="bg-white rounded-lg border p-4 hover:shadow-md transition-shadow"
            >
              <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div className="flex items-start gap-4">
                  <div className="w-12 h-12 bg-gray-100 rounded-lg flex items-center justify-center">
                    <Package className="h-6 w-6 text-gray-500" />
                  </div>
                  <div>
                    <div className="flex items-center gap-2 mb-1">
                      <span className="font-medium">{order.code}</span>
                      <Badge variant={statusVariant[order.status]}>
                        {order.statusText}
                      </Badge>
                    </div>
                    <p className="text-sm text-gray-500">
                      {order.itemCount} sản phẩm • {formatDate(order.createdAt)}
                    </p>
                  </div>
                </div>

                <div className="flex items-center justify-between md:justify-end gap-6">
                  <div className="text-right">
                    <p className="text-sm text-gray-500">Tổng tiền</p>
                    <p className="font-bold text-primary">
                      {formatPrice(order.totalAmount)}
                    </p>
                  </div>
                  <Link href={`/tai-khoan/don-hang/${order.id}`}>
                    <Button variant="outline" size="sm">
                      <Eye className="h-4 w-4 mr-2" />
                      Chi tiết
                    </Button>
                  </Link>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="text-center py-12">
          <div className="w-20 h-20 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Package className="h-10 w-10 text-gray-400" />
          </div>
          <h2 className="text-xl font-semibold mb-2">Chưa có đơn hàng nào</h2>
          <p className="text-gray-500 mb-6">
            Bạn chưa có đơn hàng nào. Hãy khám phá các sản phẩm của chúng tôi!
          </p>
          <Link href="/san-pham">
            <Button>Mua sắm ngay</Button>
          </Link>
        </div>
      )}
    </div>
  );
}
