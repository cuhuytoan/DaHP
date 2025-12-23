import { Metadata } from "next";
import { Suspense } from "react";
import Link from "next/link";
import Image from "next/image";
import { articlesApi, type Article } from "@/lib/api";
import { formatDate } from "@/lib/utils";
import { ArticleFilters } from "./article-filters";
import { ArticlePagination } from "./article-pagination";
import { ChevronRight, Newspaper, Clock, Eye, ArrowRight, Bookmark } from "lucide-react";

export const metadata: Metadata = {
  title: "Tin Tức & Bài Viết | Lăng Mộ Đá Trường Thành",
  description: "Cập nhật tin tức mới nhất về lăng mộ đá, kiến thức phong thủy, hướng dẫn chọn mộ đá và các thông tin hữu ích từ Trường Thành.",
  keywords: "tin tức lăng mộ đá, kiến thức phong thủy, hướng dẫn chọn mộ đá, lăng mộ đá ninh bình",
};

// Mock data for fallback
const mockArticles: Article[] = [
  {
    id: 1,
    name: "Hướng Dẫn Chọn Lăng Mộ Đá Phù Hợp Cho Gia Đình",
    subTitle: "Những điều cần biết trước khi đặt mua",
    description: "Bài viết chia sẻ kinh nghiệm và những tiêu chí quan trọng giúp quý khách hàng lựa chọn được khu lăng mộ đá phù hợp nhất với gia đình.",
    articleStatusName: "Hướng dẫn",
    author: "Trường Thành",
    counter: 1234,
    createDate: "2024-12-15",
    active: true,
  },
  {
    id: 2,
    name: "Ý Nghĩa Các Họa Tiết Trên Lăng Mộ Đá Truyền Thống",
    subTitle: "Rồng, Phượng, Hoa Sen và nhiều hơn nữa",
    description: "Tìm hiểu ý nghĩa sâu xa của các họa tiết điêu khắc truyền thống như rồng phượng, hoa sen, mây trời trên lăng mộ đá Việt Nam.",
    articleStatusName: "Kiến thức",
    author: "Trường Thành",
    counter: 892,
    createDate: "2024-12-14",
    active: true,
  },
  {
    id: 3,
    name: "Top 10 Mẫu Lăng Mộ Đá Đẹp Nhất Năm 2024",
    subTitle: "Xu hướng thiết kế lăng mộ hiện đại",
    description: "Điểm qua những mẫu lăng mộ đá được yêu thích nhất trong năm, kết hợp giữa nét truyền thống và hiện đại.",
    articleStatusName: "Mẫu đẹp",
    author: "Trường Thành",
    counter: 2156,
    createDate: "2024-12-13",
    active: true,
  },
  {
    id: 4,
    name: "Quy Trình Thi Công Lăng Mộ Đá Tại Trường Thành",
    subTitle: "5 bước để hoàn thành một công trình",
    description: "Từ khảo sát, thiết kế, chế tác đến lắp đặt và bàn giao - quy trình chuyên nghiệp đảm bảo chất lượng.",
    articleStatusName: "Quy trình",
    author: "Trường Thành",
    counter: 756,
    createDate: "2024-12-12",
    active: true,
  },
  {
    id: 5,
    name: "Phong Thủy Lăng Mộ: Những Điều Cần Lưu Ý",
    subTitle: "Hướng dẫn cơ bản về phong thủy mộ phần",
    description: "Các nguyên tắc phong thủy cơ bản khi xây dựng lăng mộ để mang lại bình an và may mắn cho gia đình.",
    articleStatusName: "Phong thủy",
    author: "Trường Thành",
    counter: 1890,
    createDate: "2024-12-11",
    active: true,
  },
  {
    id: 6,
    name: "Đá Xanh Thanh Hóa - Chất Liệu Vàng Cho Lăng Mộ",
    subTitle: "Tại sao đá xanh được ưa chuộng?",
    description: "Khám phá những đặc điểm nổi bật của đá xanh Thanh Hóa và lý do nó trở thành lựa chọn hàng đầu.",
    articleStatusName: "Chất liệu",
    author: "Trường Thành",
    counter: 645,
    createDate: "2024-12-10",
    active: true,
  },
];

