"use client";

import { useRouter, useSearchParams } from "next/navigation";
import { useState, useCallback } from "react";
import { Search, X, SlidersHorizontal, ChevronDown } from "lucide-react";
import type { Category } from "@/lib/api";

interface ProductFiltersProps {
  categories: Category[];
}

const priceRanges = [
  { label: "Tất cả mức giá", min: undefined, max: undefined },
  { label: "Dưới 10 triệu", min: undefined, max: 10000000 },
  { label: "10 - 50 triệu", min: 10000000, max: 50000000 },
  { label: "50 - 100 triệu", min: 50000000, max: 100000000 },
  { label: "100 - 500 triệu", min: 100000000, max: 500000000 },
  { label: "Trên 500 triệu", min: 500000000, max: undefined },
];

export function ProductFilters({ categories }: ProductFiltersProps) {
  const router = useRouter();
  const searchParams = useSearchParams();
  const [search, setSearch] = useState(searchParams.get("keyword") || "");
  const [isExpanded, setIsExpanded] = useState(true);

  const currentCategoryId = searchParams.get("categoryId");
  const currentMinPrice = searchParams.get("minPrice");
  const currentMaxPrice = searchParams.get("maxPrice");

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
    router.push(`/san-pham?${params.toString()}`);
  }, [router, searchParams]);

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    updateFilters({ keyword: search || null });
  };

  const handlePriceRange = (min?: number, max?: number) => {
    updateFilters({
      minPrice: min?.toString() || null,
      maxPrice: max?.toString() || null,
    });
  };

  const clearFilters = () => {
    setSearch("");
    router.push("/san-pham");
  };

  const hasActiveFilters = currentCategoryId || searchParams.get("keyword") || currentMinPrice || currentMaxPrice;

  const getCurrentPriceLabel = () => {
    const range = priceRanges.find(
      r => r.min?.toString() === currentMinPrice && r.max?.toString() === currentMaxPrice
    );
    return range?.label || "Tất cả mức giá";
  };

  return (
    <div className="bg-white rounded-2xl shadow-lg overflow-hidden">
      {/* Header */}
      <div
        className="flex items-center justify-between p-5 bg-gradient-to-r from-[#2D2D2D] to-[#4A4A4A] text-white cursor-pointer"
        onClick={() => setIsExpanded(!isExpanded)}
      >
        <div className="flex items-center gap-2">
          <SlidersHorizontal className="w-5 h-5" />
          <span className="font-semibold">Bộ lọc sản phẩm</span>
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
                placeholder="Nhập tên sản phẩm..."
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

          {/* Categories */}
          <div>
            <h3 className="font-semibold text-[#2D2D2D] mb-3">Danh mục sản phẩm</h3>
            <div className="space-y-1">
              <button
                onClick={() => updateFilters({ categoryId: null })}
                className={`w-full text-left px-4 py-2.5 rounded-xl transition-all duration-200 ${!currentCategoryId
                    ? "bg-gradient-to-r from-[#C9A227] to-[#D4AF37] text-white font-medium shadow-md"
                    : "hover:bg-[#F9F7F4] text-[#6B6B6B] hover:text-[#2D2D2D]"
                  }`}
              >
                Tất cả sản phẩm
              </button>
              {categories.map((category) => (
                <button
                  key={category.id}
                  onClick={() => updateFilters({ categoryId: category.id.toString() })}
                  className={`w-full text-left px-4 py-2.5 rounded-xl transition-all duration-200 flex items-center justify-between ${currentCategoryId === category.id.toString()
                      ? "bg-gradient-to-r from-[#C9A227] to-[#D4AF37] text-white font-medium shadow-md"
                      : "hover:bg-[#F9F7F4] text-[#6B6B6B] hover:text-[#2D2D2D]"
                    }`}
                >
                  <span>{category.name}</span>
                  {category.productCount !== undefined && (
                    <span className={`text-xs px-2 py-0.5 rounded-full ${currentCategoryId === category.id.toString()
                        ? "bg-white/20"
                        : "bg-gray-100"
                      }`}>
                      {category.productCount}
                    </span>
                  )}
                </button>
              ))}
            </div>
          </div>

          {/* Divider */}
          <div className="h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent" />

          {/* Price Range */}
          <div>
            <h3 className="font-semibold text-[#2D2D2D] mb-3">Khoảng giá</h3>
            <div className="space-y-1">
              {priceRanges.map((range, index) => {
                const isActive =
                  range.min?.toString() === currentMinPrice &&
                  range.max?.toString() === currentMaxPrice ||
                  (!range.min && !range.max && !currentMinPrice && !currentMaxPrice);

                return (
                  <button
                    key={index}
                    onClick={() => handlePriceRange(range.min, range.max)}
                    className={`w-full text-left px-4 py-2.5 rounded-xl transition-all duration-200 ${isActive
                        ? "bg-gradient-to-r from-[#C9A227] to-[#D4AF37] text-white font-medium shadow-md"
                        : "hover:bg-[#F9F7F4] text-[#6B6B6B] hover:text-[#2D2D2D]"
                      }`}
                  >
                    {range.label}
                  </button>
                );
              })}
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
            {currentCategoryId && (
              <span className="inline-flex items-center gap-1 px-3 py-1 bg-[#C9A227]/10 text-[#C9A227] rounded-full text-sm">
                {categories.find(c => c.id.toString() === currentCategoryId)?.name}
              </span>
            )}
            {(currentMinPrice || currentMaxPrice) && (
              <span className="inline-flex items-center gap-1 px-3 py-1 bg-[#C9A227]/10 text-[#C9A227] rounded-full text-sm">
                {getCurrentPriceLabel()}
              </span>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
