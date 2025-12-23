import { Metadata } from "next";
import { notFound } from "next/navigation";
import Image from "next/image";
import Link from "next/link";
import { ChevronRight } from "lucide-react";
import { productsApi, type Product } from "@/lib/api";
import { formatPrice } from "@/lib/utils";
import { ProductCard } from "@/components/product";
import { AddToCartButton } from "./add-to-cart-button";

// Mock data
const mockProduct: Product = {
  id: 1,
  name: "Lăng Mộ Đá Mẫu A - Cao Cấp",
  code: "LMD-001",
  price: 150000000,
  priceOld: 180000000,
  description: "Lăng mộ đá nguyên khối cao cấp, thiết kế tinh xảo theo phong cách truyền thống Việt Nam, phù hợp cho gia đình có điều kiện.",
  content: `
    <h2>Thông tin sản phẩm</h2>
    <p>Lăng mộ đá được chế tác từ đá xanh tự nhiên nguyên khối, kết hợp kỹ thuật điêu khắc truyền thống và công nghệ hiện đại.</p>
    <h3>Đặc điểm nổi bật</h3>
    <ul>
      <li>Đá xanh tự nhiên nguyên khối</li>
      <li>Điêu khắc họa tiết rồng phượng tinh xảo</li>
      <li>Thiết kế theo yêu cầu khách hàng</li>
      <li>Bảo hành vĩnh viễn</li>
    </ul>
    <h3>Thông số kỹ thuật</h3>
    <p>Kích thước: 3m x 4m x 2.5m<br/>Chất liệu: Đá xanh Thanh Hóa<br/>Màu sắc: Xanh đen tự nhiên</p>
  `,
  categoryId: 1,
  categoryName: "Lăng Mộ Đá",
  brandId: 1,
  productBrandName: "Trường Thành",
  quantity: 5,
  counter: 1234,
  averageRating: 4.8,
  totalReview: 25,
};

const mockRelatedProducts: Product[] = [
  { id: 2, name: "Lăng Mộ Đá Mẫu B", code: "LMD-002", price: 120000000, categoryName: "Lăng Mộ Đá" },
  { id: 3, name: "Mộ Đá Đơn Cao Cấp", code: "MD-001", price: 80000000, priceOld: 95000000, categoryName: "Mộ Đá" },
  { id: 4, name: "Cuốn Thư Đá Rồng", code: "CTD-001", price: 45000000, categoryName: "Cuốn Thư Đá" },
  { id: 5, name: "Tượng Phật Đá", code: "TPD-001", price: 35000000, categoryName: "Tượng Đá" },
];

async function getProduct(id: number): Promise<Product | null> {
  try {
    const response = await productsApi.getById(id);
    return response.data || mockProduct;
  } catch {
    // Return mock data if API fails
    if (id === 1 || id <= 10) {
      return { ...mockProduct, id };
    }
    return null;
  }
}

async function getRelatedProducts(): Promise<Product[]> {
  try {
    const response = await productsApi.getAll({ pageSize: 4 });
    return response.data || mockRelatedProducts;
  } catch {
    return mockRelatedProducts;
  }
}

interface PageProps {
  params: Promise<{ id: string }>;
}

export async function generateMetadata({ params }: PageProps): Promise<Metadata> {
  const { id } = await params;
  const product = await getProduct(parseInt(id));

  if (!product) {
    return {
      title: "Không tìm thấy sản phẩm",
    };
  }

  return {
    title: product.name,
    description: product.description || `Mua ${product.name} với giá tốt nhất tại DaHP Shop`,
    openGraph: {
      title: product.name,
      description: product.description,
      images: product.image ? [product.image] : [],
    },
  };
}

