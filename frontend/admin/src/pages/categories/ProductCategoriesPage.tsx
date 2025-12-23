import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { categoriesAdminApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import {
    Plus,
    Edit,
    Trash2,
    ChevronRight,
    ChevronDown,
    ToggleLeft,
    ToggleRight,
    Loader2,
    X,
    Save,
    Boxes,
} from 'lucide-react';
import { ImageUploader } from '../../components/ui/image-uploader';
import { Textarea } from '../../components/ui/textarea';
import { Select } from '../../components/ui/select';

interface Category {
    id: number;
    parentId?: number | null;
    name: string;
    url?: string;
    image?: string;
    description?: string;
    sort?: number;
    displayMenu?: boolean;
    menuColor?: string;
    active?: boolean;
    canDelete?: boolean;
    createDate?: string;
}

interface CategoryFormData {
    name: string;
    parentId: number | null;
    url: string;
    image: string;
    description: string;
    sort: number;
    displayMenu: boolean;
    menuColor: string;
    active: boolean;
}

const defaultFormData: CategoryFormData = {
    name: '',
    parentId: null,
    url: '',
    image: '',
    description: '',
    sort: 0,
    displayMenu: true,
    menuColor: '',
    active: true,
};

export function ProductCategoriesPage() {
    const queryClient = useQueryClient();
    const [expandedIds, setExpandedIds] = useState<Set<number>>(new Set());
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [editingId, setEditingId] = useState<number | null>(null);
    const [formData, setFormData] = useState<CategoryFormData>(defaultFormData);

    const { data, isLoading } = useQuery({
        queryKey: ['product-categories-admin'],
        queryFn: () => categoriesAdminApi.getProductCategories({ includeInactive: true }),
    });

    const createMutation = useMutation({
        mutationFn: (data: CategoryFormData) => categoriesAdminApi.createProductCategory(data as unknown as Record<string, unknown>),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product-categories-admin'] });
            closeForm();
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: CategoryFormData }) =>
            categoriesAdminApi.updateProductCategory(id, data as unknown as Record<string, unknown>),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product-categories-admin'] });
            closeForm();
        },
    });

    const deleteMutation = useMutation({
        mutationFn: (id: number) => categoriesAdminApi.deleteProductCategory(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product-categories-admin'] });
        },
    });

    const toggleStatusMutation = useMutation({
        mutationFn: (id: number) => categoriesAdminApi.toggleProductCategoryStatus(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['product-categories-admin'] });
        },
    });

    const categories: Category[] = data?.data?.data || [];

    const buildTree = (items: Category[], parentId: number | null = null): Category[] => {
        return items
            .filter((item) => item.parentId === parentId)
            .sort((a, b) => (a.sort || 0) - (b.sort || 0));
    };

    const toggleExpand = (id: number) => {
        const newExpanded = new Set(expandedIds);
        if (newExpanded.has(id)) {
            newExpanded.delete(id);
        } else {
            newExpanded.add(id);
        }
        setExpandedIds(newExpanded);
    };

    const openCreateForm = (parentId: number | null = null) => {
        setFormData({ ...defaultFormData, parentId });
        setEditingId(null);
        setIsFormOpen(true);
    };

    const openEditForm = (category: Category) => {
        setFormData({
            name: category.name,
            parentId: category.parentId || null,
            url: category.url || '',
            image: category.image || '',
            description: category.description || '',
            sort: category.sort || 0,
            displayMenu: category.displayMenu ?? true,
            menuColor: category.menuColor || '',
            active: category.active ?? true,
        });
        setEditingId(category.id);
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
        if (confirm(`Bạn có chắc muốn xóa danh mục "${name}"?`)) {
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

    const renderCategoryRow = (category: Category, level: number = 0) => {
        const children = buildTree(categories, category.id);
        const hasChildren = children.length > 0;
        const isExpanded = expandedIds.has(category.id);

        return (
            <div key={category.id}>
                <div
                    className={`flex items-center gap-2 py-2 px-3 border-b hover:bg-muted/50 ${
                        !category.active ? 'opacity-60' : ''
                    }`}
                    style={{ paddingLeft: `${level * 24 + 12}px` }}
                >
                    <button
                        type="button"
                        className="w-5 h-5 flex items-center justify-center"
                        onClick={() => hasChildren && toggleExpand(category.id)}
                    >
                        {hasChildren ? (
                            isExpanded ? (
                                <ChevronDown className="h-4 w-4" />
                            ) : (
                                <ChevronRight className="h-4 w-4" />
                            )
                        ) : (
                            <span className="w-4" />
                        )}
                    </button>

                    {category.image && (
                        <img
                            src={category.image}
                            alt={category.name}
                            className="w-8 h-8 rounded object-cover"
                        />
                    )}

                    <span className="flex-1 font-medium">{category.name}</span>
                    <span className="text-sm text-muted-foreground w-24">/{category.url}</span>
                    <span className="text-sm text-muted-foreground w-16 text-center">
                        {category.sort}
                    </span>
                    <span
                        className={`text-xs px-2 py-0.5 rounded ${
                            category.active
                                ? 'bg-green-100 text-green-800'
                                : 'bg-gray-100 text-gray-600'
                        }`}
                    >
                        {category.active ? 'Hiện' : 'Ẩn'}
                    </span>

                    <div className="flex items-center gap-1">
                        <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8"
                            onClick={() => openCreateForm(category.id)}
                            title="Thêm danh mục con"
                        >
                            <Plus className="h-4 w-4" />
                        </Button>
                        <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8"
                            onClick={() => toggleStatusMutation.mutate(category.id)}
                            title={category.active ? 'Ẩn' : 'Hiện'}
                        >
                            {category.active ? (
                                <ToggleRight className="h-4 w-4 text-green-600" />
                            ) : (
                                <ToggleLeft className="h-4 w-4" />
                            )}
                        </Button>
                        <Button
                            variant="ghost"
                            size="icon"
                            className="h-8 w-8"
                            onClick={() => openEditForm(category)}
                            title="Sửa"
                        >
                            <Edit className="h-4 w-4" />
                        </Button>
                        {category.canDelete !== false && (
                            <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8"
                                onClick={() => handleDelete(category.id, category.name)}
                                title="Xóa"
                            >
                                <Trash2 className="h-4 w-4 text-destructive" />
                            </Button>
                        )}
                    </div>
                </div>

                {hasChildren && isExpanded && (
                    <div>
                        {children.map((child) => renderCategoryRow(child, level + 1))}
                    </div>
                )}
            </div>
        );
    };

    const rootCategories = buildTree(categories, null);

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight flex items-center gap-2">
                        <Boxes className="h-8 w-8" />
                        Danh mục sản phẩm
                    </h1>
                    <p className="text-muted-foreground">
                        Quản lý cây danh mục cho sản phẩm
                    </p>
                </div>
                <Button onClick={() => openCreateForm()}>
                    <Plus className="h-4 w-4 mr-2" />
                    Thêm danh mục
                </Button>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Category tree */}
                <div className="lg:col-span-2">
                    <Card>
                        <CardHeader className="py-3">
                            <div className="flex items-center text-sm font-medium text-muted-foreground">
                                <span className="flex-1 pl-10">Tên danh mục</span>
                                <span className="w-24">URL</span>
                                <span className="w-16 text-center">Thứ tự</span>
                                <span className="w-12">T.thái</span>
                                <span className="w-32 text-center">Thao tác</span>
                            </div>
                        </CardHeader>
                        <CardContent className="p-0">
                            {isLoading ? (
                                <div className="flex items-center justify-center py-8">
                                    <Loader2 className="h-8 w-8 animate-spin text-primary" />
                                </div>
                            ) : rootCategories.length === 0 ? (
                                <div className="text-center py-8 text-muted-foreground">
                                    Chưa có danh mục nào
                                </div>
                            ) : (
                                <div className="divide-y">
                                    {rootCategories.map((cat) => renderCategoryRow(cat))}
                                </div>
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
                                    {editingId ? 'Sửa danh mục' : 'Thêm danh mục'}
                                </CardTitle>
                                <Button variant="ghost" size="icon" onClick={closeForm}>
                                    <X className="h-4 w-4" />
                                </Button>
                            </CardHeader>
                            <CardContent>
                                <form onSubmit={handleSubmit} className="space-y-4">
                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Tên danh mục *</label>
                                        <Input
                                            value={formData.name}
                                            onChange={(e) =>
                                                setFormData((prev) => ({
                                                    ...prev,
                                                    name: e.target.value,
                                                }))
                                            }
                                            placeholder="Nhập tên danh mục"
                                            required
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Danh mục cha</label>
                                        <Select
                                            value={formData.parentId?.toString() || ''}
                                            onChange={(e) =>
                                                setFormData((prev) => ({
                                                    ...prev,
                                                    parentId: e.target.value
                                                        ? Number(e.target.value)
                                                        : null,
                                                }))
                                            }
                                        >
                                            <option value="">-- Không có --</option>
                                            {categories
                                                .filter((c) => c.id !== editingId)
                                                .map((cat) => (
                                                    <option key={cat.id} value={cat.id}>
                                                        {cat.name}
                                                    </option>
                                                ))}
                                        </Select>
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">URL</label>
                                        <div className="flex gap-2">
                                            <Input
                                                value={formData.url}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({
                                                        ...prev,
                                                        url: e.target.value,
                                                    }))
                                                }
                                                placeholder="ten-danh-muc"
                                            />
                                            <Button type="button" variant="outline" onClick={generateUrl}>
                                                Tạo
                                            </Button>
                                        </div>
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Ảnh đại diện</label>
                                        <ImageUploader
                                            value={formData.image}
                                            onChange={(url) =>
                                                setFormData((prev) => ({ ...prev, image: url || '' }))
                                            }
                                            folder="data/product/categories"
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
                                            placeholder="Mô tả danh mục..."
                                        />
                                    </div>

                                    <div className="grid grid-cols-2 gap-4">
                                        <div className="space-y-2">
                                            <label className="text-sm font-medium">Thứ tự</label>
                                            <Input
                                                type="number"
                                                value={formData.sort}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({
                                                        ...prev,
                                                        sort: Number(e.target.value),
                                                    }))
                                                }
                                            />
                                        </div>
                                        <div className="space-y-2">
                                            <label className="text-sm font-medium">Màu menu</label>
                                            <Input
                                                type="color"
                                                value={formData.menuColor || '#000000'}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({
                                                        ...prev,
                                                        menuColor: e.target.value,
                                                    }))
                                                }
                                                className="h-10 p-1"
                                            />
                                        </div>
                                    </div>

                                    <div className="space-y-2">
                                        <div className="flex items-center gap-2">
                                            <input
                                                type="checkbox"
                                                id="displayMenu"
                                                checked={formData.displayMenu}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({
                                                        ...prev,
                                                        displayMenu: e.target.checked,
                                                    }))
                                                }
                                                className="rounded"
                                            />
                                            <label htmlFor="displayMenu" className="text-sm">
                                                Hiển thị trên menu
                                            </label>
                                        </div>
                                        <div className="flex items-center gap-2">
                                            <input
                                                type="checkbox"
                                                id="active"
                                                checked={formData.active}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({
                                                        ...prev,
                                                        active: e.target.checked,
                                                    }))
                                                }
                                                className="rounded"
                                            />
                                            <label htmlFor="active" className="text-sm">
                                                Kích hoạt
                                            </label>
                                        </div>
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
                                            disabled={
                                                createMutation.isPending || updateMutation.isPending
                                            }
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
                                <Boxes className="h-12 w-12 mx-auto mb-3 opacity-50" />
                                <p>Chọn một danh mục để sửa</p>
                                <p className="text-sm">hoặc thêm danh mục mới</p>
                                <Button
                                    className="mt-4"
                                    variant="outline"
                                    onClick={() => openCreateForm()}
                                >
                                    <Plus className="h-4 w-4 mr-2" />
                                    Thêm danh mục mới
                                </Button>
                            </CardContent>
                        </Card>
                    )}
                </div>
            </div>
        </div>
    );
}

