import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ordersApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Card, CardContent, CardHeader } from '../../components/ui/card';
import { Eye, CheckCircle, Truck, Package } from 'lucide-react';
import { Link } from 'react-router-dom';

interface Order {
    id: number;
    orderCode?: string;
    customerName?: string;
    customerPhone?: string;
    total?: number;
    itemCount?: number;
    productOrderStatusId?: number;
    productOrderStatusName?: string;
    productOrderStatusColor?: string;
    productOrderPaymentStatusId?: number;
    productOrderPaymentStatusName?: string;
    productOrderPaymentStatusColor?: string;
    createDate?: string;
}

// Map backend status IDs to colors (fallback if color not provided)
const getStatusColor = (statusName?: string, color?: string): string => {
    if (color) return color;
    const name = statusName?.toLowerCase() || '';
    if (name.includes('pending') || name.includes('chờ')) return 'bg-yellow-100 text-yellow-800';
    if (name.includes('confirm') || name.includes('xác nhận')) return 'bg-blue-100 text-blue-800';
    if (name.includes('ship') || name.includes('giao')) return 'bg-purple-100 text-purple-800';
    if (name.includes('deliver') || name.includes('nhận')) return 'bg-green-100 text-green-800';
    if (name.includes('complete') || name.includes('hoàn')) return 'bg-green-100 text-green-800';
    if (name.includes('cancel') || name.includes('hủy')) return 'bg-red-100 text-red-800';
    return 'bg-gray-100 text-gray-800';
};

