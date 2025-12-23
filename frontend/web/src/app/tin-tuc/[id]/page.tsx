import { Metadata } from "next";
import { notFound } from "next/navigation";
import Image from "next/image";
import Link from "next/link";
import { ChevronRight, Calendar, Eye } from "lucide-react";
import { articlesApi, type Article } from "@/lib/api";
import { formatDate } from "@/lib/utils";

// Mock data
const mockArticle: Article = {
  id: 1,
  name: "Hướng Dẫn Chọn Lăng Mộ Đá Phù Hợp Cho Gia Đình",
  subTitle: "Những điều cần biết trước khi đặt mua",
  description: "Bài viết chia sẻ kinh nghiệm và những tiêu chí quan trọng giúp quý khách hàng lựa chọn được khu lăng mộ đá phù hợp nhất.",
  content: `
    <p>Việc lựa chọn lăng mộ đá cho gia đình là một quyết định quan trọng, cần được cân nhắc kỹ lưỡng. Dưới đây là những kinh nghiệm quý báu từ các chuyên gia đá mỹ nghệ Trường Thành.</p>
    
    <h2>1. Xác định ngân sách phù hợp</h2>
    <p>Trước tiên, quý khách cần xác định rõ ngân sách dự kiến. Lăng mộ đá có nhiều mức giá, từ vài chục triệu đến hàng tỷ đồng tùy thuộc vào kích thước và độ phức tạp của thiết kế.</p>
    
    <h2>2. Lựa chọn chất liệu đá</h2>
    <p>Đá xanh Thanh Hóa là loại đá được ưa chuộng nhất do độ bền cao và màu sắc đẹp tự nhiên. Ngoài ra còn có đá trắng, đá đen, đá vàng tùy theo sở thích.</p>
    
    <h2>3. Thiết kế phù hợp với phong thủy</h2>
    <p>Mỗi công trình lăng mộ đều cần tuân thủ các nguyên tắc phong thủy cơ bản để mang lại bình an cho gia đình.</p>
    
    <h2>4. Chọn đơn vị uy tín</h2>
    <p>Nên chọn những đơn vị có kinh nghiệm lâu năm, có xưởng sản xuất trực tiếp và đội ngũ nghệ nhân lành nghề.</p>
    
    <h2>Kết luận</h2>
    <p>Lăng Mộ Đá Trường Thành với hơn 30 năm kinh nghiệm luôn sẵn sàng tư vấn và đồng hành cùng quý khách hàng trong việc xây dựng công trình lăng mộ trang nghiêm và bền vững.</p>
  `,
  author: "Trường Thành",
  articleStatusName: "Đã xuất bản",
  counter: 1234,
  createDate: "2024-12-15",
  active: true,
};

const mockRelatedArticles: Article[] = [
  { id: 2, name: "Ý nghĩa các họa tiết trên lăng mộ đá", description: "Tìm hiểu ý nghĩa của các họa tiết rồng, phượng, hoa sen...", articleStatusName: "Tin tức", createDate: "2024-12-14" },
  { id: 3, name: "Top 10 mẫu lăng mộ đá đẹp nhất 2024", description: "Những mẫu lăng mộ được ưa chuộng nhất năm nay", articleStatusName: "Mẫu đẹp", createDate: "2024-12-13" },
  { id: 4, name: "Quy trình thi công lăng mộ đá tại Trường Thành", description: "5 bước để hoàn thành một công trình lăng mộ", articleStatusName: "Hướng dẫn", createDate: "2024-12-12" },
];

async function getArticle(id: number): Promise<Article | null> {
  try {
    const response = await articlesApi.getById(id);
    return response.data || mockArticle;
  } catch {
    if (id === 1 || id <= 10) {
      return { ...mockArticle, id };
    }
    return null;
  }
}

async function getRelatedArticles(): Promise<Article[]> {
  try {
    const response = await articlesApi.getAll({ pageSize: 3 });
    return response.data || mockRelatedArticles;
  } catch {
    return mockRelatedArticles;
  }
}

interface PageProps {
  params: Promise<{ id: string }>;
}

export async function generateMetadata({ params }: PageProps): Promise<Metadata> {
  const { id } = await params;
  const article = await getArticle(parseInt(id));

  if (!article) {
    return {
      title: "Không tìm thấy bài viết",
    };
  }

  return {
    title: article.name,
    description: article.description || article.name,
    openGraph: {
      title: article.name,
      description: article.description,
      type: "article",
      images: article.image ? [article.image] : [],
    },
  };
}

