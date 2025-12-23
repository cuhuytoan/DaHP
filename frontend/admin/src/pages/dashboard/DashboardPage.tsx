import { useQuery } from '@tanstack/react-query';
import { dashboardApi } from '../../api/client';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import { Package, FileText, ShoppingCart, TrendingUp } from 'lucide-react';

export function DashboardPage() {
    const { data: summary, isLoading } = useQuery({
        queryKey: ['dashboard-summary'],
        queryFn: () => dashboardApi.getSummary(),
    });

    const stats = summary?.data?.data || {
        totalProducts: 0,
        totalArticles: 0,
        totalOrders: 0,
        todayRevenue: 0,
        pendingOrders: 0,
        pendingProducts: 0,
        pendingArticles: 0,
    };

    const cards = [
        {
            title: 'Tổng sản phẩm',
            value: stats.totalProducts || 0,
            icon: Package,
            color: 'text-blue-500',
            bgColor: 'bg-blue-500/10',
        },
        {
            title: 'Tổng bài viết',
            value: stats.totalArticles || 0,
            icon: FileText,
            color: 'text-green-500',
            bgColor: 'bg-green-500/10',
        },
        {
            title: 'Đơn hàng',
            value: stats.totalOrders || 0,
            icon: ShoppingCart,
            color: 'text-purple-500',
            bgColor: 'bg-purple-500/10',
        },
        {
            title: 'Doanh thu hôm nay',
            value: new Intl.NumberFormat('vi-VN', {
                style: 'currency',
                currency: 'VND',
            }).format(stats.todayRevenue || 0),
            icon: TrendingUp,
            color: 'text-orange-500',
            bgColor: 'bg-orange-500/10',
        },
    ];

    if (isLoading) {
        return (
            <div className="flex items-center justify-center h-64">
                <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary" />
            </div>
        );
    }

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
                <p className="text-muted-foreground">Tổng quan hệ thống quản trị</p>
            </div>

            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                {cards.map((card) => (
                    <Card key={card.title}>
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="text-sm font-medium">{card.title}</CardTitle>
                            <div className={`p-2 rounded-lg ${card.bgColor}`}>
                                <card.icon className={`h-4 w-4 ${card.color}`} />
                            </div>
                        </CardHeader>
                        <CardContent>
                            <div className="text-2xl font-bold">{card.value}</div>
                        </CardContent>
                    </Card>
                ))}
            </div>

            <div className="grid gap-4 md:grid-cols-2">
                <Card>
                    <CardHeader>
                        <CardTitle>Đơn hàng chờ xử lý</CardTitle>
                    </CardHeader>
                    <CardContent>
                        <div className="text-3xl font-bold text-orange-500">
                            {stats.pendingOrders || 0}
                        </div>
                    </CardContent>
                </Card>

                <Card>
                    <CardHeader>
                        <CardTitle>Sản phẩm chờ duyệt</CardTitle>
                    </CardHeader>
                    <CardContent>
                        <div className="text-3xl font-bold text-blue-500">
                            {stats.pendingProducts || 0}
                        </div>
                    </CardContent>
                </Card>
            </div>
        </div>
    );
}