export function OrdersPage() {
    const [page, setPage] = useState(1);
    const [statusFilter, setStatusFilter] = useState<number | undefined>(undefined);
    const queryClient = useQueryClient();

    // Load order statuses for filter dropdown
    const { data: statusesData } = useQuery({
        queryKey: ['order-statuses'],
        queryFn: () => ordersApi.getStatuses(),
    });

    const { data, isLoading } = useQuery({
        queryKey: ['orders', page, statusFilter],
        queryFn: () => ordersApi.getAll({ page, pageSize: 10, productOrderStatusId: statusFilter }),
    });

    const confirmMutation = useMutation({
        mutationFn: (id: number) => ordersApi.confirm(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['orders'] }),
    });

    const shipMutation = useMutation({
        mutationFn: (id: number) => ordersApi.ship(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['orders'] }),
    });

    const deliverMutation = useMutation({
        mutationFn: (id: number) => ordersApi.deliver(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['orders'] }),
    });

    const completeMutation = useMutation({
        mutationFn: (id: number) => ordersApi.complete(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['orders'] }),
    });

    // Backend returns: { success, data: Order[], pagination: {...} }
    const orders: Order[] = data?.data?.data || [];
    const pagination = data?.data?.pagination;
    const totalPages = pagination?.totalPages || 1;
    
    const statuses = statusesData?.data?.data || [];

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-3xl font-bold tracking-tight">Đơn hàng</h1>
                <p className="text-muted-foreground">Quản lý đơn hàng khách hàng</p>
            </div>

            <Card>
                <CardHeader>
                    <div className="flex items-center gap-4">
                        <select
                            className="border rounded px-3 py-2 text-sm"
                            value={statusFilter || ''}
                            onChange={(e) => {
                                setStatusFilter(e.target.value ? Number(e.target.value) : undefined);
                                setPage(1);
                            }}
                        >
                            <option value="">Tất cả trạng thái</option>
                            {statuses.map((status: { id: number; name: string }) => (
                                <option key={status.id} value={status.id}>{status.name}</option>
                            ))}
                        </select>
                    </div>
                </CardHeader>
                <CardContent>
                    {isLoading ? (
                        <div className="flex items-center justify-center h-32">
                            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary" />
                        </div>
                    ) : orders.length === 0 ? (
                        <div className="text-center py-8 text-muted-foreground">Chưa có đơn hàng nào</div>
                    ) : (
                        <>
                            <table className="w-full">
                                <thead>
                                    <tr className="border-b">
                                        <th className="text-left py-3 px-2 font-medium">Mã đơn</th>
                                        <th className="text-left py-3 px-2 font-medium">Khách hàng</th>
                                        <th className="text-right py-3 px-2 font-medium">Tổng tiền</th>
                                        <th className="text-center py-3 px-2 font-medium">Trạng thái</th>
                                        <th className="text-center py-3 px-2 font-medium">Thanh toán</th>
                                        <th className="text-left py-3 px-2 font-medium">Ngày đặt</th>
                                        <th className="text-right py-3 px-2 font-medium">Thao tác</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {orders.map((order) => (
                                        <tr key={order.id} className="border-b hover:bg-muted/50">
                                            <td className="py-3 px-2 font-mono">{order.orderCode || `#${order.id}`}</td>
                                            <td className="py-3 px-2">
                                                <div>
                                                    <div className="font-medium">{order.customerName}</div>
                                                    <div className="text-sm text-muted-foreground">{order.customerPhone}</div>
                                                </div>
                                            </td>
                                            <td className="py-3 px-2 text-right font-medium">
                                                {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(order.total || 0)}
                                            </td>
                                            <td className="py-3 px-2 text-center">
                                                <span 
                                                    className={`inline-flex px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(order.productOrderStatusName, order.productOrderStatusColor)}`}
                                                >
                                                    {order.productOrderStatusName || '-'}
                                                </span>
                                            </td>
                                            <td className="py-3 px-2 text-center">
                                                <span 
                                                    className={`inline-flex px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(order.productOrderPaymentStatusName, order.productOrderPaymentStatusColor)}`}
                                                >
                                                    {order.productOrderPaymentStatusName || '-'}
                                                </span>
                                            </td>
                                            <td className="py-3 px-2 text-muted-foreground">
                                                {order.createDate ? new Date(order.createDate).toLocaleDateString('vi-VN') : '-'}
                                            </td>
                                            <td className="py-3 px-2">
                                                <div className="flex justify-end gap-1">
                                                    <Link to={`/orders/${order.id}`}>
                                                        <Button variant="ghost" size="icon" title="Xem chi tiết"><Eye className="h-4 w-4" /></Button>
                                                    </Link>
                                                    {/* Show action buttons based on current status */}
                                                    {order.productOrderStatusId === 1 && ( // Pending
                                                        <Button variant="ghost" size="icon" title="Xác nhận" onClick={() => confirmMutation.mutate(order.id)}>
                                                            <CheckCircle className="h-4 w-4 text-blue-500" />
                                                        </Button>
                                                    )}
                                                    {order.productOrderStatusId === 2 && ( // Confirmed
                                                        <Button variant="ghost" size="icon" title="Giao hàng" onClick={() => shipMutation.mutate(order.id)}>
                                                            <Truck className="h-4 w-4 text-purple-500" />
                                                        </Button>
                                                    )}
                                                    {order.productOrderStatusId === 3 && ( // Shipping
                                                        <Button variant="ghost" size="icon" title="Đã giao" onClick={() => deliverMutation.mutate(order.id)}>
                                                            <Package className="h-4 w-4 text-green-500" />
                                                        </Button>
                                                    )}
                                                    {order.productOrderStatusId === 4 && ( // Delivered
                                                        <Button variant="ghost" size="icon" title="Hoàn thành" onClick={() => completeMutation.mutate(order.id)}>
                                                            <CheckCircle className="h-4 w-4 text-green-600" />
                                                        </Button>
                                                    )}
                                                </div>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                            <div className="flex items-center justify-between mt-4">
                                <span className="text-sm text-muted-foreground">
                                    Trang {page} / {totalPages}
                                    {pagination?.totalCount && ` (${pagination.totalCount} đơn hàng)`}
                                </span>
                                <div className="flex gap-2">
                                    <Button variant="outline" size="sm" onClick={() => setPage(p => Math.max(1, p - 1))} disabled={page === 1}>Trước</Button>
                                    <Button variant="outline" size="sm" onClick={() => setPage(p => Math.min(totalPages, p + 1))} disabled={page === totalPages}>Sau</Button>
                                </div>
                            </div>
                        </>
                    )}
                </CardContent>
            </Card>
        </div>
    );
}