const articleTypes = [
  { id: 1, name: "Tin tức", count: 25 },
  { id: 2, name: "Hướng dẫn", count: 18 },
  { id: 3, name: "Kiến thức", count: 32 },
  { id: 4, name: "Mẫu đẹp", count: 45 },
  { id: 5, name: "Phong thủy", count: 15 },
  { id: 6, name: "Chất liệu", count: 12 },
];

interface SearchParams {
  page?: string;
  typeId?: string;
  keyword?: string;
}

async function getArticles(params: SearchParams) {
  try {
    const response = await articlesApi.getAll({
      page: params.page ? parseInt(params.page) : 1,
      pageSize: 9,
      typeId: params.typeId ? parseInt(params.typeId) : undefined,
      keyword: params.keyword,
    });
    return {
      articles: response.data?.length > 0 ? response.data : mockArticles,
      total: response.pagination?.totalCount || mockArticles.length,
      totalPages: response.pagination?.totalPages || 1,
    };
  } catch {
    return {
      articles: mockArticles,
      total: mockArticles.length,
      totalPages: 1,
    };
  }
}

// Article Card Component
function ArticleCard({ article, featured = false }: { article: Article; featured?: boolean }) {
  const articleUrl = article.url ? `/tin-tuc/${article.url}` : `/tin-tuc/${article.id}`;

  if (featured) {
    return (
      <Link href={articleUrl} className="group block">
        <article className="relative bg-white rounded-2xl overflow-hidden shadow-lg hover:shadow-2xl transition-all duration-500">
          <div className="grid md:grid-cols-2 gap-0">
            {/* Image */}
            <div className="relative aspect-[4/3] md:aspect-auto overflow-hidden bg-[#F9F7F4]">
              {article.image ? (
                <Image
                  src={article.image}
                  alt={article.name}
                  fill
                  className="object-cover transition-transform duration-700 group-hover:scale-105"
                  sizes="(max-width: 768px) 100vw, 50vw"
                  priority
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center min-h-[300px]">
                  <span className="text-8xl opacity-30">📰</span>
                </div>
              )}
              {/* Overlay gradient */}
              <div className="absolute inset-0 bg-gradient-to-t from-black/30 via-transparent to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

              {/* Featured badge */}
              <div className="absolute top-4 left-4 bg-[#C9A227] text-white px-4 py-1.5 rounded-full text-sm font-semibold shadow-lg flex items-center gap-1">
                <Bookmark className="w-4 h-4" />
                Nổi bật
              </div>
            </div>

            {/* Content */}
            <div className="p-6 md:p-8 flex flex-col justify-center">
              {/* Category */}
              {article.articleStatusName && (
                <span className="inline-block bg-[#C9A227]/10 text-[#C9A227] text-sm font-medium px-3 py-1 rounded-full mb-4 w-fit">
                  {article.articleStatusName}
                </span>
              )}

              {/* Title */}
              <h2 className="text-2xl md:text-3xl font-bold text-[#2D2D2D] mb-3 group-hover:text-[#C9A227] transition-colors line-clamp-2">
                {article.name}
              </h2>

              {/* Subtitle */}
              {article.subTitle && (
                <p className="text-[#6B6B6B] font-medium mb-3">{article.subTitle}</p>
              )}

              {/* Description */}
              {article.description && (
                <p className="text-[#6B6B6B] line-clamp-3 mb-4">{article.description}</p>
              )}

              {/* Meta */}
              <div className="flex items-center gap-4 text-sm text-[#6B6B6B] mt-auto">
                {article.createDate && (
                  <span className="flex items-center gap-1">
                    <Clock className="w-4 h-4" />
                    {formatDate(article.createDate)}
                  </span>
                )}
                {article.counter !== undefined && article.counter > 0 && (
                  <span className="flex items-center gap-1">
                    <Eye className="w-4 h-4" />
                    {article.counter.toLocaleString()}
                  </span>
                )}
              </div>

              {/* Read more */}
              <div className="mt-4 flex items-center gap-2 text-[#C9A227] font-medium group-hover:gap-3 transition-all">
                Đọc tiếp
                <ArrowRight className="w-4 h-4" />
              </div>
            </div>
          </div>
        </article>
      </Link>
    );
  }

  return (
    <Link href={articleUrl} className="group block">
      <article className="bg-white rounded-2xl overflow-hidden shadow-lg hover:shadow-2xl transition-all duration-500 transform hover:-translate-y-1 h-full flex flex-col">
        {/* Image */}
        <div className="relative aspect-[16/10] overflow-hidden bg-[#F9F7F4]">
          {article.image ? (
            <Image
              src={article.image}
              alt={article.name}
              fill
              className="object-cover transition-transform duration-700 group-hover:scale-110"
              sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center">
              <span className="text-6xl opacity-30">📰</span>
            </div>
          )}
          {/* Overlay gradient */}
          <div className="absolute inset-0 bg-gradient-to-t from-black/40 via-transparent to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

          {/* Category badge */}
          {article.articleStatusName && (
            <div className="absolute top-3 left-3 bg-[#C9A227] text-white px-3 py-1 rounded-full text-xs font-medium shadow-lg">
              {article.articleStatusName}
            </div>
          )}
        </div>

        {/* Content */}
        <div className="p-5 flex-1 flex flex-col">
          {/* Title */}
          <h3 className="font-semibold text-[#2D2D2D] text-lg line-clamp-2 mb-2 group-hover:text-[#C9A227] transition-colors">
            {article.name}
          </h3>

          {/* Description */}
          {article.description && (
            <p className="text-[#6B6B6B] text-sm line-clamp-2 mb-4 flex-1">
              {article.description}
            </p>
          )}

          {/* Meta */}
          <div className="flex items-center justify-between text-xs text-[#6B6B6B] mt-auto pt-4 border-t border-gray-100">
            {article.createDate && (
              <span className="flex items-center gap-1">
                <Clock className="w-3.5 h-3.5" />
                {formatDate(article.createDate)}
              </span>
            )}
            {article.counter !== undefined && article.counter > 0 && (
              <span className="flex items-center gap-1">
                <Eye className="w-3.5 h-3.5" />
                {article.counter.toLocaleString()} lượt xem
              </span>
            )}
          </div>
        </div>
      </article>
    </Link>
  );
}

