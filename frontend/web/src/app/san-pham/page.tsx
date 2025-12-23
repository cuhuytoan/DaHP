import { Metadata } from "next";
import { Suspense } from "react";
import Link from "next/link";
import { ProductCard } from "@/components/product";
import { productsApi, categoriesApi, type Product, type Category } from "@/lib/api";
import { ProductFilters } from "./product-filters";
import { ProductPagination } from "./product-pagination";
import { ChevronRight, Package, Grid3X3, LayoutGrid, Sparkles } from "lucide-react";

export const metadata: Metadata = {
  title: "Sản phẩm Lăng Mộ Đá | Trường Thành",
  description: "Khám phá bộ sưu tập lăng mộ đá, mộ đá, cuốn thư đá cao cấp. Cam kết chất lượng, giá tốt nhất thị trường.",
  keywords: "lăng mộ đá, mộ đá, cuốn thư đá, đá mỹ nghệ, lăng mộ đá ninh bình",
};

// Mock data for fallback
const mockProducts: Product[] = Array.from({ length: 12 }, (_, i) => ({
  id: i + 1,
  name: `Lăng Mộ Đá Mẫu ${String.fromCharCode(65 + i)}`,
  code: `LMD-${String(i + 1).padStart(3, "0")}`,
  price: 50000000 + i * 10000000,
  priceOld: i % 3 === 0 ? 60000000 + i * 10000000 : undefined,
  image: undefined,
  categoryName: ["Lăng Mộ Đá", "Mộ Đá", "Cuốn Thư Đá"][i % 3],
  productBrandName: "Trường Thành",
  quantitySold: Math.floor(Math.random() * 50) + 10,
  averageRating: 4 + Math.random(),
  totalReview: Math.floor(Math.random() * 100) + 5,
  active: true,
}));

const mockCategories: Category[] = [
  { id: 1, name: "Lăng Mộ Đá", productCount: 150 },
  { id: 2, name: "Mộ Đá", productCount: 200 },
  { id: 3, name: "Mộ Tháp", productCount: 80 },
  { id: 4, name: "Bàn Thờ Đá", productCount: 120 },
  { id: 5, name: "Tượng Đá", productCount: 90 },
  { id: 6, name: "Cuốn Thư Đá", productCount: 60 },
];

interface SearchParams {
  page?: string;
  categoryId?: string;
  brandId?: string;
  keyword?: string;
  minPrice?: string;
  maxPrice?: string;
  sort?: string;
}

async function getProducts(params: SearchParams) {
  try {
    const response = await productsApi.getAll({
      page: params.page ? parseInt(params.page) : 1,
      pageSize: 12,
      productCategoryId: params.categoryId ? parseInt(params.categoryId) : undefined,
      productBrandId: params.brandId ? parseInt(params.brandId) : undefined,
      keyword: params.keyword,
      minPrice: params.minPrice ? parseInt(params.minPrice) : undefined,
      maxPrice: params.maxPrice ? parseInt(params.maxPrice) : undefined,
      active: true,
    });
    return {
      products: response.data?.length > 0 ? response.data : mockProducts,
      total: response.pagination?.totalCount || mockProducts.length,
      totalPages: response.pagination?.totalPages || 1,
    };
  } catch {
    return {
      products: mockProducts,
      total: mockProducts.length,
      totalPages: 1,
    };
  }
}

async function getCategories() {
  try {
    const response = await categoriesApi.getAll();
    return response.data?.length > 0 ? response.data : mockCategories;
  } catch {
    return mockCategories;
  }
}

// Loading skeleton
function ProductGridSkeleton() {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      {Array.from({ length: 6 }).map((_, i) => (
        <div key={i} className="bg-white rounded-2xl overflow-hidden shadow-lg animate-pulse">
          <div className="aspect-[4/3] bg-gray-200" />
          <div className="p-5 space-y-3">
            <div className="h-3 bg-gray-200 rounded w-1/4" />
            <div className="h-5 bg-gray-200 rounded w-3/4" />
            <div className="h-5 bg-gray-200 rounded w-1/2" />
            <div className="h-6 bg-gray-200 rounded w-2/5" />
          </div>
        </div>
      ))}
    </div>
  );
}