export default async function ArticleDetailPage({ params }: PageProps) {
  const { id } = await params;
  const [article, relatedArticles] = await Promise.all([
    getArticle(parseInt(id)),
    getRelatedArticles(),
  ]);

  if (!article) {
    notFound();
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Breadcrumb */}
      <nav className="flex items-center gap-2 text-sm text-gray-500 mb-6">
        <Link href="/" className="hover:text-primary">
          Trang chủ
        </Link>
        <ChevronRight className="h-4 w-4" />
        <Link href="/tin-tuc" className="hover:text-primary">
          Tin tức
        </Link>
        <ChevronRight className="h-4 w-4" />
        <span className="text-gray-900 line-clamp-1">{article.name}</span>
      </nav>

      <div className="grid lg:grid-cols-3 gap-8">
        {/* Main content */}
        <article className="lg:col-span-2">
          {/* Article header */}
          <header className="mb-8">
            {article.articleStatusName && (
              <span className="inline-block bg-blue-100 text-blue-700 text-sm px-3 py-1 rounded-full mb-4">
                {article.articleStatusName}
              </span>
            )}
            <h1 className="text-3xl md:text-4xl font-bold text-gray-900 mb-4">
              {article.name}
            </h1>
            <div className="flex items-center gap-4 text-sm text-gray-500">
              {article.createDate && (
                <span className="flex items-center gap-1">
                  <Calendar className="h-4 w-4" />
                  {formatDate(article.createDate)}
                </span>
              )}
              {article.counter !== undefined && (
                <span className="flex items-center gap-1">
                  <Eye className="h-4 w-4" />
                  {article.counter.toLocaleString()} lượt xem
                </span>
              )}
            </div>
          </header>

          {/* Featured image */}
          {article.image && (
            <div className="relative aspect-video mb-8 rounded-lg overflow-hidden">
              <Image
                src={article.image}
                alt={article.name}
                fill
                className="object-cover"
                priority
              />
            </div>
          )}

          {/* Description */}
          {article.description && (
            <p className="text-lg text-gray-600 mb-8 font-medium">
              {article.description}
            </p>
          )}

          {/* Content */}
          {article.content && (
            <div
              className="prose prose-gray max-w-none prose-headings:font-bold prose-h2:text-2xl prose-h2:mt-8 prose-h2:mb-4 prose-p:text-gray-600 prose-p:leading-relaxed"
              dangerouslySetInnerHTML={{ __html: article.content }}
            />
          )}

          {/* Share buttons */}
          <div className="mt-8 pt-8 border-t">
            <h3 className="font-semibold mb-4">Chia sẻ bài viết</h3>
            <div className="flex gap-2">
              <button className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors">
                Facebook
              </button>
              <button className="px-4 py-2 bg-sky-500 text-white rounded-md hover:bg-sky-600 transition-colors">
                Twitter
              </button>
              <button className="px-4 py-2 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300 transition-colors">
                Copy link
              </button>
            </div>
          </div>
        </article>

        {/* Sidebar */}
        <aside className="lg:col-span-1">
          <div className="sticky top-24">
            <h2 className="text-xl font-bold mb-6">Bài viết liên quan</h2>
            <div className="space-y-4">
              {relatedArticles
                .filter((a) => a.id !== article.id)
                .slice(0, 3)
                .map((a) => (
                  <Link
                    key={a.id}
                    href={`/tin-tuc/${a.id}`}
                    className="block group"
                  >
                    <div className="flex gap-4">
                      <div className="w-24 h-16 bg-gray-100 rounded flex-shrink-0 flex items-center justify-center">
                        {a.image ? (
                          <Image
                            src={a.image}
                            alt={a.name}
                            width={96}
                            height={64}
                            className="object-cover rounded"
                          />
                        ) : (
                          <span className="text-2xl">📰</span>
                        )}
                      </div>
                      <div>
                        <h3 className="font-medium text-sm line-clamp-2 group-hover:text-primary transition-colors">
                          {a.name}
                        </h3>
                        <p className="text-xs text-gray-500 mt-1">
                          {a.createDate && formatDate(a.createDate)}
                        </p>
                      </div>
                    </div>
                  </Link>
                ))}
            </div>
          </div>
        </aside>
      </div>
    </div>
  );
}