// Loading skeleton
function ArticleGridSkeleton() {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {Array.from({ length: 6 }).map((_, i) => (
        <div key={i} className="bg-white rounded-2xl overflow-hidden shadow-lg animate-pulse">
          <div className="aspect-[16/10] bg-gray-200" />
          <div className="p-5 space-y-3">
            <div className="h-5 bg-gray-200 rounded w-3/4" />
            <div className="h-4 bg-gray-200 rounded w-full" />
            <div className="h-4 bg-gray-200 rounded w-2/3" />
            <div className="h-3 bg-gray-200 rounded w-1/3" />
          </div>
        </div>
      ))}
    </div>
  );
}

export default async function ArticlesPage({
  searchParams,
}: {
  searchParams: Promise<SearchParams>;
}) {
  const params = await searchParams;
  const { articles, total, totalPages } = await getArticles(params);
  const currentPage = params.page ? parseInt(params.page) : 1;

  // Get featured article (first one) and rest
  const featuredArticle = articles[0];
  const regularArticles = articles.slice(1);

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
            <span className="text-[#C9A227]">Tin tức</span>
          </nav>

          <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4">
            <div>
              <div className="flex items-center gap-3 mb-2">
                <div className="w-12 h-12 bg-[#C9A227] rounded-xl flex items-center justify-center">
                  <Newspaper className="w-6 h-6 text-white" />
                </div>
                <h1 className="text-3xl md:text-4xl font-bold text-white">
                  Tin Tức & Bài Viết
                </h1>
              </div>
              <p className="text-white/70 max-w-xl">
                Cập nhật kiến thức về lăng mộ đá, phong thủy, và những mẫu thiết kế đẹp từ xưởng đá Trường Thành.
              </p>
            </div>

            {/* Stats */}
            <div className="flex items-center gap-6">
              <div className="text-center">
                <p className="text-2xl font-bold text-[#C9A227]">{total}</p>
                <p className="text-sm text-white/60">Bài viết</p>
              </div>
              <div className="w-px h-12 bg-white/20" />
              <div className="text-center">
                <p className="text-2xl font-bold text-[#C9A227]">{articleTypes.length}</p>
                <p className="text-sm text-white/60">Chuyên mục</p>
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
                <ArticleFilters types={articleTypes} />
              </Suspense>

              {/* Popular Articles */}
              <div className="mt-6 bg-white rounded-2xl shadow-lg p-5">
                <h3 className="font-semibold text-[#2D2D2D] mb-4 flex items-center gap-2">
                  <Eye className="w-5 h-5 text-[#C9A227]" />
                  Bài viết phổ biến
                </h3>
                <div className="space-y-4">
                  {articles.slice(0, 3).map((article, index) => (
                    <Link
                      key={article.id}
                      href={article.url ? `/tin-tuc/${article.url}` : `/tin-tuc/${article.id}`}
                      className="flex gap-3 group"
                    >
                      <span className="flex-shrink-0 w-8 h-8 bg-[#C9A227]/10 text-[#C9A227] rounded-lg flex items-center justify-center font-bold text-sm">
                        {index + 1}
                      </span>
                      <div className="flex-1 min-w-0">
                        <h4 className="text-sm font-medium text-[#2D2D2D] line-clamp-2 group-hover:text-[#C9A227] transition-colors">
                          {article.name}
                        </h4>
                        {article.counter !== undefined && (
                          <span className="text-xs text-[#6B6B6B]">{article.counter.toLocaleString()} lượt xem</span>
                        )}
                      </div>
                    </Link>
                  ))}
                </div>
              </div>

              {/* Newsletter CTA */}
              <div className="mt-6 bg-gradient-to-br from-[#C9A227] to-[#D4AF37] rounded-2xl p-5 text-white">
                <h3 className="font-semibold mb-2">Đăng ký nhận tin</h3>
                <p className="text-sm text-white/80 mb-4">
                  Nhận thông tin mới nhất về mẫu đẹp và ưu đãi đặc biệt.
                </p>
                <input
                  type="email"
                  placeholder="Email của bạn"
                  className="w-full px-4 py-2 rounded-lg bg-white/20 backdrop-blur-sm text-white placeholder-white/60 border border-white/30 focus:outline-none focus:border-white mb-3"
                />
                <button className="w-full bg-white text-[#C9A227] py-2 rounded-lg font-medium hover:bg-white/90 transition-colors">
                  Đăng ký
                </button>
              </div>
            </div>
          </aside>

          {/* Articles Grid */}
          <div className="lg:col-span-3">
            <Suspense fallback={<ArticleGridSkeleton />}>
              {articles.length > 0 ? (
                <>
                  {/* Featured Article */}
                  {currentPage === 1 && featuredArticle && (
                    <div className="mb-8">
                      <ArticleCard article={featuredArticle} featured />
                    </div>
                  )}

                  {/* Regular Articles Grid */}
                  <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {(currentPage === 1 ? regularArticles : articles).map((article) => (
                      <ArticleCard key={article.id} article={article} />
                    ))}
                  </div>

                  {/* Pagination */}
                  {totalPages > 1 && (
                    <div className="mt-10">
                      <ArticlePagination
                        currentPage={currentPage}
                        totalPages={totalPages}
                      />
                    </div>
                  )}
                </>
              ) : (
                <div className="text-center py-20 bg-white rounded-2xl shadow-lg">
                  <div className="w-24 h-24 bg-[#F9F7F4] rounded-full flex items-center justify-center mx-auto mb-6">
                    <Newspaper className="w-12 h-12 text-[#C9A227]" />
                  </div>
                  <h3 className="text-xl font-semibold text-[#2D2D2D] mb-2">
                    Chưa có bài viết nào
                  </h3>
                  <p className="text-[#6B6B6B] mb-6">
                    Hãy quay lại sau để xem các bài viết mới nhất
                  </p>
                  <Link
                    href="/tin-tuc"
                    className="inline-flex items-center gap-2 px-6 py-3 bg-gradient-to-r from-[#C9A227] to-[#D4AF37] text-white rounded-xl font-medium hover:shadow-lg transition-all"
                  >
                    Xem tất cả bài viết
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
                Cần tư vấn thêm?
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
              <Link
                href="/san-pham"
                className="inline-flex items-center gap-2 px-6 py-3 bg-white text-[#2D2D2D] rounded-xl font-medium hover:bg-gray-100 transition-colors shadow-lg"
              >
                Xem sản phẩm
                <ArrowRight className="w-4 h-4" />
              </Link>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
