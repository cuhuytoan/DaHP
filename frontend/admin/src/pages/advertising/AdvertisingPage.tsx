import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { advertisingApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import {
    Plus,
    Edit,
    Trash2,
    ToggleLeft,
    ToggleRight,
    Loader2,
    X,
    Save,
    Megaphone,
    ImageIcon,
    ExternalLink,
} from 'lucide-react';
import { ImageUploader } from '../../components/ui/image-uploader';
import { Textarea } from '../../components/ui/textarea';

interface AdvertisingBlock {
    id: number;
    name: string;
    code?: string;
    description?: string;
    active?: boolean;
    sort?: number;
}

interface AdvertisingItem {
    id: number;
    blockId: number;
    name: string;
    image?: string;
    url?: string;
    description?: string;
    sort?: number;
    active?: boolean;
}

export function AdvertisingPage() {
    const queryClient = useQueryClient();
    const [selectedBlockId, setSelectedBlockId] = useState<number | null>(null);
    const [isBlockFormOpen, setIsBlockFormOpen] = useState(false);
    const [editingBlockId, setEditingBlockId] = useState<number | null>(null);
    const [blockFormData, setBlockFormData] = useState({ name: '', code: '', description: '', active: true, sort: 0 });

    const [isItemFormOpen, setIsItemFormOpen] = useState(false);
    const [editingItemId, setEditingItemId] = useState<number | null>(null);
    const [itemFormData, setItemFormData] = useState({
        name: '',
        image: '',
        url: '',
        description: '',
        sort: 0,
        active: true,
    });

    // Blocks query
    const { data: blocksData, isLoading: blocksLoading } = useQuery({
        queryKey: ['advertising-blocks'],
        queryFn: () => advertisingApi.getBlocks(),
    });

    // Items query
    const { data: itemsData, isLoading: itemsLoading } = useQuery({
        queryKey: ['advertising-items', selectedBlockId],
        queryFn: () => advertisingApi.getItems(selectedBlockId!),
        enabled: !!selectedBlockId,
    });

    // Block mutations
    const createBlockMutation = useMutation({
        mutationFn: (data: typeof blockFormData) => advertisingApi.createBlock(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['advertising-blocks'] });
            closeBlockForm();
        },
    });

    const updateBlockMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: typeof blockFormData }) =>
            advertisingApi.updateBlock(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['advertising-blocks'] });
            closeBlockForm();
        },
    });

    const deleteBlockMutation = useMutation({
        mutationFn: (id: number) => advertisingApi.deleteBlock(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['advertising-blocks'] });
            if (selectedBlockId === editingBlockId) setSelectedBlockId(null);
        },
    });

    const toggleBlockStatusMutation = useMutation({
        mutationFn: (id: number) => advertisingApi.toggleBlockStatus(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['advertising-blocks'] }),
    });

    // Item mutations
    const createItemMutation = useMutation({
        mutationFn: (data: typeof itemFormData) =>
            advertisingApi.createItem(selectedBlockId!, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['advertising-items', selectedBlockId] });
            closeItemForm();
        },
    });

    const updateItemMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: typeof itemFormData }) =>
            advertisingApi.updateItem(selectedBlockId!, id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['advertising-items', selectedBlockId] });
            closeItemForm();
        },
    });

    const deleteItemMutation = useMutation({
        mutationFn: (id: number) => advertisingApi.deleteItem(selectedBlockId!, id),
        onSuccess: () =>
            queryClient.invalidateQueries({ queryKey: ['advertising-items', selectedBlockId] }),
    });

    const blocks: AdvertisingBlock[] = blocksData?.data?.data || [];
    const items: AdvertisingItem[] = itemsData?.data?.data || [];

    const openBlockForm = (block?: AdvertisingBlock) => {
        if (block) {
            setBlockFormData({
                name: block.name,
                code: block.code || '',
                description: block.description || '',
                active: block.active ?? true,
                sort: block.sort || 0,
            });
            setEditingBlockId(block.id);
        } else {
            setBlockFormData({ name: '', code: '', description: '', active: true, sort: 0 });
            setEditingBlockId(null);
        }
        setIsBlockFormOpen(true);
    };

    const closeBlockForm = () => {
        setIsBlockFormOpen(false);
        setEditingBlockId(null);
    };

    const openItemForm = (item?: AdvertisingItem) => {
        if (item) {
            setItemFormData({
                name: item.name,
                image: item.image || '',
                url: item.url || '',
                description: item.description || '',
                sort: item.sort || 0,
                active: item.active ?? true,
            });
            setEditingItemId(item.id);
        } else {
            setItemFormData({ name: '', image: '', url: '', description: '', sort: 0, active: true });
            setEditingItemId(null);
        }
        setIsItemFormOpen(true);
    };

    const closeItemForm = () => {
        setIsItemFormOpen(false);
        setEditingItemId(null);
    };

    const handleBlockSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (editingBlockId) {
            updateBlockMutation.mutate({ id: editingBlockId, data: blockFormData });
        } else {
            createBlockMutation.mutate(blockFormData);
        }
    };

    const handleItemSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (editingItemId) {
            updateItemMutation.mutate({ id: editingItemId, data: itemFormData });
        } else {
            createItemMutation.mutate(itemFormData);
        }
    };

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight flex items-center gap-2">
                        <Megaphone className="h-8 w-8" />
                        Quảng cáo
                    </h1>
                    <p className="text-muted-foreground">Quản lý blocks và banners quảng cáo</p>
                </div>
                <Button onClick={() => openBlockForm()}>
                    <Plus className="h-4 w-4 mr-2" />
                    Thêm Block
                </Button>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Blocks list */}
                <div>
                    <Card>
                        <CardHeader className="py-3">
                            <CardTitle className="text-lg">Advertising Blocks</CardTitle>
                        </CardHeader>
                        <CardContent className="p-0">
                            {blocksLoading ? (
                                <div className="flex items-center justify-center py-8">
                                    <Loader2 className="h-6 w-6 animate-spin" />
                                </div>
                            ) : blocks.length === 0 ? (
                                <div className="text-center py-8 text-muted-foreground text-sm">
                                    Chưa có block nào
                                </div>
                            ) : (
                                <div className="divide-y">
                                    {blocks.map((block) => (
                                        <div
                                            key={block.id}
                                            className={`p-3 cursor-pointer hover:bg-muted/50 ${
                                                selectedBlockId === block.id ? 'bg-muted' : ''
                                            } ${!block.active ? 'opacity-60' : ''}`}
                                            onClick={() => setSelectedBlockId(block.id)}
                                        >
                                            <div className="flex items-center justify-between">
                                                <div>
                                                    <p className="font-medium">{block.name}</p>
                                                    <p className="text-xs text-muted-foreground">
                                                        {block.code || `ID: ${block.id}`}
                                                    </p>
                                                </div>
                                                <div className="flex items-center gap-1">
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="h-7 w-7"
                                                        onClick={(e) => {
                                                            e.stopPropagation();
                                                            toggleBlockStatusMutation.mutate(block.id);
                                                        }}
                                                    >
                                                        {block.active ? (
                                                            <ToggleRight className="h-4 w-4 text-green-600" />
                                                        ) : (
                                                            <ToggleLeft className="h-4 w-4" />
                                                        )}
                                                    </Button>
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="h-7 w-7"
                                                        onClick={(e) => {
                                                            e.stopPropagation();
                                                            openBlockForm(block);
                                                        }}
                                                    >
                                                        <Edit className="h-4 w-4" />
                                                    </Button>
                                                    <Button
                                                        variant="ghost"
                                                        size="icon"
                                                        className="h-7 w-7"
                                                        onClick={(e) => {
                                                            e.stopPropagation();
                                                            if (confirm(`Xóa block "${block.name}"?`)) {
                                                                deleteBlockMutation.mutate(block.id);
                                                            }
                                                        }}
                                                    >
                                                        <Trash2 className="h-4 w-4 text-destructive" />
                                                    </Button>
                                                </div>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            )}
                        </CardContent>
                    </Card>
                </div>

                {/* Items */}
                <div className="lg:col-span-2">
                    {selectedBlockId ? (
                        <Card>
                            <CardHeader className="flex flex-row items-center justify-between py-3">
                                <CardTitle className="text-lg">
                                    Banners trong block:{' '}
                                    {blocks.find((b) => b.id === selectedBlockId)?.name}
                                </CardTitle>
                                <Button size="sm" onClick={() => openItemForm()}>
                                    <Plus className="h-4 w-4 mr-1" />
                                    Thêm banner
                                </Button>
                            </CardHeader>
                            <CardContent>
                                {itemsLoading ? (
                                    <div className="flex items-center justify-center py-8">
                                        <Loader2 className="h-6 w-6 animate-spin" />
                                    </div>
                                ) : items.length === 0 ? (
                                    <div className="text-center py-8 text-muted-foreground">
                                        Chưa có banner trong block này
                                    </div>
                                ) : (
                                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                        {items.map((item) => (
                                            <div
                                                key={item.id}
                                                className={`border rounded-lg overflow-hidden ${
                                                    !item.active ? 'opacity-60' : ''
                                                }`}
                                            >
                                                {item.image ? (
                                                    <img
                                                        src={item.image}
                                                        alt={item.name}
                                                        className="w-full h-32 object-cover"
                                                    />
                                                ) : (
                                                    <div className="w-full h-32 bg-muted flex items-center justify-center">
                                                        <ImageIcon className="h-10 w-10 text-muted-foreground" />
                                                    </div>
                                                )}
                                                <div className="p-3">
                                                    <p className="font-medium truncate">{item.name}</p>
                                                    {item.url && (
                                                        <a
                                                            href={item.url}
                                                            target="_blank"
                                                            rel="noopener noreferrer"
                                                            className="text-xs text-blue-500 flex items-center gap-1"
                                                        >
                                                            <ExternalLink className="h-3 w-3" />
                                                            {item.url}
                                                        </a>
                                                    )}
                                                    <div className="flex items-center justify-between mt-2">
                                                        <span className="text-xs text-muted-foreground">
                                                            Thứ tự: {item.sort}
                                                        </span>
                                                        <div className="flex gap-1">
                                                            <Button
                                                                variant="ghost"
                                                                size="icon"
                                                                className="h-7 w-7"
                                                                onClick={() => openItemForm(item)}
                                                            >
                                                                <Edit className="h-4 w-4" />
                                                            </Button>
                                                            <Button
                                                                variant="ghost"
                                                                size="icon"
                                                                className="h-7 w-7"
                                                                onClick={() => {
                                                                    if (confirm(`Xóa banner "${item.name}"?`)) {
                                                                        deleteItemMutation.mutate(item.id);
                                                                    }
                                                                }}
                                                            >
                                                                <Trash2 className="h-4 w-4 text-destructive" />
                                                            </Button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                )}
                            </CardContent>
                        </Card>
                    ) : (
                        <Card>
                            <CardContent className="py-16 text-center text-muted-foreground">
                                <Megaphone className="h-12 w-12 mx-auto mb-3 opacity-50" />
                                <p>Chọn một block để xem và quản lý banners</p>
                            </CardContent>
                        </Card>
                    )}
                </div>
            </div>

            {/* Block Form Modal */}
            {isBlockFormOpen && (
                <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
                    <Card className="w-full max-w-md mx-4">
                        <CardHeader className="flex flex-row items-center justify-between">
                            <CardTitle>{editingBlockId ? 'Sửa Block' : 'Thêm Block'}</CardTitle>
                            <Button variant="ghost" size="icon" onClick={closeBlockForm}>
                                <X className="h-4 w-4" />
                            </Button>
                        </CardHeader>
                        <CardContent>
                            <form onSubmit={handleBlockSubmit} className="space-y-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Tên block *</label>
                                    <Input
                                        value={blockFormData.name}
                                        onChange={(e) =>
                                            setBlockFormData((p) => ({ ...p, name: e.target.value }))
                                        }
                                        required
                                    />
                                </div>
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Mã code</label>
                                    <Input
                                        value={blockFormData.code}
                                        onChange={(e) =>
                                            setBlockFormData((p) => ({ ...p, code: e.target.value }))
                                        }
                                    />
                                </div>
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Mô tả</label>
                                    <Textarea
                                        value={blockFormData.description}
                                        onChange={(e) =>
                                            setBlockFormData((p) => ({ ...p, description: e.target.value }))
                                        }
                                        rows={2}
                                    />
                                </div>
                                <div className="flex items-center gap-2">
                                    <input
                                        type="checkbox"
                                        checked={blockFormData.active}
                                        onChange={(e) =>
                                            setBlockFormData((p) => ({ ...p, active: e.target.checked }))
                                        }
                                        className="rounded"
                                    />
                                    <label className="text-sm">Kích hoạt</label>
                                </div>
                                <div className="flex gap-2">
                                    <Button type="button" variant="outline" className="flex-1" onClick={closeBlockForm}>
                                        Hủy
                                    </Button>
                                    <Button
                                        type="submit"
                                        className="flex-1"
                                        disabled={createBlockMutation.isPending || updateBlockMutation.isPending}
                                    >
                                        <Save className="h-4 w-4 mr-2" />
                                        Lưu
                                    </Button>
                                </div>
                            </form>
                        </CardContent>
                    </Card>
                </div>
            )}

            {/* Item Form Modal */}
            {isItemFormOpen && (
                <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
                    <Card className="w-full max-w-md mx-4">
                        <CardHeader className="flex flex-row items-center justify-between">
                            <CardTitle>{editingItemId ? 'Sửa Banner' : 'Thêm Banner'}</CardTitle>
                            <Button variant="ghost" size="icon" onClick={closeItemForm}>
                                <X className="h-4 w-4" />
                            </Button>
                        </CardHeader>
                        <CardContent>
                            <form onSubmit={handleItemSubmit} className="space-y-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Tên banner *</label>
                                    <Input
                                        value={itemFormData.name}
                                        onChange={(e) =>
                                            setItemFormData((p) => ({ ...p, name: e.target.value }))
                                        }
                                        required
                                    />
                                </div>
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Hình ảnh</label>
                                    <ImageUploader
                                        value={itemFormData.image}
                                        onChange={(url) =>
                                            setItemFormData((p) => ({ ...p, image: url || '' }))
                                        }
                                        folder="data/advertising"
                                    />
                                </div>
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Link URL</label>
                                    <Input
                                        value={itemFormData.url}
                                        onChange={(e) =>
                                            setItemFormData((p) => ({ ...p, url: e.target.value }))
                                        }
                                        placeholder="https://..."
                                    />
                                </div>
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Thứ tự</label>
                                    <Input
                                        type="number"
                                        value={itemFormData.sort}
                                        onChange={(e) =>
                                            setItemFormData((p) => ({ ...p, sort: Number(e.target.value) }))
                                        }
                                    />
                                </div>
                                <div className="flex items-center gap-2">
                                    <input
                                        type="checkbox"
                                        checked={itemFormData.active}
                                        onChange={(e) =>
                                            setItemFormData((p) => ({ ...p, active: e.target.checked }))
                                        }
                                        className="rounded"
                                    />
                                    <label className="text-sm">Kích hoạt</label>
                                </div>
                                <div className="flex gap-2">
                                    <Button type="button" variant="outline" className="flex-1" onClick={closeItemForm}>
                                        Hủy
                                    </Button>
                                    <Button
                                        type="submit"
                                        className="flex-1"
                                        disabled={createItemMutation.isPending || updateItemMutation.isPending}
                                    >
                                        <Save className="h-4 w-4 mr-2" />
                                        Lưu
                                    </Button>
                                </div>
                            </form>
                        </CardContent>
                    </Card>
                </div>
            )}
        </div>
    );
}



