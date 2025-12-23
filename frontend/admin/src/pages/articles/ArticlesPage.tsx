import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { articlesApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Card, CardContent, CardHeader } from '../../components/ui/card';
import { Plus, Search, Edit, Trash2, Eye } from 'lucide-react';
import { Link } from 'react-router-dom';

export function ArticlesPage() {
    const [search, setSearch] = useState('');
    const [page, setPage] = useState(1);
    const queryClient = useQueryClient();

    const { data, isLoading } = useQuery({
        queryKey: ['articles', page, search],
        queryFn: () => articlesApi.getAll({ page, pageSize: 10, search }),
    });

    const deleteMutation = useMutation({
        mutationFn: (id: number) => articlesApi.delete(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['articles'] }),
    });

    const articles = data?.data?.data?.items || [];
    const totalPages = data?.data?.data?.totalPages || 1;

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight">Bài viết</h1>
                    <p className="text-muted-foreground">Quản lý tin tức và bài viết</p>
                </div>
                <Link to="/articles/new">
                    <Button>
                        <Plus className="h-4 w-4 mr-2" />
                        Thêm bài viết
                    </Button>
                </Link>
            </div>

            <Card>
                <CardHeader>
                    <div className="relative max-w-sm">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                        <Input
                            placeholder="Tìm kiếm bài viết..."
                            value={search}
                            onChange={(e) => setSearch(e.target.value)}
                            className="pl-9"
                        />
                    </div>
                </CardHeader>
                <CardContent>
                    {isLoading ? (
                        <div className="flex items-center justify-center h-32">
                            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary" />
                        </div>
                    ) : articles.length === 0 ? (
                        <div className="text-center py-8 text-muted-foreground">
                            Không tìm thấy bài viết nào
                        </div>
                    ) : (
                        <>
                            <table className="w-full">
                                <thead>
                                    <tr className="border-b">
                                        <th className="text-left py-3 px-2 font-medium">Tiêu đề</th>
                                        <th className="text-left py-3 px-2 font-medium">Ngày tạo</th>
                                        <th className="text-center py-3 px-2 font-medium">Trạng thái</th>
                                        <th className="text-right py-3 px-2 font-medium">Thao tác</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {articles.map((article: any) => (
                                        <tr key={article.id} className="border-b hover:bg-muted/50">
                                            <td className="py-3 px-2 font-medium">{article.name}</td>
                                            <td className="py-3 px-2 text-muted-foreground">
                                                {new Date(article.createdDate).toLocaleDateString('vi-VN')}
                                            </td>
                                            <td className="py-3 px-2 text-center">
                                                <span className={`inline-flex px-2 py-1 rounded-full text-xs font-medium ${article.isPublished ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'
                                                    }`}>
                                                    {article.isPublished ? 'Đã xuất bản' : 'Nháp'}
                                                </span>
                                            </td>
                                            <td className="py-3 px-2">
                                                <div className="flex justify-end gap-2">
                                                    <Link to={`/articles/${article.id}`}>
                                                        <Button variant="ghost" size="icon"><Eye className="h-4 w-4" /></Button>
                                                    </Link>
                                                    <Link to={`/articles/${article.id}/edit`}>
                                                        <Button variant="ghost" size="icon"><Edit className="h-4 w-4" /></Button>
                                                    </Link>
                                                    <Button variant="ghost" size="icon" onClick={() => {
                                                        if (confirm('Xóa bài viết này?')) deleteMutation.mutate(article.id);
                                                    }}>
                                                        <Trash2 className="h-4 w-4 text-destructive" />
                                                    </Button>
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
