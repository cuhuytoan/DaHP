import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { articlesApi, masterDataApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Textarea } from '../../components/ui/textarea';
import { Select } from '../../components/ui/select';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '../../components/ui/card';
import { ImageUploader } from '../../components/ui/image-uploader';
import { ArrowLeft, Save, Loader2, Eye, Send } from 'lucide-react';

interface ArticleFormData {
    name: string;
    subTitle: string;
    image: string;
    imageDescription: string;
    bannerImage: string;
    description: string;
    content: string;
    author: string;
    articleTypeId: number | null;
    startDate: string;
    endDate: string;
    active: boolean;
    url: string;
    tags: string;
    canCopy: boolean;
    canComment: boolean;
    metaTitle: string;
    metaDescription: string;
    metaKeywords: string;
    categoryIds: number[];
}

const defaultFormData: ArticleFormData = {
    name: '',
    subTitle: '',
    image: '',
    imageDescription: '',
    bannerImage: '',
    description: '',
    content: '',
    author: '',
    articleTypeId: null,
    startDate: '',
    endDate: '',
    active: true,
    url: '',
    tags: '',
    canCopy: true,
    canComment: true,
    metaTitle: '',
    metaDescription: '',
    metaKeywords: '',
    categoryIds: [],
};

export function ArticleFormPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const queryClient = useQueryClient();
    const isEdit = !!id;

    const [formData, setFormData] = useState<ArticleFormData>(defaultFormData);
    const [errors, setErrors] = useState<Record<string, string>>({});

    // Fetch article data for edit
    const { data: articleData, isLoading: isArticleLoading } = useQuery({
        queryKey: ['article', id],
        queryFn: () => articlesApi.getById(Number(id)),
        enabled: isEdit,
    });

    // Fetch categories for dropdown
    const { data: categoriesData } = useQuery({
        queryKey: ['article-categories'],
        queryFn: () => masterDataApi.getArticleCategories(),
    });

    // Fetch article types
    const { data: typesData } = useQuery({
        queryKey: ['article-types'],
        queryFn: () => masterDataApi.getArticleTypes(),
    });

    // Populate form when editing
    useEffect(() => {
        if (articleData?.data?.data) {
            const a = articleData.data.data;
            setFormData({
                name: a.name || '',
                subTitle: a.subTitle || '',
                image: a.image || '',
                imageDescription: a.imageDescription || '',
                bannerImage: a.bannerImage || '',
                description: a.description || '',
                content: a.content || '',
                author: a.author || '',
                articleTypeId: a.articleTypeId || null,
                startDate: a.startDate ? new Date(a.startDate).toISOString().split('T')[0] : '',
                endDate: a.endDate ? new Date(a.endDate).toISOString().split('T')[0] : '',
                active: a.active ?? true,
                url: a.url || '',
                tags: a.tags || '',
                canCopy: a.canCopy ?? true,
                canComment: a.canComment ?? true,
                metaTitle: a.metaTitle || '',
                metaDescription: a.metaDescription || '',
                metaKeywords: a.metaKeywords || '',
                categoryIds: a.categories?.map((c: { id: number }) => c.id) || [],
            });
        }
    }, [articleData]);

    // Create/Update mutation - now sends JSON
    const mutation = useMutation({
        mutationFn: (data: ArticleFormData) => {
            const payload = {
                ...data,
                articleTypeId: data.articleTypeId || undefined,
                startDate: data.startDate || undefined,
                endDate: data.endDate || undefined,
            };
            return isEdit
                ? articlesApi.update(Number(id), payload)
                : articlesApi.create(payload);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['articles'] });
            navigate('/articles');
        },
        onError: (error: unknown) => {
            const err = error as { response?: { data?: { errors?: string[] } } };
            if (err?.response?.data?.errors) {
                setErrors({ form: err.response.data.errors.join(', ') });
            }
        },
    });

    // Approve mutation
    const approveMutation = useMutation({
        mutationFn: () => articlesApi.approve(Number(id)),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['article', id] });
            queryClient.invalidateQueries({ queryKey: ['articles'] });
        },
    });

    // Publish mutation
    const publishMutation = useMutation({
        mutationFn: () => articlesApi.publish(Number(id)),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['article', id] });
            queryClient.invalidateQueries({ queryKey: ['articles'] });
            navigate('/articles');
        },
    });

    const validateForm = (): boolean => {
        const newErrors: Record<string, string> = {};
        if (!formData.name.trim()) {
            newErrors.name = 'Tiêu đề là bắt buộc';
        }
        if (!formData.content.trim()) {
            newErrors.content = 'Nội dung là bắt buộc';
        }
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (validateForm()) {
            mutation.mutate(formData);
        }
    };

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
    ) => {
        const { name, value, type } = e.target;
        const checked = (e.target as HTMLInputElement).checked;
        setFormData((prev) => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value,
        }));
        // Clear error when field is modified
        if (errors[name]) {
            setErrors((prev) => ({ ...prev, [name]: '' }));
        }
    };

    const handleCategoryChange = (categoryId: number, checked: boolean) => {
        setFormData((prev) => ({
            ...prev,
            categoryIds: checked
                ? [...prev.categoryIds, categoryId]
                : prev.categoryIds.filter((id) => id !== categoryId),
        }));
    };

    // Generate URL slug from title
    const generateUrl = () => {
        const slug = formData.name
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .replace(/đ/g, 'd')
            .replace(/Đ/g, 'd')
            .replace(/[^a-z0-9]+/g, '-')
            .replace(/(^-|-$)/g, '');
        setFormData((prev) => ({ ...prev, url: slug }));
    };

    if (isEdit && isArticleLoading) {
        return (
            <div className="flex justify-center items-center h-64">
                <Loader2 className="h-8 w-8 animate-spin text-primary" />
            </div>
        );
    }

    const categories = categoriesData?.data?.data || [];
    const types = typesData?.data?.data || [];
    const article = articleData?.data?.data;

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex items-center justify-between">
                <div className="flex items-center gap-4">
                    <Button variant="ghost" size="icon" onClick={() => navigate('/articles')}>
                        <ArrowLeft className="h-4 w-4" />
                    </Button>
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                            {isEdit ? 'Chỉnh sửa bài viết' : 'Thêm bài viết mới'}
                        </h1>
                        {isEdit && article && (
                            <p className="text-sm text-muted-foreground">
                                ID: {article.id} | Trạng thái: {article.articleStatusName || 'Nháp'}
                            </p>
                        )}
                    </div>
                </div>
                {isEdit && (
                    <div className="flex gap-2">
                        <Button
                            type="button"
                            variant="outline"
                            onClick={() => window.open(`/articles/${id}/preview`, '_blank')}
                        >
                            <Eye className="h-4 w-4 mr-2" />
                            Xem trước
                        </Button>
                        {article?.approved !== 1 && (
                            <Button
                                type="button"
                                variant="secondary"
                                onClick={() => approveMutation.mutate()}
                                disabled={approveMutation.isPending}
                            >
                                {approveMutation.isPending && (
                                    <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                                )}
                                Duyệt bài
                            </Button>
                        )}
                        <Button
                            type="button"
                            onClick={() => publishMutation.mutate()}
                            disabled={publishMutation.isPending}
                        >
                            {publishMutation.isPending ? (
                                <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                            ) : (
                                <Send className="h-4 w-4 mr-2" />
                            )}
                            Xuất bản
                        </Button>
                    </div>
                )}
            </div>

            {errors.form && (
                <div className="bg-destructive/10 text-destructive px-4 py-3 rounded-lg">
                    {errors.form}
                </div>
            )}

            <form onSubmit={handleSubmit}>
                <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                    {/* Main content */}
                    <div className="lg:col-span-2 space-y-6">
                        <Card>
                            <CardHeader>
                                <CardTitle>Nội dung chính</CardTitle>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">
                                        Tiêu đề <span className="text-destructive">*</span>
                                    </label>
                                    <Input
                                        name="name"
                                        value={formData.name}
                                        onChange={handleChange}
                                        placeholder="Nhập tiêu đề bài viết"
                                        className={errors.name ? 'border-destructive' : ''}
                                    />
                                    {errors.name && (
                                        <p className="text-sm text-destructive">{errors.name}</p>
                                    )}
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Tiêu đề phụ</label>
                                    <Input
                                        name="subTitle"
                                        value={formData.subTitle}
                                        onChange={handleChange}
                                        placeholder="Tiêu đề phụ (nếu có)"
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Mô tả ngắn</label>
                                    <Textarea
                                        name="description"
                                        value={formData.description}
                                        onChange={handleChange}
                                        placeholder="Mô tả ngắn gọn về bài viết..."
                                        rows={3}
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">
                                        Nội dung chi tiết <span className="text-destructive">*</span>
                                    </label>
                                    <Textarea
                                        name="content"
                                        value={formData.content}
                                        onChange={handleChange}
                                        placeholder="Nội dung đầy đủ của bài viết..."
                                        rows={15}
                                        className={errors.content ? 'border-destructive' : ''}
                                    />
                                    {errors.content && (
                                        <p className="text-sm text-destructive">{errors.content}</p>
                                    )}
                                </div>
                            </CardContent>
                        </Card>

                        {/* SEO */}
                        <Card>
                            <CardHeader>
                                <CardTitle>SEO & Metadata</CardTitle>
                                <CardDescription>
                                    Tối ưu hóa cho công cụ tìm kiếm
                                </CardDescription>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">URL</label>
                                    <div className="flex gap-2">
                                        <Input
                                            name="url"
                                            value={formData.url}
                                            onChange={handleChange}
                                            placeholder="duong-dan-bai-viet"
                                        />
                                        <Button type="button" variant="outline" onClick={generateUrl}>
                                            Tạo tự động
                                        </Button>
                                    </div>
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Meta Title</label>
                                    <Input
                                        name="metaTitle"
                                        value={formData.metaTitle}
                                        onChange={handleChange}
                                        placeholder="Tiêu đề SEO"
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Meta Description</label>
                                    <Textarea
                                        name="metaDescription"
                                        value={formData.metaDescription}
                                        onChange={handleChange}
                                        placeholder="Mô tả cho SEO (150-160 ký tự)"
                                        rows={2}
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Meta Keywords</label>
                                    <Input
                                        name="metaKeywords"
                                        value={formData.metaKeywords}
                                        onChange={handleChange}
                                        placeholder="từ khóa 1, từ khóa 2, ..."
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Tags</label>
                                    <Input
                                        name="tags"
                                        value={formData.tags}
                                        onChange={handleChange}
                                        placeholder="tag1, tag2, tag3"
                                    />
                                </div>
                            </CardContent>
                        </Card>
                    </div>

                    {/* Sidebar */}
                    <div className="space-y-6">
                        {/* Publish settings */}
                        <Card>
                            <CardHeader>
                                <CardTitle>Xuất bản</CardTitle>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="flex items-center gap-2">
                                    <input
                                        type="checkbox"
                                        id="active"
                                        name="active"
                                        checked={formData.active}
                                        onChange={handleChange}
                                        className="rounded border-input"
                                    />
                                    <label htmlFor="active" className="text-sm">
                                        Hiển thị bài viết
                                    </label>
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Ngày bắt đầu</label>
                                    <Input
                                        type="date"
                                        name="startDate"
                                        value={formData.startDate}
                                        onChange={handleChange}
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Ngày kết thúc</label>
                                    <Input
                                        type="date"
                                        name="endDate"
                                        value={formData.endDate}
                                        onChange={handleChange}
                                    />
                                </div>

                                <div className="flex flex-col gap-2">
                                    <div className="flex items-center gap-2">
                                        <input
                                            type="checkbox"
                                            id="canComment"
                                            name="canComment"
                                            checked={formData.canComment}
                                            onChange={handleChange}
                                            className="rounded border-input"
                                        />
                                        <label htmlFor="canComment" className="text-sm">
                                            Cho phép bình luận
                                        </label>
                                    </div>
                                    <div className="flex items-center gap-2">
                                        <input
                                            type="checkbox"
                                            id="canCopy"
                                            name="canCopy"
                                            checked={formData.canCopy}
                                            onChange={handleChange}
                                            className="rounded border-input"
                                        />
                                        <label htmlFor="canCopy" className="text-sm">
                                            Cho phép sao chép
                                        </label>
                                    </div>
                                </div>

                                <div className="pt-4 flex gap-2">
                                    <Button
                                        type="button"
                                        variant="outline"
                                        className="flex-1"
                                        onClick={() => navigate('/articles')}
                                    >
                                        Hủy
                                    </Button>
                                    <Button type="submit" className="flex-1" disabled={mutation.isPending}>
                                        {mutation.isPending ? (
                                            <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                                        ) : (
                                            <Save className="h-4 w-4 mr-2" />
                                        )}
                                        Lưu
                                    </Button>
                                </div>
                            </CardContent>
                        </Card>

                        {/* Images */}
                        <Card>
                            <CardHeader>
                                <CardTitle>Hình ảnh</CardTitle>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Ảnh đại diện</label>
                                    <ImageUploader
                                        value={formData.image}
                                        onChange={(url) =>
                                            setFormData((prev) => ({ ...prev, image: url || '' }))
                                        }
                                        folder="data/article/mainimages"
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Mô tả ảnh</label>
                                    <Input
                                        name="imageDescription"
                                        value={formData.imageDescription}
                                        onChange={handleChange}
                                        placeholder="Alt text cho ảnh"
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Ảnh banner</label>
                                    <ImageUploader
                                        value={formData.bannerImage}
                                        onChange={(url) =>
                                            setFormData((prev) => ({ ...prev, bannerImage: url || '' }))
                                        }
                                        folder="data/article/banners"
                                    />
                                </div>
                            </CardContent>
                        </Card>

                        {/* Classification */}
                        <Card>
                            <CardHeader>
                                <CardTitle>Phân loại</CardTitle>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Loại bài viết</label>
                                    <Select
                                        name="articleTypeId"
                                        value={formData.articleTypeId?.toString() || ''}
                                        onChange={(e) =>
                                            setFormData((prev) => ({
                                                ...prev,
                                                articleTypeId: e.target.value ? Number(e.target.value) : null,
                                            }))
                                        }
                                    >
                                        <option value="">-- Chọn loại --</option>
                                        {types.map((type: { id: number; name: string }) => (
                                            <option key={type.id} value={type.id}>
                                                {type.name}
                                            </option>
                                        ))}
                                    </Select>
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Tác giả</label>
                                    <Input
                                        name="author"
                                        value={formData.author}
                                        onChange={handleChange}
                                        placeholder="Tên tác giả"
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Danh mục</label>
                                    <div className="border rounded-md p-3 max-h-48 overflow-y-auto space-y-2">
                                        {categories.length === 0 ? (
                                            <p className="text-sm text-muted-foreground">
                                                Chưa có danh mục
                                            </p>
                                        ) : (
                                            categories.map((cat: { id: number; name: string }) => (
                                                <div key={cat.id} className="flex items-center gap-2">
                                                    <input
                                                        type="checkbox"
                                                        id={`cat-${cat.id}`}
                                                        checked={formData.categoryIds.includes(cat.id)}
                                                        onChange={(e) =>
                                                            handleCategoryChange(cat.id, e.target.checked)
                                                        }
                                                        className="rounded border-input"
                                                    />
                                                    <label
                                                        htmlFor={`cat-${cat.id}`}
                                                        className="text-sm cursor-pointer"
                                                    >
                                                        {cat.name}
                                                    </label>
                                                </div>
                                            ))
                                        )}
                                    </div>
                                </div>
                            </CardContent>
                        </Card>
                    </div>
                </div>
            </form>
        </div>
    );
}
