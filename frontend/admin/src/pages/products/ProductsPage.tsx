import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { productsApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import { Plus, Search, Edit, Trash2, CheckCircle, Eye } from 'lucide-react';
import { Link } from 'react-router-dom';

export function ProductsPage() {
    const [search, setSearch] = useState('');
    const [page, setPage] = useState(1);
    const queryClient = useQueryClient();

    const { data, isLoading } = useQuery({
        queryKey: ['products', page, search],
        queryFn: () => productsApi.getAll({ page, pageSize: 10, search }),
    });

    const deleteMutation = useMutation({
        mutationFn: (id: number) => productsApi.delete(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] });
        },
    });

    const products = data?.data?.data?.items || [];
    const totalPages = data?.data?.data?.totalPages || 1;

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight">Sản phẩm</h1>
                    <p className="text-muted-foreground">Quản lý sản phẩm trong hệ thống</p>
                </div>
                <Link to="/products/new">
                    <Button>
                        <Plus className="h-4 w-4 mr-2" />
                        Thêm sản phẩm
                    </Button>
                </Link>
            </div>

            <Card>
                <CardHeader>
                    <div className="flex items-center gap-4">
                        <div className="relative flex-1 max-w-sm">
                            <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                            <Input
                                placeholder="Tìm kiếm sản phẩm..."
                                value={search}
                                onChange={(e) => setSearch(e.target.value)}
                                className="pl-9"
                            />
                        </div>
                    </div>
                </CardHeader>
                <CardContent>
                    {isLoading ? (
                        <div className="flex items-center justify-center h-32">
                            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary" />
                        </div>
                    ) : products.length === 0 ? (
                        <div className="text-center py-8 text-muted-foreground">
                            Không tìm thấy sản phẩm nào
                        </div>
                    ) : (
                        <>
                            <div className="overflow-x-auto">
                                <table className="w-full">
                                    <thead>
                                        <tr className="border-b">
                                            <th className="text-left py-3 px-2 font-medium">Tên sản phẩm</th>
                                            <th className="text-left py-3 px-2 font-medium">Mã</th>
                                            <th className="text-right py-3 px-2 font-medium">Giá</th>
                                            <th className="text-center py-3 px-2 font-medium">Trạng thái</th>
                                            <th className="text-right py-3 px-2 font-medium">Thao tác</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {products.map((product: any) => (
                                            <tr key={product.id} className="border-b hover:bg-muted/50">
                                                <td className="py-3 px-2">
                                                    <div className="flex items-center gap-3">
                                                        {product.image && (
                                                            <img
                                                                src={product.image}
                                                                alt={product.name}
                                                                className="h-10 w-10 rounded object-cover"
                                                            />
                                                        )}
                                                        <span className="font-medium">{product.name}</span>
                                                    </div>
                                                </td>
                                                <td className="py-3 px-2 text-muted-foreground">{product.code}</td>
                                                <td className="py-3 px-2 text-right">
                                                    {new Intl.NumberFormat('vi-VN', {
                                                        style: 'currency',
                                                        currency: 'VND',
                                                    }).format(product.price || 0)}
                                                </td>
                                                <td className="py-3 px-2 text-center">
                                                    <span
                                                        className={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium ${product.isPublished
                                                                ? 'bg-green-100 text-green-800'
                                                                : 'bg-yellow-100 text-yellow-800'
                                                            }`}
                                                    >
                                                        {product.isPublished ? 'Đã xuất bản' : 'Nháp'}
                                                    </span>
                                                </td>
                                                <td className="py-3 px-2">
                                                    <div className="flex items-center justify-end gap-2">
                                                        <Link to={`/products/${product.id}`}>
                                                            <Button variant="ghost" size="icon">
                                                                <Eye className="h-4 w-4" />
                                                            </Button>
                                                        </Link>
                                                        <Link to={`/products/${product.id}/edit`}>
                                                            <Button variant="ghost" size="icon">
                                                                <Edit className="h-4 w-4" />
                                                            </Button>
                                                        </Link>
                                                        <Button
                                                            variant="ghost"
                                                            size="icon"
                                                            onClick={() => {
                                                                if (confirm('Bạn có chắc muốn xóa sản phẩm này?')) {
                                                                    deleteMutation.mutate(product.id);
                                                                }
                                                            }}
                                                        >
                                                            <Trash2 className="h-4 w-4 text-destructive" />
                                                        </Button>
                                                    </div>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>

                            {/* Pagination */}
                            <div className="flex items-center justify-between mt-4">
                                <span className="text-sm text-muted-foreground">
                                    Trang {page} / {totalPages}
                                </span>
                                <div className="flex gap-2">
                                    <Button
                                        variant="outline"
                                        size="sm"
                                        onClick={() => setPage((p) => Math.max(1, p - 1))}
                                        disabled={page === 1}
                                    >
                                        Trước
                                    </Button>
                                    <Button
                                        variant="outline"
                                        size="sm"
                                        onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
                                        disabled={page === totalPages}
                                    >
                                        Sau
                                    </Button>
                                </div>
                            </div>
                        </>
                    )}
                </CardContent>
            </Card>
        </div>
    );
}
