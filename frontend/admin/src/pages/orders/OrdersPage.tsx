import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ordersApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Card, CardContent, CardHeader } from '../../components/ui/card';
import { Eye, CheckCircle, Truck, Package, XCircle } from 'lucide-react';
import { Link } from 'react-router-dom';

const statusColors: Record<string, string> = {
    pending: 'bg-yellow-100 text-yellow-800',
    confirmed: 'bg-blue-100 text-blue-800',
    shipping: 'bg-purple-100 text-purple-800',
    delivered: 'bg-green-100 text-green-800',
    completed: 'bg-green-100 text-green-800',
    cancelled: 'bg-red-100 text-red-800',
};

export function OrdersPage() {
    const [page, setPage] = useState(1);
    const queryClient = useQueryClient();

    const { data, isLoading } = useQuery({
        queryKey: ['orders', page],
        queryFn: () => ordersApi.getAll({ page, pageSize: 10 }),
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

    const orders = data?.data?.data?.items || [];
    const totalPages = data?.data?.data?.totalPages || 1;

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-3xl font-bold tracking-tight">Đơn hàng</h1>
                <p className="text-muted-foreground">Quản lý đơn hàng khách hàng</p>
            </div>

            <Card>
                <CardContent className="pt-6">
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
                                        <th className="text-left py-3 px-2 font-medium">Ngày đặt</th>
                                        <th className="text-right py-3 px-2 font-medium">Thao tác</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {orders.map((order: any) => (
                                        <tr key={order.id} className="border-b hover:bg-muted/50">
                                            <td className="py-3 px-2 font-mono">{order.orderCode}</td>
                                            <td className="py-3 px-2">{order.customerName}</td>
                                            <td className="py-3 px-2 text-right font-medium">
                                                {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(order.totalAmount || 0)}
                                            </td>
                                            <td className="py-3 px-2 text-center">
                                                <span className={`inline-flex px-2 py-1 rounded-full text-xs font-medium ${statusColors[order.status?.toLowerCase()] || 'bg-gray-100'}`}>
                                                    {order.statusName || order.status}
                                                </span>
                                            </td>
                                            <td className="py-3 px-2 text-muted-foreground">
                                                {new Date(order.createdDate).toLocaleDateString('vi-VN')}
                                            </td>
                                            <td className="py-3 px-2">
                                                <div className="flex justify-end gap-1">
                                                    <Link to={`/orders/${order.id}`}>
                                                        <Button variant="ghost" size="icon" title="Xem chi tiết"><Eye className="h-4 w-4" /></Button>
                                                    </Link>
                                                    {order.status?.toLowerCase() === 'pending' && (
                                                        <Button variant="ghost" size="icon" title="Xác nhận" onClick={() => confirmMutation.mutate(order.id)}>
                                                            <CheckCircle className="h-4 w-4 text-blue-500" />
                                                        </Button>
                                                    )}
                                                    {order.status?.toLowerCase() === 'confirmed' && (
                                                        <Button variant="ghost" size="icon" title="Giao hàng" onClick={() => shipMutation.mutate(order.id)}>
                                                            <Truck className="h-4 w-4 text-purple-500" />
                                                        </Button>
                                                    )}
                                                    {order.status?.toLowerCase() === 'shipping' && (
                                                        <Button variant="ghost" size="icon" title="Đã giao" onClick={() => deliverMutation.mutate(order.id)}>
                                                            <Package className="h-4 w-4 text-green-500" />
                                                        </Button>
                                                    )}
                                                </div>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                            <div className="flex items-center justify-between mt-4">
                                <span className="text-sm text-muted-foreground">Trang {page} / {totalPages}</span>
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