// Sort options component
function SortSelect({ currentSort }: { currentSort?: string }) {
  return (
    <div className="relative">
      <select
        defaultValue={currentSort || "newest"}
        className="appearance-none bg-white border border-gray-200 rounded-xl px-4 py-2.5 pr-10 text-sm font-medium text-[#2D2D2D] focus:outline-none focus:ring-2 focus:ring-[#C9A227]/50 focus:border-[#C9A227] cursor-pointer shadow-sm hover:shadow-md transition-all"
      >
        <option value="newest">Mới nhất</option>
        <option value="price-asc">Giá: Thấp → Cao</option>
        <option value="price-desc">Giá: Cao → Thấp</option>
        <option value="popular">Bán chạy nhất</option>
        <option value="rating">Đánh giá cao</option>
      </select>
      <ChevronRight className="absolute right-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400 rotate-90 pointer-events-none" />
    </div>
  );
}

// View toggle component
function ViewToggle() {
  return (
    <div className="hidden md:flex items-center gap-1 bg-white rounded-xl p-1 shadow-sm">
      <button className="p-2 rounded-lg bg-[#C9A227] text-white">
        <Grid3X3 className="w-4 h-4" />
      </button>
      <button className="p-2 rounded-lg text-gray-400 hover:text-[#2D2D2D] transition-colors">
        <LayoutGrid className="w-4 h-4" />
      </button>
    </div>
  );
}

