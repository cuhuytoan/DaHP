import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { productBrandsApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import {
    Plus,
    Search,
    Edit,
    Trash2,
    ToggleLeft,
    ToggleRight,
    Loader2,
    X,
    Save,
    Tag,
} from 'lucide-react';
import { ImageUploader } from '../../components/ui/image-uploader';
import { Textarea } from '../../components/ui/textarea';

interface Brand {
    id: number;
    code?: string;
    name: string;
    tradingName?: string;
    brandName?: string;
    url?: string;
    image?: string;
    description?: string;
    address?: string;
    telephone?: string;
    mobile?: string;
    email?: string;
    website?: string;
    facebook?: string;
    zalo?: string;
    hotline?: string;
    bankAcc?: string;
    active?: boolean;
    viewCount?: number;
    createDate?: string;
}

interface BrandFormData {
    name: string;
    code: string;
    tradingName: string;
    brandName: string;
    url: string;
    image: string;
    description: string;
    address: string;
    telephone: string;
    mobile: string;
    email: string;
    website: string;
    facebook: string;
    zalo: string;
    hotline: string;
    bankAcc: string;
    active: boolean;
}

const defaultFormData: BrandFormData = {
    name: '',
    code: '',
    tradingName: '',
    brandName: '',
    url: '',
    image: '',
    description: '',
    address: '',
    telephone: '',
    mobile: '',
    email: '',
    website: '',
    facebook: '',
    zalo: '',
    hotline: '',
    bankAcc: '',
    active: true,
};

export function ProductBrandsPage() {
    const queryClient = useQueryClient();
    const [search, setSearch] = useState('');
    const [page, setPage] = useState(1);
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [editingId, setEditingId] = useState<number | null>(null);
    const [formData, setFormData] = useState<BrandFormData>(defaultFormData);

    const { data, isLoading } = useQuery({
        queryKey: ['product-brands', page, search],
        queryFn: () => productBrandsApi.getAll({ page, pageSize: 10, keyword: search }),
    });

    const createMutation = useMutation({
        mutationFn: (data: BrandFormData) => productBrandsApi.create(data as unknown as Record<string, unknown>),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product-brands'] });
            closeForm();
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: BrandFormData }) =>
            productBrandsApi.update(id, data as unknown as Record<string, unknown>),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product-brands'] });
            closeForm();
        },
    });

    const deleteMutation = useMutation({
        mutationFn: (id: number) => productBrandsApi.delete(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product-brands'] });
        },
    });

    const toggleStatusMutation = useMutation({
        mutationFn: (id: number) => productBrandsApi.toggleStatus(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product-brands'] });
        },
    });

    const brands: Brand[] = data?.data?.data || [];
    const pagination = data?.data?.pagination;
    const totalPages = pagination?.totalPages || 1;

    const openCreateForm = () => {
        setFormData(defaultFormData);
        setEditingId(null);
        setIsFormOpen(true);
    };

    const openEditForm = (brand: Brand) => {
        setFormData({
            name: brand.name,
            code: brand.code || '',
            tradingName: brand.tradingName || '',
            brandName: brand.brandName || '',
            url: brand.url || '',
            image: brand.image || '',
            description: brand.description || '',
            address: brand.address || '',
            telephone: brand.telephone || '',
            mobile: brand.mobile || '',
            email: brand.email || '',
            website: brand.website || '',
            facebook: brand.facebook || '',
            zalo: brand.zalo || '',
            hotline: brand.hotline || '',
            bankAcc: brand.bankAcc || '',
            active: brand.active ?? true,
        });
        setEditingId(brand.id);
        setIsFormOpen(true);
    };

    const closeForm = () => {
        setIsFormOpen(false);
        setEditingId(null);
        setFormData(defaultFormData);
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (editingId) {
            updateMutation.mutate({ id: editingId, data: formData });
        } else {
            createMutation.mutate(formData);
        }
    };

    const handleDelete = (id: number, name: string) => {
        if (confirm(`Bạn có chắc muốn xóa thương hiệu "${name}"?`)) {
            deleteMutation.mutate(id);
        }
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

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight flex items-center gap-2">
                        <Tag className="h-8 w-8" />
                        Thương hiệu
                    </h1>
                    <p className="text-muted-foreground">Quản lý thương hiệu sản phẩm</p>
                </div>
                <Button onClick={openCreateForm}>
                    <Plus className="h-4 w-4 mr-2" />
                    Thêm thương hiệu
                </Button>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Brand list */}
                <div className="lg:col-span-2">
                    <Card>
                        <CardHeader>
                            <div className="relative max-w-sm">
                                <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                                <Input
                                    placeholder="Tìm kiếm thương hiệu..."
                                    value={search}
                                    onChange={(e) => {
                                        setSearch(e.target.value);
                                        setPage(1);
                                    }}
                                    className="pl-9"
                                />
                            </div>
                        </CardHeader>
                        <CardContent>
                            {isLoading ? (
                                <div className="flex items-center justify-center py-8">
                                    <Loader2 className="h-8 w-8 animate-spin text-primary" />
                                </div>
                            ) : brands.length === 0 ? (
                                <div className="text-center py-8 text-muted-foreground">
                                    Chưa có thương hiệu nào
                                </div>
                            ) : (
                                <>
                                    <div className="space-y-2">
                                        {brands.map((brand) => (
                                            <div
                                                key={brand.id}
                                                className={`flex items-center gap-4 p-3 rounded-lg border hover:bg-muted/50 ${
                                                    !brand.active ? 'opacity-60' : ''
                                                }`}
                                            >
                                                {brand.image ? (
                                                    <img
                                                        src={brand.image}
                                                        alt={brand.name}
                                                        className="w-12 h-12 rounded object-cover"
                                                    />
                                                ) : (
                                                    <div className="w-12 h-12 rounded bg-muted flex items-center justify-center">
                                                        <Tag className="h-6 w-6 text-muted-foreground" />
                                                    </div>
                                                )}

                                                <div className="flex-1 min-w-0">
                                                    <p className="font-medium truncate">{brand.name}</p>
                                                    <p className="text-sm text-muted-foreground truncate">
                                                        {brand.code && `Mã: ${brand.code} | `}
                                                        {brand.telephone || brand.mobile || brand.email || brand.website || 'Chưa có thông tin'}
                                                    </p>
                                                </div>

                                                <span
                                                    className={`text-xs px-2 py-0.5 rounded ${
                                                        brand.active
                                                            ? 'bg-green-100 text-green-800'
                                                            : 'bg-gray-100 text-gray-600'
                                                    }`}
                                                >
                                                    {brand.active ? 'Hiện' : 'Ẩn'}
                                                </span>

                                                <div className="flex items-center gap-1">
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="h-8 w-8"
                                                        onClick={() => toggleStatusMutation.mutate(brand.id)}
                                                    >
                                                        {brand.active ? (
                                                            <ToggleRight className="h-4 w-4 text-green-600" />
                                                        ) : (
                                                            <ToggleLeft className="h-4 w-4" />
                                                        )}
                                                    </Button>
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="h-8 w-8"
                                                        onClick={() => openEditForm(brand)}
                                                    >
                                                        <Edit className="h-4 w-4" />
                                                    </Button>
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="h-8 w-8"
                                                        onClick={() => handleDelete(brand.id, brand.name)}
                                                    >
                                                        <Trash2 className="h-4 w-4 text-destructive" />
                                                    </Button>
                                                </div>
                                            </div>
                                        ))}
                                    </div>

                                    <div className="flex items-center justify-between mt-4 pt-4 border-t">
                                        <span className="text-sm text-muted-foreground">
                                            Trang {page} / {totalPages}
                                            {pagination?.totalCount && ` (${pagination.totalCount} thương hiệu)`}
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

                {/* Form */}
                <div>
                    {isFormOpen ? (
                        <Card>
                            <CardHeader className="flex flex-row items-center justify-between py-3">
                                <CardTitle className="text-lg">
                                    {editingId ? 'Sửa thương hiệu' : 'Thêm thương hiệu'}
                                </CardTitle>
                                <Button variant="ghost" size="icon" onClick={closeForm}>
                                    <X className="h-4 w-4" />
                                </Button>
                            </CardHeader>
                            <CardContent className="max-h-[calc(100vh-200px)] overflow-y-auto">
                                <form onSubmit={handleSubmit} className="space-y-4">
                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Tên thương hiệu *</label>
                                        <Input
                                            value={formData.name}
                                            onChange={(e) =>
                                                setFormData((prev) => ({ ...prev, name: e.target.value }))
                                            }
                                            placeholder="Nhập tên thương hiệu"
                                            required
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Mã</label>
                                        <Input
                                            value={formData.code}
                                            onChange={(e) =>
                                                setFormData((prev) => ({ ...prev, code: e.target.value }))
                                            }
                                            placeholder="Mã thương hiệu"
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">URL</label>
                                        <div className="flex gap-2">
                                            <Input
                                                value={formData.url}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({ ...prev, url: e.target.value }))
                                                }
                                                placeholder="ten-thuong-hieu"
                                            />
                                            <Button type="button" variant="outline" onClick={generateUrl}>
                                                Tạo
                                            </Button>
                                        </div>
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Logo</label>
                                        <ImageUploader
                                            value={formData.image}
                                            onChange={(url) =>
                                                setFormData((prev) => ({ ...prev, image: url || '' }))
                                            }
                                            folder="data/productbrand"
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Mô tả</label>
                                        <Textarea
                                            value={formData.description}
                                            onChange={(e) =>
                                                setFormData((prev) => ({
                                                    ...prev,
                                                    description: e.target.value,
                                                }))
                                            }
                                            rows={3}
                                            placeholder="Mô tả thương hiệu..."
                                        />
                                    </div>

                                    <div className="grid grid-cols-2 gap-3">
                                        <div className="space-y-2">
                                            <label className="text-sm font-medium">Điện thoại</label>
                                            <Input
                                                value={formData.telephone}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({ ...prev, telephone: e.target.value }))
                                                }
                                                placeholder="0123456789"
                                            />
                                        </div>
                                        <div className="space-y-2">
                                            <label className="text-sm font-medium">Di động</label>
                                            <Input
                                                value={formData.mobile}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({ ...prev, mobile: e.target.value }))
                                                }
                                                placeholder="0912345678"
                                            />
                                        </div>
                                    </div>

                                    <div className="grid grid-cols-2 gap-3">
                                        <div className="space-y-2">
                                            <label className="text-sm font-medium">Email</label>
                                            <Input
                                                type="email"
                                                value={formData.email}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({ ...prev, email: e.target.value }))
                                                }
                                                placeholder="email@domain.com"
                                            />
                                        </div>
                                        <div className="space-y-2">
                                            <label className="text-sm font-medium">Hotline</label>
                                            <Input
                                                value={formData.hotline}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({ ...prev, hotline: e.target.value }))
                                                }
                                                placeholder="1800xxxx"
                                            />
                                        </div>
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Website</label>
                                        <Input
                                            value={formData.website}
                                            onChange={(e) =>
                                                setFormData((prev) => ({ ...prev, website: e.target.value }))
                                            }
                                            placeholder="https://..."
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Địa chỉ</label>
                                        <Input
                                            value={formData.address}
                                            onChange={(e) =>
                                                setFormData((prev) => ({ ...prev, address: e.target.value }))
                                            }
                                            placeholder="Địa chỉ..."
                                        />
                                    </div>

                                    <div className="flex items-center gap-2">
                                        <input
                                            type="checkbox"
                                            id="active"
                                            checked={formData.active}
                                            onChange={(e) =>
                                                setFormData((prev) => ({ ...prev, active: e.target.checked }))
                                            }
                                            className="rounded"
                                        />
                                        <label htmlFor="active" className="text-sm">
                                            Kích hoạt
                                        </label>
                                    </div>

                                    <div className="flex gap-2 pt-2">
                                        <Button
                                            type="button"
                                            variant="outline"
                                            className="flex-1"
                                            onClick={closeForm}
                                        >
                                            Hủy
                                        </Button>
                                        <Button
                                            type="submit"
                                            className="flex-1"
                                            disabled={createMutation.isPending || updateMutation.isPending}
                                        >
                                            {(createMutation.isPending || updateMutation.isPending) && (
                                                <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                                            )}
                                            <Save className="h-4 w-4 mr-2" />
                                            {editingId ? 'Cập nhật' : 'Tạo mới'}
                                        </Button>
                                    </div>
                                </form>
                            </CardContent>
                        </Card>
                    ) : (
                        <Card>
                            <CardContent className="py-8 text-center text-muted-foreground">
                                <Tag className="h-12 w-12 mx-auto mb-3 opacity-50" />
                                <p>Chọn một thương hiệu để sửa</p>
                                <p className="text-sm">hoặc thêm thương hiệu mới</p>
                                <Button className="mt-4" variant="outline" onClick={openCreateForm}>
                                    <Plus className="h-4 w-4 mr-2" />
                                    Thêm thương hiệu mới
                                </Button>
                            </CardContent>
                        </Card>
                    )}
                </div>
            </div>
        </div>
    );
}

