import { useState, useRef, useCallback } from 'react';
import { useMutation } from '@tanstack/react-query';
import { filesApi } from '../../api/client';
import { Button } from './button';
import { Upload, X, Loader2, ImageIcon } from 'lucide-react';
import { cn } from '../../lib/utils';

interface ImageUploaderProps {
    value?: string;
    onChange: (url: string | undefined) => void;
    folder?: string;
    className?: string;
    disabled?: boolean;
    generateVariants?: boolean;
}

interface UploadResponse {
    url?: string;
    objectPath?: string;
    contentType?: string;
    size?: number;
    smallUrl?: string;
    thumbUrl?: string;
}

export function ImageUploader({
    value,
    onChange,
    folder = 'data/images',
    className,
    disabled = false,
    generateVariants = false,
}: ImageUploaderProps) {
    const [isDragging, setIsDragging] = useState(false);
    const [previewUrl, setPreviewUrl] = useState<string | undefined>(value);
    const fileInputRef = useRef<HTMLInputElement>(null);

    const uploadMutation = useMutation({
        mutationFn: async (file: File) => {
            const formData = new FormData();
            formData.append('file', file);
            formData.append('folder', folder);
            if (generateVariants) {
                formData.append('generateVariants', 'true');
            }
            const response = await filesApi.uploadImage(file, folder);
            return response.data.data as UploadResponse;
        },
        onSuccess: (data) => {
            if (data?.url) {
                setPreviewUrl(data.url);
                onChange(data.url);
            }
        },
        onError: (error) => {
            console.error('Upload failed:', error);
            alert('Tải ảnh lên thất bại. Vui lòng thử lại.');
        },
    });

    const handleFileSelect = useCallback(
        (file: File) => {
            if (!file.type.startsWith('image/')) {
                alert('Vui lòng chọn file ảnh.');
                return;
            }
            if (file.size > 10 * 1024 * 1024) {
                alert('Kích thước file không được vượt quá 10MB.');
                return;
            }
            uploadMutation.mutate(file);
        },
        [uploadMutation]
    );

    const handleDrop = useCallback(
        (e: React.DragEvent<HTMLDivElement>) => {
            e.preventDefault();
            setIsDragging(false);
            if (disabled) return;

            const file = e.dataTransfer.files[0];
            if (file) {
                handleFileSelect(file);
            }
        },
        [disabled, handleFileSelect]
    );

    const handleDragOver = useCallback((e: React.DragEvent<HTMLDivElement>) => {
        e.preventDefault();
        setIsDragging(true);
    }, []);

    const handleDragLeave = useCallback((e: React.DragEvent<HTMLDivElement>) => {
        e.preventDefault();
        setIsDragging(false);
    }, []);

    const handleInputChange = useCallback(
        (e: React.ChangeEvent<HTMLInputElement>) => {
            const file = e.target.files?.[0];
            if (file) {
                handleFileSelect(file);
            }
        },
        [handleFileSelect]
    );

    const handleRemove = useCallback(() => {
        setPreviewUrl(undefined);
        onChange(undefined);
        if (fileInputRef.current) {
            fileInputRef.current.value = '';
        }
    }, [onChange]);

    const handleClick = useCallback(() => {
        if (!disabled && !uploadMutation.isPending) {
            fileInputRef.current?.click();
        }
    }, [disabled, uploadMutation.isPending]);

    // Update preview when external value changes
    if (value !== previewUrl && !uploadMutation.isPending) {
        setPreviewUrl(value);
    }

    return (
        <div className={cn('space-y-2', className)}>
            <input
                ref={fileInputRef}
                type="file"
                accept="image/*"
                onChange={handleInputChange}
                className="hidden"
                disabled={disabled || uploadMutation.isPending}
            />

            {previewUrl ? (
                <div className="relative group">
                    <img
                        src={previewUrl}
                        alt="Preview"
                        className="w-full h-48 object-cover rounded-lg border border-border"
                    />
                    {!disabled && (
                        <div className="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity rounded-lg flex items-center justify-center gap-2">
                            <Button
                                type="button"
                                variant="secondary"
                                size="sm"
                                onClick={handleClick}
                            >
                                <Upload className="h-4 w-4 mr-1" />
                                Thay đổi
                            </Button>
                            <Button
                                type="button"
                                variant="destructive"
                                size="sm"
                                onClick={handleRemove}
                            >
                                <X className="h-4 w-4 mr-1" />
                                Xóa
                            </Button>
                        </div>
                    )}
                </div>
            ) : (
                <div
                    onClick={handleClick}
                    onDrop={handleDrop}
                    onDragOver={handleDragOver}
                    onDragLeave={handleDragLeave}
                    className={cn(
                        'flex flex-col items-center justify-center w-full h-48 border-2 border-dashed rounded-lg cursor-pointer transition-colors',
                        isDragging
                            ? 'border-primary bg-primary/5'
                            : 'border-border hover:border-primary/50 hover:bg-muted/50',
                        disabled && 'opacity-50 cursor-not-allowed',
                        uploadMutation.isPending && 'pointer-events-none'
                    )}
                >
                    {uploadMutation.isPending ? (
                        <>
                            <Loader2 className="h-10 w-10 text-muted-foreground animate-spin mb-2" />
                            <span className="text-sm text-muted-foreground">Đang tải lên...</span>
                        </>
                    ) : (
                        <>
                            <ImageIcon className="h-10 w-10 text-muted-foreground mb-2" />
                            <span className="text-sm text-muted-foreground">
                                Kéo thả ảnh vào đây hoặc click để chọn
                            </span>
                            <span className="text-xs text-muted-foreground mt-1">
                                PNG, JPG, GIF, WebP (tối đa 10MB)
                            </span>
                        </>
                    )}
                </div>
            )}
        </div>
    );
}



