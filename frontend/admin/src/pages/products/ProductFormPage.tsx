import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { productsApi, masterDataApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Textarea } from '../../components/ui/textarea';
import { Select } from '../../components/ui/select';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '../../components/ui/card';
import { ImageUploader } from '../../components/ui/image-uploader';
import { ArrowLeft, Save, Loader2, Eye, Send, Trash2 } from 'lucide-react';

interface ProductPropertyValue {
    productPropertyId: number;
    value: string;
}

interface ProductFormData {
    name: string;
    code: string;
    subTitle: string;
    image: string;
    imageDescription: string;
    bannerImage: string;
    description: string;
    content: string;
    price: number | null;
    priceOld: number | null;
    quantity: number | null;
    productTypeId: number | null;
    productBrandId: number | null;
    productManufactureId: number | null;
    unitId: number | null;
    startDate: string;
    endDate: string;
    active: boolean;
    url: string;
    tags: string;
    canCopy: boolean;
    canComment: boolean;
    allowOrder: boolean;
    allowReview: boolean;
    metaTitle: string;
    metaDescription: string;
    metaKeywords: string;
    categoryIds: number[];
    properties: ProductPropertyValue[];
}

const defaultFormData: ProductFormData = {
    name: '',
    code: '',
    subTitle: '',
    image: '',
    imageDescription: '',
    bannerImage: '',
    description: '',
    content: '',
    price: null,
    priceOld: null,
    quantity: null,
    productTypeId: null,
    productBrandId: null,
    productManufactureId: null,
    unitId: null,
    startDate: '',
    endDate: '',
    active: true,
    url: '',
    tags: '',
    canCopy: true,
    canComment: true,
    allowOrder: true,
    allowReview: true,
    metaTitle: '',
    metaDescription: '',
    metaKeywords: '',
    categoryIds: [],
    properties: [],
};

