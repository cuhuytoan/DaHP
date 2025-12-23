"use client";

import { useRouter, useSearchParams } from "next/navigation";
import { ChevronLeft, ChevronRight, MoreHorizontal } from "lucide-react";

interface ProductPaginationProps {
  currentPage: number;
  totalPages: number;
}

export function ProductPagination({ currentPage, totalPages }: ProductPaginationProps) {
  const router = useRouter();
  const searchParams = useSearchParams();

  const goToPage = (page: number) => {
    if (page < 1 || page > totalPages) return;

    const params = new URLSearchParams(searchParams.toString());
    if (page === 1) {
      params.delete("page");
    } else {
      params.set("page", page.toString());
    }
    router.push(`/san-pham?${params.toString()}`);
  };

  // Generate page numbers to display
  const getPageNumbers = () => {
    const pages: (number | "ellipsis")[] = [];
    const showPages = 5; // Number of pages to show

    if (totalPages <= showPages + 2) {
      // Show all pages if total is small
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      // Always show first page
      pages.push(1);

      if (currentPage <= 3) {
        // Near the start
        pages.push(2, 3, 4);
        pages.push("ellipsis");
      } else if (currentPage >= totalPages - 2) {
        // Near the end
        pages.push("ellipsis");
        pages.push(totalPages - 3, totalPages - 2, totalPages - 1);
      } else {
        // In the middle
        pages.push("ellipsis");
        pages.push(currentPage - 1, currentPage, currentPage + 1);
        pages.push("ellipsis");
      }

      // Always show last page
      pages.push(totalPages);
    }

    return pages;
  };

  const pageNumbers = getPageNumbers();

  return (
    <div className="flex items-center justify-center gap-2">
      {/* Previous Button */}
      <button
        onClick={() => goToPage(currentPage - 1)}
        disabled={currentPage === 1}
        className={`flex items-center gap-1 px-4 py-2.5 rounded-xl font-medium transition-all duration-200 ${currentPage === 1
            ? "bg-gray-100 text-gray-400 cursor-not-allowed"
            : "bg-white text-[#2D2D2D] hover:bg-[#C9A227] hover:text-white shadow-md hover:shadow-lg"
          }`}
      >
        <ChevronLeft className="w-4 h-4" />
        <span className="hidden sm:inline">Trước</span>
      </button>

      {/* Page Numbers */}
      <div className="flex items-center gap-1">
        {pageNumbers.map((page, index) => {
          if (page === "ellipsis") {
            return (
              <span key={`ellipsis-${index}`} className="w-10 h-10 flex items-center justify-center text-[#6B6B6B]">
                <MoreHorizontal className="w-4 h-4" />
              </span>
            );
          }

          const isActive = page === currentPage;
          return (
            <button
              key={page}
              onClick={() => goToPage(page)}
              className={`w-10 h-10 rounded-xl font-medium transition-all duration-200 ${isActive
                  ? "bg-gradient-to-r from-[#C9A227] to-[#D4AF37] text-white shadow-lg"
                  : "bg-white text-[#2D2D2D] hover:bg-[#F9F7F4] shadow-md"
                }`}
            >
              {page}
            </button>
          );
        })}
      </div>

      {/* Next Button */}
      <button
        onClick={() => goToPage(currentPage + 1)}
        disabled={currentPage === totalPages}
        className={`flex items-center gap-1 px-4 py-2.5 rounded-xl font-medium transition-all duration-200 ${currentPage === totalPages
            ? "bg-gray-100 text-gray-400 cursor-not-allowed"
            : "bg-white text-[#2D2D2D] hover:bg-[#C9A227] hover:text-white shadow-md hover:shadow-lg"
          }`}
      >
        <span className="hidden sm:inline">Tiếp</span>
        <ChevronRight className="w-4 h-4" />
      </button>
    </div>
  );
}
