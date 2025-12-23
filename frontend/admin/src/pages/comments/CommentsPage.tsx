import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { commentsAdminApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import {
    Check,
    X,
    Trash2,
    Loader2,
    MessageSquare,
    FileText,
    Package,
    Star,
} from 'lucide-react';

interface Comment {
    id: number;
    articleId?: number;
    productId?: number;
    articleName?: string;
    productName?: string;
    parentId?: number;
    userId?: string;
    userName?: string;
    name?: string;
    email?: string;
    phone?: string;
    content: string;
    active?: boolean;
    createDate?: string;
    rating?: number; // For reviews
}

type TabType = 'article-comments' | 'product-comments' | 'product-reviews';

export function CommentsPage() {
    const queryClient = useQueryClient();
    const [activeTab, setActiveTab] = useState<TabType>('article-comments');
    const [page, setPage] = useState(1);

    // Article comments query
    const { data: articleCommentsData, isLoading: articleCommentsLoading } = useQuery({
        queryKey: ['article-comments', page],
        queryFn: () => commentsAdminApi.getArticleComments({ page, pageSize: 20 }),
        enabled: activeTab === 'article-comments',
    });

    // Product comments query
    const { data: productCommentsData, isLoading: productCommentsLoading } = useQuery({
        queryKey: ['product-comments', page],
        queryFn: () => commentsAdminApi.getProductComments({ page, pageSize: 20 }),
        enabled: activeTab === 'product-comments',
    });

    // Product reviews query
    const { data: productReviewsData, isLoading: productReviewsLoading } = useQuery({
        queryKey: ['product-reviews', page],
        queryFn: () => commentsAdminApi.getProductReviews({ page, pageSize: 20 }),
        enabled: activeTab === 'product-reviews',
    });

    // Article comment mutations
    const approveArticleCommentMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.approveArticleComment(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['article-comments'] }),
    });

    const rejectArticleCommentMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.rejectArticleComment(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['article-comments'] }),
    });

    const deleteArticleCommentMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.deleteArticleComment(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['article-comments'] }),
    });

    // Product comment mutations
    const approveProductCommentMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.approveProductComment(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['product-comments'] }),
    });

    const rejectProductCommentMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.rejectProductComment(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['product-comments'] }),
    });

    const deleteProductCommentMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.deleteProductComment(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['product-comments'] }),
    });

    // Product review mutations
    const approveProductReviewMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.approveProductReview(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['product-reviews'] }),
    });

    const rejectProductReviewMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.rejectProductReview(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['product-reviews'] }),
    });

    const deleteProductReviewMutation = useMutation({
        mutationFn: (id: number) => commentsAdminApi.deleteProductReview(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['product-reviews'] }),
    });

    const getCurrentData = () => {
        switch (activeTab) {
            case 'article-comments':
                return {
                    items: articleCommentsData?.data?.data || [],
                    pagination: articleCommentsData?.data?.pagination,
                    isLoading: articleCommentsLoading,
                };
            case 'product-comments':
                return {
                    items: productCommentsData?.data?.data || [],
                    pagination: productCommentsData?.data?.pagination,
                    isLoading: productCommentsLoading,
                };
            case 'product-reviews':
                return {
                    items: productReviewsData?.data?.data || [],
                    pagination: productReviewsData?.data?.pagination,
                    isLoading: productReviewsLoading,
                };
        }
    };

    const { items, pagination, isLoading } = getCurrentData();
    const totalPages = pagination?.totalPages || 1;

    const handleApprove = (id: number) => {
        switch (activeTab) {
            case 'article-comments':
                approveArticleCommentMutation.mutate(id);
                break;
            case 'product-comments':
                approveProductCommentMutation.mutate(id);
                break;
            case 'product-reviews':
                approveProductReviewMutation.mutate(id);
                break;
        }
    };

    const handleReject = (id: number) => {
        switch (activeTab) {
            case 'article-comments':
                rejectArticleCommentMutation.mutate(id);
                break;
            case 'product-comments':
                rejectProductCommentMutation.mutate(id);
                break;
            case 'product-reviews':
                rejectProductReviewMutation.mutate(id);
                break;
        }
    };

    const handleDelete = (id: number) => {
        if (!confirm('Bạn có chắc muốn xóa?')) return;
        switch (activeTab) {
            case 'article-comments':
                deleteArticleCommentMutation.mutate(id);
                break;
            case 'product-comments':
                deleteProductCommentMutation.mutate(id);
                break;
            case 'product-reviews':
                deleteProductReviewMutation.mutate(id);
                break;
        }
    };

    const tabs = [
        { id: 'article-comments' as TabType, name: 'Bình luận bài viết', icon: FileText },
        { id: 'product-comments' as TabType, name: 'Bình luận sản phẩm', icon: Package },
        { id: 'product-reviews' as TabType, name: 'Đánh giá sản phẩm', icon: Star },
    ];

    const renderRating = (rating?: number) => {
        if (!rating) return null;
        return (
            <div className="flex items-center gap-1">
                {[1, 2, 3, 4, 5].map((star) => (
                    <Star
                        key={star}
                        className={`h-4 w-4 ${
                            star <= rating ? 'fill-yellow-400 text-yellow-400' : 'text-gray-300'
                        }`}
                    />
                ))}
            </div>
        );
    };

    return (
        <div className="space-y-6">
            <div>
                <h1 className="text-3xl font-bold tracking-tight flex items-center gap-2">
                    <MessageSquare className="h-8 w-8" />
                    Quản lý bình luận
                </h1>
                <p className="text-muted-foreground">Duyệt và quản lý bình luận, đánh giá</p>
            </div>

            {/* Tabs */}
            <div className="flex gap-2 border-b pb-2">
                {tabs.map((tab) => (
                    <Button
                        key={tab.id}
                        variant={activeTab === tab.id ? 'default' : 'ghost'}
                        onClick={() => {
                            setActiveTab(tab.id);
                            setPage(1);
                        }}
                        className="gap-2"
                    >
                        <tab.icon className="h-4 w-4" />
                        {tab.name}
                    </Button>
                ))}
            </div>

            <Card>
                <CardHeader>
                    <CardTitle>
                        {tabs.find((t) => t.id === activeTab)?.name}
                    </CardTitle>
                </CardHeader>
                <CardContent>
                    {isLoading ? (
                        <div className="flex items-center justify-center py-8">
                            <Loader2 className="h-8 w-8 animate-spin text-primary" />
                        </div>
                    ) : items.length === 0 ? (
                        <div className="text-center py-8 text-muted-foreground">
                            Không có dữ liệu
                        </div>
                    ) : (
                        <>
                            <div className="space-y-4">
                                {items.map((item: Comment) => (
                                    <div
                                        key={item.id}
                                        className={`p-4 border rounded-lg ${
                                            item.active === false
                                                ? 'bg-destructive/5 border-destructive/20'
                                                : item.active === true
                                                ? 'bg-green-50 border-green-200'
                                                : 'bg-yellow-50 border-yellow-200'
                                        }`}
                                    >
                                        <div className="flex items-start justify-between gap-4">
                                            <div className="flex-1 min-w-0">
                                                <div className="flex items-center gap-2 mb-1">
                                                    <span className="font-medium">
                                                        {item.userName || item.name || 'Ẩn danh'}
                                                    </span>
                                                    {item.email && (
                                                        <span className="text-sm text-muted-foreground">
                                                            ({item.email})
                                                        </span>
                                                    )}
                                                    {activeTab === 'product-reviews' && renderRating(item.rating)}
                                                </div>

                                                <p className="text-sm text-muted-foreground mb-2">
                                                    {activeTab === 'article-comments' && item.articleName && (
                                                        <>Bài viết: {item.articleName}</>
                                                    )}
                                                    {(activeTab === 'product-comments' ||
                                                        activeTab === 'product-reviews') &&
                                                        item.productName && <>Sản phẩm: {item.productName}</>}
                                                </p>

                                                <p className="whitespace-pre-wrap">{item.content}</p>

                                                <div className="flex items-center gap-4 mt-2 text-xs text-muted-foreground">
                                                    <span>
                                                        {item.createDate &&
                                                            new Date(item.createDate).toLocaleString('vi-VN')}
                                                    </span>
                                                    <span
                                                        className={`px-2 py-0.5 rounded ${
                                                            item.active === false
                                                                ? 'bg-destructive/10 text-destructive'
                                                                : item.active === true
                                                                ? 'bg-green-100 text-green-800'
                                                                : 'bg-yellow-100 text-yellow-800'
                                                        }`}
                                                    >
                                                        {item.active === false
                                                            ? 'Từ chối'
                                                            : item.active === true
                                                            ? 'Đã duyệt'
                                                            : 'Chờ duyệt'}
                                                    </span>
                                                </div>
                                            </div>

                                            <div className="flex items-center gap-1 shrink-0">
                                                {item.active !== true && (
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="h-8 w-8 text-green-600 hover:text-green-700 hover:bg-green-100"
                                                        onClick={() => handleApprove(item.id)}
                                                        title="Duyệt"
                                                    >
                                                        <Check className="h-4 w-4" />
                                                    </Button>
                                                )}
                                                {item.active !== false && (
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="h-8 w-8 text-orange-600 hover:text-orange-700 hover:bg-orange-100"
                                                        onClick={() => handleReject(item.id)}
                                                        title="Từ chối"
                                                    >
                                                        <X className="h-4 w-4" />
                                                    </Button>
                                                )}
                                                <Button
                                                    variant="ghost"
                                                    size="icon"
                                                    className="h-8 w-8"
                                                    onClick={() => handleDelete(item.id)}
                                                    title="Xóa"
                                                >
                                                    <Trash2 className="h-4 w-4 text-destructive" />
                                                </Button>
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>

                            <div className="flex items-center justify-between mt-4 pt-4 border-t">
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



