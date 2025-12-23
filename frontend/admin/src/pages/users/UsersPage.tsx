import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { usersAdminApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import {
    Plus,
    Search,
    Edit,
    Trash2,
    Lock,
    Unlock,
    Loader2,
    X,
    Save,
    Users,
    Key,
    Shield,
} from 'lucide-react';

interface User {
    id: string;
    email: string;
    fullName?: string;
    phoneNumber?: string;
    userName?: string;
    emailConfirmed?: boolean;
    lockoutEnd?: string;
    roles?: string[];
    createDate?: string;
}

interface UserFormData {
    email: string;
    fullName: string;
    phoneNumber: string;
    password: string;
    roles: string[];
}

const defaultFormData: UserFormData = {
    email: '',
    fullName: '',
    phoneNumber: '',
    password: '',
    roles: [],
};

export function UsersPage() {
    const queryClient = useQueryClient();
    const [search, setSearch] = useState('');
    const [page, setPage] = useState(1);
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [editingId, setEditingId] = useState<string | null>(null);
    const [formData, setFormData] = useState<UserFormData>(defaultFormData);
    const [resetPasswordId, setResetPasswordId] = useState<string | null>(null);
    const [newPassword, setNewPassword] = useState('');

    const { data, isLoading } = useQuery({
        queryKey: ['users', page, search],
        queryFn: () => usersAdminApi.getAll({ page, pageSize: 10, keyword: search }),
    });

    const { data: rolesData } = useQuery({
        queryKey: ['roles'],
        queryFn: () => usersAdminApi.getRoles(),
    });

    const createMutation = useMutation({
        mutationFn: (data: UserFormData) => usersAdminApi.create(data as unknown as Record<string, unknown>),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] });
            closeForm();
        },
    });

    const updateMutation = useMutation({
        mutationFn: ({ id, data }: { id: string; data: Partial<UserFormData> }) =>
            usersAdminApi.update(id, data as unknown as Record<string, unknown>),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] });
            closeForm();
        },
    });

    const deleteMutation = useMutation({
        mutationFn: (id: string) => usersAdminApi.delete(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
    });

    const lockMutation = useMutation({
        mutationFn: (id: string) => usersAdminApi.lockUser(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
    });

    const unlockMutation = useMutation({
        mutationFn: (id: string) => usersAdminApi.unlockUser(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
    });

    const resetPasswordMutation = useMutation({
        mutationFn: ({ id, newPassword }: { id: string; newPassword: string }) =>
            usersAdminApi.resetPassword(id, newPassword),
        onSuccess: () => {
            setResetPasswordId(null);
            setNewPassword('');
            alert('Đặt lại mật khẩu thành công!');
        },
    });

    const users: User[] = data?.data?.data || [];
    const pagination = data?.data?.pagination;
    const totalPages = pagination?.totalPages || 1;
    const roles: { id: string; name: string }[] = rolesData?.data?.data || [];

    const openCreateForm = () => {
        setFormData(defaultFormData);
        setEditingId(null);
        setIsFormOpen(true);
    };

    const openEditForm = (user: User) => {
        setFormData({
            email: user.email,
            fullName: user.fullName || '',
            phoneNumber: user.phoneNumber || '',
            password: '',
            roles: user.roles || [],
        });
        setEditingId(user.id);
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
            const { password, ...updateData } = formData;
            updateMutation.mutate({ id: editingId, data: updateData });
        } else {
            createMutation.mutate(formData);
        }
    };

    const handleDelete = (id: string, email: string) => {
        if (confirm(`Bạn có chắc muốn xóa người dùng "${email}"?`)) {
            deleteMutation.mutate(id);
        }
    };

    const handleRoleToggle = (role: string) => {
        setFormData((prev) => ({
            ...prev,
            roles: prev.roles.includes(role)
                ? prev.roles.filter((r) => r !== role)
                : [...prev.roles, role],
        }));
    };

    const isLocked = (user: User) => {
        if (!user.lockoutEnd) return false;
        return new Date(user.lockoutEnd) > new Date();
    };

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight flex items-center gap-2">
                        <Users className="h-8 w-8" />
                        Người dùng
                    </h1>
                    <p className="text-muted-foreground">Quản lý tài khoản người dùng</p>
                </div>
                <Button onClick={openCreateForm}>
                    <Plus className="h-4 w-4 mr-2" />
                    Thêm người dùng
                </Button>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* User list */}
                <div className="lg:col-span-2">
                    <Card>
                        <CardHeader>
                            <div className="relative max-w-sm">
                                <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                                <Input
                                    placeholder="Tìm theo email, tên..."
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
                            ) : users.length === 0 ? (
                                <div className="text-center py-8 text-muted-foreground">
                                    Không tìm thấy người dùng
                                </div>
                            ) : (
                                <>
                                    <div className="space-y-2">
                                        {users.map((user) => {
                                            const locked = isLocked(user);
                                            return (
                                                <div
                                                    key={user.id}
                                                    className={`flex items-center gap-4 p-3 rounded-lg border hover:bg-muted/50 ${
                                                        locked ? 'opacity-60 bg-destructive/5' : ''
                                                    }`}
                                                >
                                                    <div className="h-10 w-10 rounded-full bg-primary flex items-center justify-center">
                                                        <span className="text-primary-foreground font-medium">
                                                            {user.fullName?.charAt(0) || user.email.charAt(0).toUpperCase()}
                                                        </span>
                                                    </div>

                                                    <div className="flex-1 min-w-0">
                                                        <p className="font-medium truncate">
                                                            {user.fullName || user.email}
                                                        </p>
                                                        <p className="text-sm text-muted-foreground truncate">
                                                            {user.email}
                                                        </p>
                                                    </div>

                                                    <div className="flex flex-wrap gap-1">
                                                        {user.roles?.map((role) => (
                                                            <span
                                                                key={role}
                                                                className="text-xs px-2 py-0.5 rounded bg-primary/10 text-primary"
                                                            >
                                                                {role}
                                                            </span>
                                                        ))}
                                                    </div>

                                                    {locked && (
                                                        <span className="text-xs px-2 py-0.5 rounded bg-destructive/10 text-destructive">
                                                            Khóa
                                                        </span>
                                                    )}

                                                    <div className="flex items-center gap-1">
                                                        <Button
                                                            variant="ghost"
                                                            size="icon"
                                                            className="h-8 w-8"
                                                            onClick={() =>
                                                                locked
                                                                    ? unlockMutation.mutate(user.id)
                                                                    : lockMutation.mutate(user.id)
                                                            }
                                                            title={locked ? 'Mở khóa' : 'Khóa'}
                                                        >
                                                            {locked ? (
                                                                <Unlock className="h-4 w-4" />
                                                            ) : (
                                                                <Lock className="h-4 w-4" />
                                                            )}
                                                        </Button>
                                                        <Button
                                                            variant="ghost"
                                                            size="icon"
                                                            className="h-8 w-8"
                                                            onClick={() => setResetPasswordId(user.id)}
                                                            title="Đặt lại mật khẩu"
                                                        >
                                                            <Key className="h-4 w-4" />
                                                        </Button>
                                                        <Button
                                                            variant="ghost"
                                                            size="icon"
                                                            className="h-8 w-8"
                                                            onClick={() => openEditForm(user)}
                                                        >
                                                            <Edit className="h-4 w-4" />
                                                        </Button>
                                                        <Button
                                                            variant="ghost"
                                                            size="icon"
                                                            className="h-8 w-8"
                                                            onClick={() => handleDelete(user.id, user.email)}
                                                        >
                                                            <Trash2 className="h-4 w-4 text-destructive" />
                                                        </Button>
                                                    </div>
                                                </div>
                                            );
                                        })}
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

                {/* Form */}
                <div>
                    {isFormOpen ? (
                        <Card>
                            <CardHeader className="flex flex-row items-center justify-between py-3">
                                <CardTitle className="text-lg">
                                    {editingId ? 'Sửa người dùng' : 'Thêm người dùng'}
                                </CardTitle>
                                <Button variant="ghost" size="icon" onClick={closeForm}>
                                    <X className="h-4 w-4" />
                                </Button>
                            </CardHeader>
                            <CardContent>
                                <form onSubmit={handleSubmit} className="space-y-4">
                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Email *</label>
                                        <Input
                                            type="email"
                                            value={formData.email}
                                            onChange={(e) =>
                                                setFormData((prev) => ({ ...prev, email: e.target.value }))
                                            }
                                            placeholder="email@domain.com"
                                            required
                                            disabled={!!editingId}
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Họ tên</label>
                                        <Input
                                            value={formData.fullName}
                                            onChange={(e) =>
                                                setFormData((prev) => ({ ...prev, fullName: e.target.value }))
                                            }
                                            placeholder="Họ và tên"
                                        />
                                    </div>

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium">Số điện thoại</label>
                                        <Input
                                            value={formData.phoneNumber}
                                            onChange={(e) =>
                                                setFormData((prev) => ({
                                                    ...prev,
                                                    phoneNumber: e.target.value,
                                                }))
                                            }
                                            placeholder="0123456789"
                                        />
                                    </div>

                                    {!editingId && (
                                        <div className="space-y-2">
                                            <label className="text-sm font-medium">Mật khẩu *</label>
                                            <Input
                                                type="password"
                                                value={formData.password}
                                                onChange={(e) =>
                                                    setFormData((prev) => ({
                                                        ...prev,
                                                        password: e.target.value,
                                                    }))
                                                }
                                                placeholder="Mật khẩu"
                                                required={!editingId}
                                            />
                                        </div>
                                    )}

                                    <div className="space-y-2">
                                        <label className="text-sm font-medium flex items-center gap-2">
                                            <Shield className="h-4 w-4" />
                                            Vai trò
                                        </label>
                                        <div className="border rounded-md p-3 space-y-2">
                                            {roles.map((role) => (
                                                <div key={role.id} className="flex items-center gap-2">
                                                    <input
                                                        type="checkbox"
                                                        id={`role-${role.id}`}
                                                        checked={formData.roles.includes(role.name)}
                                                        onChange={() => handleRoleToggle(role.name)}
                                                        className="rounded"
                                                    />
                                                    <label htmlFor={`role-${role.id}`} className="text-sm">
                                                        {role.name}
                                                    </label>
                                                </div>
                                            ))}
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
                                <Users className="h-12 w-12 mx-auto mb-3 opacity-50" />
                                <p>Chọn một người dùng để sửa</p>
                                <p className="text-sm">hoặc thêm người dùng mới</p>
                                <Button className="mt-4" variant="outline" onClick={openCreateForm}>
                                    <Plus className="h-4 w-4 mr-2" />
                                    Thêm người dùng
                                </Button>
                            </CardContent>
                        </Card>
                    )}
                </div>
            </div>

            {/* Reset Password Modal */}
            {resetPasswordId && (
                <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
                    <Card className="w-full max-w-sm mx-4">
                        <CardHeader className="flex flex-row items-center justify-between">
                            <CardTitle className="text-lg">Đặt lại mật khẩu</CardTitle>
                            <Button
                                variant="ghost"
                                size="icon"
                                onClick={() => {
                                    setResetPasswordId(null);
                                    setNewPassword('');
                                }}
                            >
                                <X className="h-4 w-4" />
                            </Button>
                        </CardHeader>
                        <CardContent>
                            <form
                                onSubmit={(e) => {
                                    e.preventDefault();
                                    resetPasswordMutation.mutate({ id: resetPasswordId, newPassword });
                                }}
                                className="space-y-4"
                            >
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Mật khẩu mới *</label>
                                    <Input
                                        type="password"
                                        value={newPassword}
                                        onChange={(e) => setNewPassword(e.target.value)}
                                        placeholder="Nhập mật khẩu mới"
                                        required
                                        minLength={6}
                                    />
                                </div>
                                <div className="flex gap-2">
                                    <Button
                                        type="button"
                                        variant="outline"
                                        className="flex-1"
                                        onClick={() => {
                                            setResetPasswordId(null);
                                            setNewPassword('');
                                        }}
                                    >
                                        Hủy
                                    </Button>
                                    <Button
                                        type="submit"
                                        className="flex-1"
                                        disabled={resetPasswordMutation.isPending}
                                    >
                                        {resetPasswordMutation.isPending && (
                                            <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                                        )}
                                        Đặt lại
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