export default async function ProductDetailPage({ params }: PageProps) {
  const { id } = await params;
  const [product, relatedProducts] = await Promise.all([
    getProduct(parseInt(id)),
    getRelatedProducts(),
  ]);

  if (!product) {
    notFound();
  }

  const hasDiscount = product.priceOld && product.priceOld > product.price;
  const discountPercent = hasDiscount
    ? Math.round(((product.priceOld! - product.price) / product.priceOld!) * 100)
    : 0;

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Breadcrumb */}
      <nav className="flex items-center gap-2 text-sm text-gray-500 mb-6">
        <Link href="/" className="hover:text-primary">
          Trang chủ
        </Link>
        <ChevronRight className="h-4 w-4" />
        <Link href="/san-pham" className="hover:text-primary">
          Sản phẩm
        </Link>
        {product.categoryName && (
          <>
            <ChevronRight className="h-4 w-4" />
            <Link
              href={`/san-pham?categoryId=${product.categoryId}`}
              className="hover:text-primary"
            >
              {product.categoryName}
            </Link>
          </>
        )}
        <ChevronRight className="h-4 w-4" />
        <span className="text-gray-900">{product.name}</span>
      </nav>

      {/* Product info */}
      <div className="grid md:grid-cols-2 gap-8 mb-12">
        {/* Product image */}
        <div className="aspect-square relative bg-gray-100 rounded-lg overflow-hidden">
          {product.image ? (
            <Image
              src={product.image}
              alt={product.name}
              fill
              className="object-cover"
              priority
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center">
              <span className="text-8xl">📦</span>
            </div>
          )}
          {hasDiscount && (
            <div className="absolute top-4 left-4 bg-red-500 text-white px-3 py-1 rounded-full text-sm font-semibold">
              -{discountPercent}%
            </div>
          )}
        </div>

        {/* Product details */}
        <div className="space-y-6">
          <div>
            <p className="text-sm text-gray-500 mb-2">
              {product.brandName} • Mã: {product.code}
            </p>
            <h1 className="text-3xl font-bold text-gray-900">{product.name}</h1>
          </div>

          {/* Price */}
          <div className="flex items-baseline gap-4">
            <span className="text-3xl font-bold text-primary">
              {formatPrice(product.price)}
            </span>
            {hasDiscount && (
              <span className="text-xl text-gray-400 line-through">
                {formatPrice(product.priceOld!)}
              </span>
            )}
          </div>

          {/* Description */}
          {product.description && (
            <p className="text-gray-600">{product.description}</p>
          )}

          {/* Stock status */}
          <div className="flex items-center gap-2">
            {product.quantity && product.quantity > 0 ? (
              <>
                <span className="w-3 h-3 bg-green-500 rounded-full" />
                <span className="text-green-600 font-medium">
                  Còn {product.quantity} sản phẩm
                </span>
              </>
            ) : (
              <>
                <span className="w-3 h-3 bg-red-500 rounded-full" />
                <span className="text-red-600 font-medium">Hết hàng</span>
              </>
            )}
          </div>

          {/* View count */}
          {product.counter !== undefined && (
            <p className="text-sm text-gray-500">
              👁️ {product.counter.toLocaleString()} lượt xem
            </p>
          )}

          {/* Add to cart */}
          <AddToCartButton product={product} />

          {/* Extra info */}
          <div className="border-t pt-6 space-y-3 text-sm">
            <div className="flex items-center gap-2">
              <span className="text-gray-500">✓</span>
              <span>Giao hàng miễn phí cho đơn từ 500.000đ</span>
            </div>
            <div className="flex items-center gap-2">
              <span className="text-gray-500">✓</span>
              <span>Đổi trả trong vòng 30 ngày</span>
            </div>
            <div className="flex items-center gap-2">
              <span className="text-gray-500">✓</span>
              <span>Bảo hành chính hãng 12 tháng</span>
            </div>
          </div>
        </div>
      </div>

      {/* Product content/description */}
      {product.content && (
        <div className="mb-12">
          <h2 className="text-2xl font-bold mb-6">Mô tả sản phẩm</h2>
          <div
            className="prose prose-gray max-w-none"
            dangerouslySetInnerHTML={{ __html: product.content }}
          />
        </div>
      )}

      {/* Related products */}
      <section>
        <h2 className="text-2xl font-bold mb-6">Sản phẩm liên quan</h2>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 md:gap-6">
          {relatedProducts
            .filter((p) => p.id !== product.id)
            .slice(0, 4)
            .map((p) => (
              <ProductCard key={p.id} product={p} />
            ))}
        </div>
      </section>
    </div>
  );
}
