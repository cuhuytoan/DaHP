"use client";

import { useRouter, useSearchParams } from "next/navigation";
import { useState, useCallback } from "react";
import { Search, X, SlidersHorizontal, ChevronDown, Tag } from "lucide-react";

interface ArticleType {
    id: number;
    name: string;
    count?: number;
}

interface ArticleFiltersProps {
    types: ArticleType[];
}

export function ArticleFilters({ types }: ArticleFiltersProps) {
    const router = useRouter();
    const searchParams = useSearchParams();
    const [search, setSearch] = useState(searchParams.get("keyword") || "");
    const [isExpanded, setIsExpanded] = useState(true);

    const currentTypeId = searchParams.get("typeId");

    const updateFilters = useCallback((updates: Record<string, string | null>) => {
        const params = new URLSearchParams(searchParams.toString());

        Object.entries(updates).forEach(([key, value]) => {
            if (value !== null && value !== undefined && value !== "") {
                params.set(key, value);
            } else {
                params.delete(key);
            }
        });

        params.delete("page"); // Reset page when filters change
        router.push(`/tin-tuc?${params.toString()}`);
    }, [router, searchParams]);

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        updateFilters({ keyword: search || null });
    };

    const clearFilters = () => {
        setSearch("");
        router.push("/tin-tuc");
    };

    const hasActiveFilters = currentTypeId || searchParams.get("keyword");

    return (
        <div className="bg-white rounded-2xl shadow-lg overflow-hidden">
            {/* Header */}
            <div
                className="flex items-center justify-between p-5 bg-gradient-to-r from-[#2D2D2D] to-[#4A4A4A] text-white cursor-pointer"
                onClick={() => setIsExpanded(!isExpanded)}
            >
                <div className="flex items-center gap-2">
                    <SlidersHorizontal className="w-5 h-5" />
                    <span className="font-semibold">Bộ lọc bài viết</span>
                </div>
                <ChevronDown className={`w-5 h-5 transition-transform ${isExpanded ? "rotate-180" : ""}`} />
            </div>

            <div className={`transition-all duration-300 ${isExpanded ? "max-h-[1000px] opacity-100" : "max-h-0 opacity-0 overflow-hidden"}`}>
                <div className="p-5 space-y-6">
                    {/* Search */}
                    <div>
                        <h3 className="font-semibold text-[#2D2D2D] mb-3 flex items-center gap-2">
                            <Search className="w-4 h-4 text-[#C9A227]" />
                            Tìm kiếm
                        </h3>
                        <form onSubmit={handleSearch} className="relative">
                            <input
                                type="text"
                                placeholder="Nhập từ khóa..."
                                value={search}
                                onChange={(e) => setSearch(e.target.value)}
                                className="w-full pl-4 pr-12 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-[#C9A227]/50 focus:border-[#C9A227] transition-all"
                            />
                            <button
                                type="submit"
                                className="absolute right-2 top-1/2 -translate-y-1/2 p-2 text-[#C9A227] hover:bg-[#C9A227]/10 rounded-lg transition-colors"
                            >
                                <Search className="w-4 h-4" />
                            </button>
                        </form>
                    </div>

                    {/* Divider */}
                    <div className="h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent" />

                    {/* Types/Categories */}
                    <div>
                        <h3 className="font-semibold text-[#2D2D2D] mb-3 flex items-center gap-2">
                            <Tag className="w-4 h-4 text-[#C9A227]" />
                            Chuyên mục
                        </h3>
                        <div className="space-y-1">
                            <button
                                onClick={() => updateFilters({ typeId: null })}
                                className={`w-full text-left px-4 py-2.5 rounded-xl transition-all duration-200 ${!currentTypeId
                                        ? "bg-gradient-to-r from-[#C9A227] to-[#D4AF37] text-white font-medium shadow-md"
                                        : "hover:bg-[#F9F7F4] text-[#6B6B6B] hover:text-[#2D2D2D]"
                                    }`}
                            >
                                Tất cả bài viết
                            </button>
                            {types.map((type) => (
                                <button
                                    key={type.id}
                                    onClick={() => updateFilters({ typeId: type.id.toString() })}
                                    className={`w-full text-left px-4 py-2.5 rounded-xl transition-all duration-200 flex items-center justify-between ${currentTypeId === type.id.toString()
                                            ? "bg-gradient-to-r from-[#C9A227] to-[#D4AF37] text-white font-medium shadow-md"
                                            : "hover:bg-[#F9F7F4] text-[#6B6B6B] hover:text-[#2D2D2D]"
                                        }`}
                                >
                                    <span>{type.name}</span>
                                    {type.count !== undefined && (
                                        <span className={`text-xs px-2 py-0.5 rounded-full ${currentTypeId === type.id.toString()
                                                ? "bg-white/20"
                                                : "bg-gray-100"
                                            }`}>
                                            {type.count}
                                        </span>
                                    )}
                                </button>
                            ))}
                        </div>
                    </div>

                    {/* Clear Filters */}
                    {hasActiveFilters && (
                        <>
                            <div className="h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent" />
                            <button
                                onClick={clearFilters}
                                className="w-full flex items-center justify-center gap-2 px-4 py-3 border-2 border-[#8B0000] text-[#8B0000] rounded-xl font-medium hover:bg-[#8B0000] hover:text-white transition-all duration-200"
                            >
                                <X className="w-4 h-4" />
                                Xóa bộ lọc
                            </button>
                        </>
                    )}
                </div>
            </div>

            {/* Active Filters Summary (collapsed state) */}
            {!isExpanded && hasActiveFilters && (
                <div className="px-5 py-3 bg-[#F9F7F4] border-t border-gray-100">
                    <div className="flex flex-wrap gap-2">
                        {searchParams.get("keyword") && (
                            <span className="inline-flex items-center gap-1 px-3 py-1 bg-[#C9A227]/10 text-[#C9A227] rounded-full text-sm">
                                &quot;{searchParams.get("keyword")}&quot;
                            </span>
                        )}
                        {currentTypeId && (
                            <span className="inline-flex items-center gap-1 px-3 py-1 bg-[#C9A227]/10 text-[#C9A227] rounded-full text-sm">
                                {types.find(t => t.id.toString() === currentTypeId)?.name}
                            </span>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
}