export default async function ProductsPage({
  searchParams,
}: {
  searchParams: Promise<SearchParams>;
}) {
  const params = await searchParams;
  const [{ products, total, totalPages }, categories] = await Promise.all([
    getProducts(params),
    getCategories(),
  ]);

  const currentPage = params.page ? parseInt(params.page) : 1;
  const currentCategory = params.categoryId
    ? categories.find((c) => c.id.toString() === params.categoryId)
    : null;

  return (
    <div className="min-h-screen bg-[#F9F7F4]">
      {/* Hero Banner */}
      <section className="relative bg-gradient-to-r from-[#2D2D2D] to-[#4A4A4A] py-16 overflow-hidden">
        {/* Decorative pattern */}
        <div className="absolute inset-0 opacity-10">
          <div className="absolute inset-0" style={{
            backgroundImage: `url("data:image/svg+xml,%3Csvg width='60' height='60' viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'%3E%3Cg fill='none' fill-rule='evenodd'%3E%3Cg fill='%23C9A227' fill-opacity='0.4'%3E%3Cpath d='M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E")`,
          }} />
        </div>

        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative">
          {/* Breadcrumb */}
          <nav className="flex items-center gap-2 text-sm text-white/60 mb-6">
            <Link href="/" className="hover:text-white transition-colors">Trang chủ</Link>
            <ChevronRight className="w-4 h-4" />
            <span className="text-[#C9A227]">Sản phẩm</span>
            {currentCategory && (
              <>
                <ChevronRight className="w-4 h-4" />
                <span className="text-[#C9A227]">{currentCategory.name}</span>
              </>
            )}
          </nav>

          <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4">
            <div>
              <div className="flex items-center gap-3 mb-2">
                <div className="w-12 h-12 bg-[#C9A227] rounded-xl flex items-center justify-center">
                  <Package className="w-6 h-6 text-white" />
                </div>
                <h1 className="text-3xl md:text-4xl font-bold text-white">
                  {currentCategory ? currentCategory.name : "Tất Cả Sản Phẩm"}
                </h1>
              </div>
              <p className="text-white/70 max-w-xl">
                Khám phá bộ sưu tập đá mỹ nghệ cao cấp, được chế tác tỉ mỉ bởi các nghệ nhân hàng đầu Việt Nam.
              </p>
            </div>

            {/* Stats */}
            <div className="flex items-center gap-6">
              <div className="text-center">
                <p className="text-2xl font-bold text-[#C9A227]">{total}</p>
                <p className="text-sm text-white/60">Sản phẩm</p>
              </div>
              <div className="w-px h-12 bg-white/20" />
              <div className="text-center">
                <p className="text-2xl font-bold text-[#C9A227]">{categories.length}</p>
                <p className="text-sm text-white/60">Danh mục</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Main Content */}
      <section className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10">
        <div className="grid lg:grid-cols-4 gap-8">
          {/* Sidebar Filters */}
          <aside className="lg:col-span-1">
            <div className="sticky top-24">
              <Suspense fallback={<div className="bg-white rounded-2xl h-96 animate-pulse" />}>
                <ProductFilters categories={categories} />
              </Suspense>

              {/* Featured Products - Sidebar */}
              <div className="mt-6 bg-white rounded-2xl shadow-lg p-5">
                <div className="flex items-center gap-2 mb-4">
                  <Sparkles className="w-5 h-5 text-[#C9A227]" />
                  <h3 className="font-semibold text-[#2D2D2D]">Sản phẩm nổi bật</h3>
                </div>
                <div className="space-y-3">
                  {products.slice(0, 3).map((product) => (
                    <ProductCard key={`featured-${product.id}`} product={product} variant="compact" />
                  ))}
                </div>
              </div>
            </div>
          </aside>

          {/* Products Grid */}
          <div className="lg:col-span-3">
            {/* Toolbar */}
            <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-6 bg-white rounded-xl p-4 shadow-sm">
              <div className="flex items-center gap-4">
                <p className="text-sm text-[#6B6B6B]">
                  Hiển thị{" "}
                  <span className="font-semibold text-[#2D2D2D]">
                    {(currentPage - 1) * 12 + 1}-{Math.min(currentPage * 12, total)}
                  </span>{" "}
                  trong <span className="font-semibold text-[#2D2D2D]">{total}</span> sản phẩm
                </p>
              </div>

              <div className="flex items-center gap-3">
                <SortSelect currentSort={params.sort} />
                <ViewToggle />
              </div>
            </div>

            {/* Products */}
            <Suspense fallback={<ProductGridSkeleton />}>
              {products.length > 0 ? (
                <>
                  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                    {products.map((product) => (
                      <ProductCard key={product.id} product={product} />
                    ))}
                  </div>

                  {/* Pagination */}
                  {totalPages > 1 && (
                    <div className="mt-10">
                      <ProductPagination
                        currentPage={currentPage}
                        totalPages={totalPages}
                      />
                    </div>
                  )}
                </>
              ) : (
                <div className="text-center py-20 bg-white rounded-2xl shadow-lg">
                  <div className="w-24 h-24 bg-[#F9F7F4] rounded-full flex items-center justify-center mx-auto mb-6">
                    <Package className="w-12 h-12 text-[#C9A227]" />
                  </div>
                  <h3 className="text-xl font-semibold text-[#2D2D2D] mb-2">
                    Không tìm thấy sản phẩm
                  </h3>
                  <p className="text-[#6B6B6B] mb-6">
                    Thử thay đổi từ khóa tìm kiếm hoặc bộ lọc khác
                  </p>
                  <Link
                    href="/san-pham"
                    className="inline-flex items-center gap-2 px-6 py-3 bg-gradient-to-r from-[#C9A227] to-[#D4AF37] text-white rounded-xl font-medium hover:shadow-lg transition-all"
                  >
                    Xem tất cả sản phẩm
                    <ChevronRight className="w-4 h-4" />
                  </Link>
                </div>
              )}
            </Suspense>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="bg-gradient-to-r from-[#2D2D2D] to-[#4A4A4A] py-12">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex flex-col md:flex-row items-center justify-between gap-6">
            <div>
              <h2 className="text-2xl font-bold text-white mb-2">
                Cần tư vấn về sản phẩm?
              </h2>
              <p className="text-white/70">
                Liên hệ ngay với đội ngũ chuyên gia của chúng tôi để được hỗ trợ tốt nhất.
              </p>
            </div>
            <div className="flex items-center gap-4">
              <a
                href="tel:0868777386"
                className="inline-flex items-center gap-2 px-6 py-3 bg-[#C9A227] text-white rounded-xl font-medium hover:bg-[#B8912D] transition-colors shadow-lg"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
                </svg>
                0868.777.386
              </a>
              <a
                href="https://zalo.me/0868777386"
                target="_blank"
                rel="noopener noreferrer"
                className="inline-flex items-center gap-2 px-6 py-3 bg-white text-[#2D2D2D] rounded-xl font-medium hover:bg-gray-100 transition-colors shadow-lg"
              >
                Chat Zalo
              </a>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