export function ProductFormPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const queryClient = useQueryClient();
    const isEdit = !!id;

    const [formData, setFormData] = useState<ProductFormData>(defaultFormData);
    const [errors, setErrors] = useState<Record<string, string>>({});
    const [additionalImages, setAdditionalImages] = useState<string[]>([]);

    // Fetch product data for edit
    const { data: productData, isLoading: isProductLoading } = useQuery({
        queryKey: ['product', id],
        queryFn: () => productsApi.getById(Number(id)),
        enabled: isEdit,
    });

    // Fetch categories
    const { data: categoriesData } = useQuery({
        queryKey: ['product-categories'],
        queryFn: () => masterDataApi.getProductCategories(),
    });

    // Fetch product types
    const { data: typesData } = useQuery({
        queryKey: ['product-types'],
        queryFn: () => masterDataApi.getProductTypes(),
    });

    // Fetch units
    const { data: unitsData } = useQuery({
        queryKey: ['units'],
        queryFn: () => masterDataApi.getUnits(),
    });

    // Fetch manufactures
    const { data: manufacturesData } = useQuery({
        queryKey: ['product-manufactures'],
        queryFn: () => masterDataApi.getProductManufactures(),
    });

    // Populate form when editing
    useEffect(() => {
        if (productData?.data?.data) {
            const p = productData.data.data;
            setFormData({
                name: p.name || '',
                code: p.code || '',
                subTitle: p.subTitle || '',
                image: p.image || '',
                imageDescription: p.imageDescription || '',
                bannerImage: p.bannerImage || '',
                description: p.description || '',
                content: p.content || '',
                price: p.price ?? null,
                priceOld: p.priceOld ?? null,
                quantity: p.quantity ?? null,
                productTypeId: p.productTypeId || null,
                productBrandId: p.productBrandId || null,
                productManufactureId: p.productManufactureId || null,
                unitId: p.unitId || null,
                startDate: p.startDate ? new Date(p.startDate).toISOString().split('T')[0] : '',
                endDate: p.endDate ? new Date(p.endDate).toISOString().split('T')[0] : '',
                active: p.active ?? true,
                url: p.url || '',
                tags: p.tags || '',
                canCopy: p.canCopy ?? true,
                canComment: p.canComment ?? true,
                allowOrder: p.allowOrder ?? true,
                allowReview: p.allowReview ?? true,
                metaTitle: p.metaTitle || '',
                metaDescription: p.metaDescription || '',
                metaKeywords: p.metaKeywords || '',
                categoryIds: p.categories?.map((c: { id: number }) => c.id) || [],
                properties: p.properties?.map((prop: { productPropertyId: number; value: string }) => ({
                    productPropertyId: prop.productPropertyId,
                    value: prop.value || '',
                })) || [],
            });
            // Set additional images from pictures
            if (p.pictures) {
                setAdditionalImages(p.pictures.map((pic: { image: string }) => pic.image).filter(Boolean));
            }
        }
    }, [productData]);

    // Create/Update mutation - sends JSON
    const mutation = useMutation({
        mutationFn: (data: ProductFormData) => {
            const payload = {
                ...data,
                productTypeId: data.productTypeId || undefined,
                productBrandId: data.productBrandId || undefined,
                productManufactureId: data.productManufactureId || undefined,
                unitId: data.unitId || undefined,
                price: data.price || undefined,
                priceOld: data.priceOld || undefined,
                quantity: data.quantity || undefined,
                startDate: data.startDate || undefined,
                endDate: data.endDate || undefined,
            };
            return isEdit
                ? productsApi.update(Number(id), payload)
                : productsApi.create(payload);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] });
            navigate('/products');
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
        mutationFn: () => productsApi.approve(Number(id)),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product', id] });
            queryClient.invalidateQueries({ queryKey: ['products'] });
        },
    });

    // Publish mutation
    const publishMutation = useMutation({
        mutationFn: () => productsApi.publish(Number(id)),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product', id] });
            queryClient.invalidateQueries({ queryKey: ['products'] });
            navigate('/products');
        },
    });

    const validateForm = (): boolean => {
        const newErrors: Record<string, string> = {};
        if (!formData.name.trim()) {
            newErrors.name = 'Tên sản phẩm là bắt buộc';
        }
        if (!formData.code.trim()) {
            newErrors.code = 'Mã sản phẩm là bắt buộc';
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
        if (errors[name]) {
            setErrors((prev) => ({ ...prev, [name]: '' }));
        }
    };

    const handleNumberChange = (name: keyof ProductFormData, value: string) => {
        const numValue = value === '' ? null : Number(value);
        setFormData((prev) => ({ ...prev, [name]: numValue }));
    };

    const handleCategoryChange = (categoryId: number, checked: boolean) => {
        setFormData((prev) => ({
            ...prev,
            categoryIds: checked
                ? [...prev.categoryIds, categoryId]
                : prev.categoryIds.filter((id) => id !== categoryId),
        }));
    };

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

    const generateCode = () => {
        const code = `SP${Date.now().toString().slice(-8)}`;
        setFormData((prev) => ({ ...prev, code }));
    };

    const addAdditionalImage = (url: string) => {
        if (url) {
            setAdditionalImages((prev) => [...prev, url]);
        }
    };

    const removeAdditionalImage = (index: number) => {
        setAdditionalImages((prev) => prev.filter((_, i) => i !== index));
    };

    if (isEdit && isProductLoading) {
        return (
            <div className="flex justify-center items-center h-64">
                <Loader2 className="h-8 w-8 animate-spin text-primary" />
            </div>
        );
    }

    const categories = categoriesData?.data?.data || [];
    const types = typesData?.data?.data || [];
    const units = unitsData?.data?.data || [];
    const manufactures = manufacturesData?.data?.data || [];
    const product = productData?.data?.data;

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex items-center justify-between">
                <div className="flex items-center gap-4">
                    <Button variant="ghost" size="icon" onClick={() => navigate('/products')}>
                        <ArrowLeft className="h-4 w-4" />
                    </Button>
                    <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                            {isEdit ? 'Chỉnh sửa sản phẩm' : 'Thêm sản phẩm mới'}
                        </h1>
                        {isEdit && product && (
                            <p className="text-sm text-muted-foreground">
                                Mã: {product.code} | Trạng thái: {product.productStatusName || 'Nháp'}
                            </p>
                        )}
                    </div>
                </div>
                {isEdit && (
                    <div className="flex gap-2">
                        <Button
                            type="button"
                            variant="outline"
                            onClick={() => window.open(`/products/${id}/preview`, '_blank')}
                        >
                            <Eye className="h-4 w-4 mr-2" />
                            Xem trước
                        </Button>
                        {product?.approved !== 1 && (
                            <Button
                                type="button"
                                variant="secondary"
                                onClick={() => approveMutation.mutate()}
                                disabled={approveMutation.isPending}
                            >
                                {approveMutation.isPending && (
                                    <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                                )}
                                Duyệt
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
                                <CardTitle>Thông tin cơ bản</CardTitle>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">
                                            Tên sản phẩm <span className="text-destructive">*</span>
                                        </label>
                                        <Input
                                            name="name"
                                            value={formData.name}
                                            onChange={handleChange}
                                            placeholder="Nhập tên sản phẩm"
                                            className={errors.name ? 'border-destructive' : ''}
                                        />
                                        {errors.name && (
                                            <p className="text-sm text-destructive">{errors.name}</p>
                                        )}
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">
                                            Mã sản phẩm <span className="text-destructive">*</span>
                                        </label>
                                        <div className="flex gap-2">
                                            <Input
                                                name="code"
                                                value={formData.code}
                                                onChange={handleChange}
                                                placeholder="Mã SKU"
                                                className={errors.code ? 'border-destructive' : ''}
                                            />
                                            <Button type="button" variant="outline" onClick={generateCode}>
                                                Tạo mã
                                            </Button>
                                        </div>
                                        {errors.code && (
                                            <p className="text-sm text-destructive">{errors.code}</p>
                                        )}
                                    </div>
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
                                        placeholder="Mô tả ngắn gọn về sản phẩm..."
                                        rows={3}
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Nội dung chi tiết</label>
                                    <Textarea
                                        name="content"
                                        value={formData.content}
                                        onChange={handleChange}
                                        placeholder="Mô tả đầy đủ về sản phẩm..."
                                        rows={10}
                                    />
                                </div>
                            </CardContent>
                        </Card>

                        {/* Pricing & Inventory */}
                        <Card>
                            <CardHeader>
                                <CardTitle>Giá & Kho hàng</CardTitle>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Giá bán (VNĐ)</label>
                                        <Input
                                            type="number"
                                            value={formData.price ?? ''}
                                            onChange={(e) => handleNumberChange('price', e.target.value)}
                                            placeholder="0"
                                            min="0"
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Giá gốc (VNĐ)</label>
                                        <Input
                                            type="number"
                                            value={formData.priceOld ?? ''}
                                            onChange={(e) => handleNumberChange('priceOld', e.target.value)}
                                            placeholder="0"
                                            min="0"
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Số lượng tồn kho</label>
                                        <Input
                                            type="number"
                                            value={formData.quantity ?? ''}
                                            onChange={(e) => handleNumberChange('quantity', e.target.value)}
                                            placeholder="0"
                                            min="0"
                                        />
                                    </div>
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Đơn vị tính</label>
                                    <Select
                                        name="unitId"
                                        value={formData.unitId?.toString() || ''}
                                        onChange={(e) =>
                                            setFormData((prev) => ({
                                                ...prev,
                                                unitId: e.target.value ? Number(e.target.value) : null,
                                            }))
                                        }
                                    >
                                        <option value="">-- Chọn đơn vị --</option>
                                        {units.map((unit: { id: number; name: string }) => (
                                            <option key={unit.id} value={unit.id}>
                                                {unit.name}
                                            </option>
                                        ))}
                                    </Select>
                                </div>
                            </CardContent>
                        </Card>

                        {/* SEO */}
                        <Card>
                            <CardHeader>
                                <CardTitle>SEO & Metadata</CardTitle>
                                <CardDescription>Tối ưu hóa cho công cụ tìm kiếm</CardDescription>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">URL</label>
                                    <div className="flex gap-2">
                                        <Input
                                            name="url"
                                            value={formData.url}
                                            onChange={handleChange}
                                            placeholder="duong-dan-san-pham"
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
                                        Hiển thị sản phẩm
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
                                            id="allowOrder"
                                            name="allowOrder"
                                            checked={formData.allowOrder}
                                            onChange={handleChange}
                                            className="rounded border-input"
                                        />
                                        <label htmlFor="allowOrder" className="text-sm">
                                            Cho phép đặt hàng
                                        </label>
                                    </div>
                                    <div className="flex items-center gap-2">
                                        <input
                                            type="checkbox"
                                            id="allowReview"
                                            name="allowReview"
                                            checked={formData.allowReview}
                                            onChange={handleChange}
                                            className="rounded border-input"
                                        />
                                        <label htmlFor="allowReview" className="text-sm">
                                            Cho phép đánh giá
                                        </label>
                                    </div>
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
                                </div>

                                <div className="pt-4 flex gap-2">
                                    <Button
                                        type="button"
                                        variant="outline"
                                        className="flex-1"
                                        onClick={() => navigate('/products')}
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
                                        folder="data/product/mainimages"
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
                                        folder="data/product/banners"
                                    />
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Ảnh bổ sung</label>
                                    <div className="grid grid-cols-2 gap-2">
                                        {additionalImages.map((img, index) => (
                                            <div key={index} className="relative group">
                                                <img
                                                    src={img}
                                                    alt={`Additional ${index + 1}`}
                                                    className="w-full h-24 object-cover rounded border"
                                                />
                                                <Button
                                                    type="button"
                                                    variant="destructive"
                                                    size="icon"
                                                    className="absolute top-1 right-1 h-6 w-6 opacity-0 group-hover:opacity-100 transition-opacity"
                                                    onClick={() => removeAdditionalImage(index)}
                                                >
                                                    <Trash2 className="h-3 w-3" />
                                                </Button>
                                            </div>
                                        ))}
                                    </div>
                                    <ImageUploader
                                        value=""
                                        onChange={(url) => url && addAdditionalImage(url)}
                                        folder="data/product/pictures"
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
                                    <label className="text-sm font-medium">Loại sản phẩm</label>
                                    <Select
                                        name="productTypeId"
                                        value={formData.productTypeId?.toString() || ''}
                                        onChange={(e) =>
                                            setFormData((prev) => ({
                                                ...prev,
                                                productTypeId: e.target.value ? Number(e.target.value) : null,
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
                                    <label className="text-sm font-medium">Nhà sản xuất</label>
                                    <Select
                                        name="productManufactureId"
                                        value={formData.productManufactureId?.toString() || ''}
                                        onChange={(e) =>
                                            setFormData((prev) => ({
                                                ...prev,
                                                productManufactureId: e.target.value
                                                    ? Number(e.target.value)
                                                    : null,
                                            }))
                                        }
                                    >
                                        <option value="">-- Chọn nhà sản xuất --</option>
                                        {manufactures.map((m: { id: number; name: string }) => (
                                            <option key={m.id} value={m.id}>
                                                {m.name}
                                            </option>
                                        ))}
                                    </Select>
                                </div>

                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Danh mục</label>
                                    <div className="border rounded-md p-3 max-h-48 overflow-y-auto space-y-2">
                                        {categories.length === 0 ? (
                                            <p className="text-sm text-muted-foreground">Chưa có danh mục</p>
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
