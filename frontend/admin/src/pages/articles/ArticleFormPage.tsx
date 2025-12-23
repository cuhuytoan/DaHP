import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { articlesApi } from '../../api/client';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Card, CardContent, CardHeader, CardTitle } from '../../components/ui/card';
import { ArrowLeft, Save, Loader2 } from 'lucide-react';

export function ArticleFormPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const queryClient = useQueryClient();
    const isEdit = !!id;

    const [formData, setFormData] = useState({
        name: '',
        description: '',
        content: '',
        typeId: '',
    });

    const { data: articleData, isLoading: isArticleLoading } = useQuery({
        queryKey: ['article', id],
        queryFn: () => articlesApi.getById(Number(id)),
        enabled: isEdit,
    });

    useEffect(() => {
        if (articleData?.data?.data) {
            const a = articleData.data.data;
            setFormData({
                name: a.name || '',
                description: a.description || '',
                content: a.content || '',
                typeId: a.typeId?.toString() || '',
            });
        }
    }, [articleData]);

    const mutation = useMutation({
        mutationFn: (data: FormData) =>
            isEdit ? articlesApi.update(Number(id), data) : articlesApi.create(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['articles'] });
            navigate('/articles');
        },
    });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        const data = new FormData();
        Object.entries(formData).forEach(([key, value]) => {
            data.append(key, value);
        });
        mutation.mutate(data);
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    if (isEdit && isArticleLoading) {
        return <div className="flex justify-center p-12"><Loader2 className="animate-spin" /></div>;
    }

    return (
        <div className="space-y-6">
            <div className="flex items-center gap-4">
                <Button variant="ghost" size="icon" onClick={() => navigate('/articles')}>
                    <ArrowLeft className="h-4 w-4" />
                </Button>
                <h1 className="text-3xl font-bold tracking-tight">
                    {isEdit ? 'Chỉnh sửa bài viết' : 'Thêm bài viết mới'}
                </h1>
            </div>

            <form onSubmit={handleSubmit}>
                <Card>
                    <CardHeader>
                        <CardTitle>Nội dung bài viết</CardTitle>
                    </CardHeader>
                    <CardContent className="space-y-4">
                        <div className="space-y-2">
                            <label className="text-sm font-medium">Tiêu đề</label>
                            <Input name="name" value={formData.name} onChange={handleChange} required />
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <label className="text-sm font-medium">Loại bài viết ID</label>
                                <Input name="typeId" type="number" value={formData.typeId} onChange={handleChange} />
                            </div>
                        </div>

                        <div className="space-y-2">
                            <label className="text-sm font-medium">Mô tả ngắn</label>
                            <textarea
                                name="description"
                                value={formData.description}
                                onChange={handleChange}
                                className="flex min-h-[80px] w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
                            />
                        </div>

                        <div className="space-y-2">
                            <label className="text-sm font-medium">Nội dung chi tiết</label>
                            <textarea
                                name="content"
                                value={formData.content}
                                onChange={handleChange}
                                className="flex min-h-[300px] w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
                            />
                        </div>

                        <div className="flex justify-end gap-4">
                            <Button type="button" variant="outline" onClick={() => navigate('/articles')}> Hủy </Button>
                            <Button type="submit" disabled={mutation.isPending}>
                                {mutation.isPending ? <Loader2 className="mr-2 h-4 w-4 animate-spin" /> : <Save className="mr-2 h-4 w-4" />}
                                Lưu bài viết
                            </Button>
                        </div>
                    </CardContent>
                </Card>
            </form>
        </div>
    );
}
