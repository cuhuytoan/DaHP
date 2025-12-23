import { useState, useEffect } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { settingsApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Textarea } from '../../components/ui/textarea';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '../../components/ui/card';
import { ImageUploader } from '../../components/ui/image-uploader';
import { Settings, Save, Loader2, Globe, Mail, Phone, MapPin, Facebook, Youtube } from 'lucide-react';

interface SettingsData {
    siteName?: string;
    siteDescription?: string;
    siteLogo?: string;
    siteFavicon?: string;
    siteEmail?: string;
    sitePhone?: string;
    siteHotline?: string;
    siteAddress?: string;
    siteFacebook?: string;
    siteYoutube?: string;
    siteZalo?: string;
    metaTitle?: string;
    metaDescription?: string;
    metaKeywords?: string;
    footerContent?: string;
    googleAnalyticsId?: string;
    facebookPixelId?: string;
}

export function SettingsPage() {
    const queryClient = useQueryClient();
    const [formData, setFormData] = useState<SettingsData>({});
    const [hasChanges, setHasChanges] = useState(false);

    const { data, isLoading } = useQuery({
        queryKey: ['settings'],
        queryFn: () => settingsApi.get(),
    });

    const updateMutation = useMutation({
        mutationFn: (data: SettingsData) => settingsApi.update(data as unknown as Record<string, unknown>),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['settings'] });
            setHasChanges(false);
            alert('Cập nhật cài đặt thành công!');
        },
    });

    useEffect(() => {
        if (data?.data?.data) {
            setFormData(data.data.data);
        }
    }, [data]);

    const handleChange = (field: keyof SettingsData, value: string) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
        setHasChanges(true);
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        updateMutation.mutate(formData);
    };

    if (isLoading) {
        return (
            <div className="flex items-center justify-center h-64">
                <Loader2 className="h-8 w-8 animate-spin text-primary" />
            </div>
        );
    }

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight flex items-center gap-2">
                        <Settings className="h-8 w-8" />
                        Cài đặt
                    </h1>
                    <p className="text-muted-foreground">Cấu hình website</p>
                </div>
                <Button onClick={handleSubmit} disabled={!hasChanges || updateMutation.isPending}>
                    {updateMutation.isPending ? (
                        <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                    ) : (
                        <Save className="h-4 w-4 mr-2" />
                    )}
                    Lưu thay đổi
                </Button>
            </div>

            <form onSubmit={handleSubmit}>
                <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                    {/* General Settings */}
                    <Card>
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <Globe className="h-5 w-5" />
                                Thông tin chung
                            </CardTitle>
                            <CardDescription>Thông tin cơ bản về website</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="space-y-2">
                                <label className="text-sm font-medium">Tên website</label>
                                <Input
                                    value={formData.siteName || ''}
                                    onChange={(e) => handleChange('siteName', e.target.value)}
                                    placeholder="Tên website"
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-medium">Mô tả website</label>
                                <Textarea
                                    value={formData.siteDescription || ''}
                                    onChange={(e) => handleChange('siteDescription', e.target.value)}
                                    placeholder="Mô tả ngắn về website"
                                    rows={3}
                                />
                            </div>

                            <div className="grid grid-cols-2 gap-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Logo</label>
                                    <ImageUploader
                                        value={formData.siteLogo}
                                        onChange={(url) => handleChange('siteLogo', url || '')}
                                        folder="data/settings"
                                    />
                                </div>
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Favicon</label>
                                    <ImageUploader
                                        value={formData.siteFavicon}
                                        onChange={(url) => handleChange('siteFavicon', url || '')}
                                        folder="data/settings"
                                    />
                                </div>
                            </div>
                        </CardContent>
                    </Card>

                    {/* Contact Settings */}
                    <Card>
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <Phone className="h-5 w-5" />
                                Thông tin liên hệ
                            </CardTitle>
                            <CardDescription>Thông tin liên lạc hiển thị trên website</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="space-y-2">
                                <label className="text-sm font-medium flex items-center gap-2">
                                    <Mail className="h-4 w-4" />
                                    Email
                                </label>
                                <Input
                                    type="email"
                                    value={formData.siteEmail || ''}
                                    onChange={(e) => handleChange('siteEmail', e.target.value)}
                                    placeholder="contact@domain.com"
                                />
                            </div>

                            <div className="grid grid-cols-2 gap-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Điện thoại</label>
                                    <Input
                                        value={formData.sitePhone || ''}
                                        onChange={(e) => handleChange('sitePhone', e.target.value)}
                                        placeholder="0123 456 789"
                                    />
                                </div>
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Hotline</label>
                                    <Input
                                        value={formData.siteHotline || ''}
                                        onChange={(e) => handleChange('siteHotline', e.target.value)}
                                        placeholder="1900 xxxx"
                                    />
                                </div>
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-medium flex items-center gap-2">
                                    <MapPin className="h-4 w-4" />
                                    Địa chỉ
                                </label>
                                <Textarea
                                    value={formData.siteAddress || ''}
                                    onChange={(e) => handleChange('siteAddress', e.target.value)}
                                    placeholder="Địa chỉ văn phòng"
                                    rows={2}
                                />
                            </div>
                        </CardContent>
                    </Card>

                    {/* Social Media */}
                    <Card>
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <Facebook className="h-5 w-5" />
                                Mạng xã hội
                            </CardTitle>
                            <CardDescription>Liên kết đến các trang mạng xã hội</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="space-y-2">
                                <label className="text-sm font-medium flex items-center gap-2">
                                    <Facebook className="h-4 w-4 text-blue-600" />
                                    Facebook
                                </label>
                                <Input
                                    value={formData.siteFacebook || ''}
                                    onChange={(e) => handleChange('siteFacebook', e.target.value)}
                                    placeholder="https://facebook.com/..."
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-medium flex items-center gap-2">
                                    <Youtube className="h-4 w-4 text-red-600" />
                                    Youtube
                                </label>
                                <Input
                                    value={formData.siteYoutube || ''}
                                    onChange={(e) => handleChange('siteYoutube', e.target.value)}
                                    placeholder="https://youtube.com/..."
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-medium">Zalo</label>
                                <Input
                                    value={formData.siteZalo || ''}
                                    onChange={(e) => handleChange('siteZalo', e.target.value)}
                                    placeholder="Số Zalo hoặc link"
                                />
                            </div>
                        </CardContent>
                    </Card>

                    {/* SEO Settings */}
                    <Card>
                        <CardHeader>
                            <CardTitle>SEO & Analytics</CardTitle>
                            <CardDescription>Tối ưu hóa công cụ tìm kiếm</CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="space-y-2">
                                <label className="text-sm font-medium">Meta Title</label>
                                <Input
                                    value={formData.metaTitle || ''}
                                    onChange={(e) => handleChange('metaTitle', e.target.value)}
                                    placeholder="Tiêu đề SEO"
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-medium">Meta Description</label>
                                <Textarea
                                    value={formData.metaDescription || ''}
                                    onChange={(e) => handleChange('metaDescription', e.target.value)}
                                    placeholder="Mô tả SEO (150-160 ký tự)"
                                    rows={2}
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-medium">Meta Keywords</label>
                                <Input
                                    value={formData.metaKeywords || ''}
                                    onChange={(e) => handleChange('metaKeywords', e.target.value)}
                                    placeholder="từ khóa 1, từ khóa 2, ..."
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-medium">Google Analytics ID</label>
                                <Input
                                    value={formData.googleAnalyticsId || ''}
                                    onChange={(e) => handleChange('googleAnalyticsId', e.target.value)}
                                    placeholder="G-XXXXXXXXXX"
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="text-sm font-medium">Facebook Pixel ID</label>
                                <Input
                                    value={formData.facebookPixelId || ''}
                                    onChange={(e) => handleChange('facebookPixelId', e.target.value)}
                                    placeholder="XXXXXXXXXXXXXXX"
                                />
                            </div>
                        </CardContent>
                    </Card>

                    {/* Footer Content */}
                    <Card className="lg:col-span-2">
                        <CardHeader>
                            <CardTitle>Nội dung Footer</CardTitle>
                            <CardDescription>HTML/Text hiển thị ở cuối trang</CardDescription>
                        </CardHeader>
                        <CardContent>
                            <Textarea
                                value={formData.footerContent || ''}
                                onChange={(e) => handleChange('footerContent', e.target.value)}
                                placeholder="Nội dung footer (hỗ trợ HTML)"
                                rows={5}
                            />
                        </CardContent>
                    </Card>
                </div>
            </form>
        </div>
    );
}

