"use client";

import Image from "next/image";
import Link from "next/link";
import { ShoppingCart, Eye, Star } from "lucide-react";
import { formatPrice } from "@/lib/utils";
import { useCart } from "@/hooks/use-cart";
import type { Product } from "@/lib/api";

interface ProductCardProps {
  product: Product;
  variant?: "default" | "compact";
}

export function ProductCard({ product, variant = "default" }: ProductCardProps) {
  const { addItem } = useCart();

  const hasDiscount = product.priceOld && product.priceOld > product.price;
  const discountPercent = hasDiscount
    ? Math.round(((product.priceOld! - product.price) / product.priceOld!) * 100)
    : 0;

  const handleAddToCart = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    addItem({
      productId: product.id,
      productName: product.name,
      productImage: product.image,
      productCode: product.code,
      price: product.price,
      quantity: 1,
    });
  };

  const productUrl = product.url ? `/san-pham/${product.url}` : `/san-pham/${product.id}`;

  if (variant === "compact") {
    return (
      <Link href={productUrl} className="group flex gap-4 p-3 rounded-xl bg-white hover:shadow-md transition-all">
        <div className="relative w-20 h-20 flex-shrink-0 rounded-lg overflow-hidden bg-gray-100">
          {product.image ? (
            <Image
              src={product.image}
              alt={product.name}
              fill
              className="object-cover"
              sizes="80px"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-gray-400 text-2xl">🗿</div>
          )}
        </div>
        <div className="flex-1 min-w-0">
          <h4 className="font-medium text-[#2D2D2D] text-sm line-clamp-2 group-hover:text-[#C9A227] transition-colors">
            {product.name}
          </h4>
          <p className="text-[#C9A227] font-bold mt-1">{formatPrice(product.price)}</p>
        </div>
      </Link>
    );
  }

  return (
    <div className="group bg-white rounded-2xl overflow-hidden shadow-lg hover:shadow-2xl transition-all duration-500 transform hover:-translate-y-1">
      <Link href={productUrl}>
        {/* Image Container */}
        <div className="relative aspect-[4/3] overflow-hidden bg-[#F9F7F4]">
          {product.image ? (
            <Image
              src={product.image}
              alt={product.name}
              fill
              className="object-cover transition-transform duration-700 group-hover:scale-110"
              sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center">
              <span className="text-6xl opacity-30">🗿</span>
            </div>
          )}

          {/* Overlay gradient */}
          <div className="absolute inset-0 bg-gradient-to-t from-black/40 via-transparent to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

          {/* Discount Badge */}
          {hasDiscount && (
            <div className="absolute top-3 left-3 bg-[#8B0000] text-white px-3 py-1.5 rounded-full text-xs font-bold shadow-lg">
              -{discountPercent}%
            </div>
          )}

          {/* Status Badge */}
          {product.productStatusName && (
            <div className="absolute top-3 right-3 bg-[#C9A227] text-white px-3 py-1.5 rounded-full text-xs font-medium shadow-lg">
              {product.productStatusName}
            </div>
          )}

          {/* Quick Actions */}
          <div className="absolute bottom-3 left-3 right-3 flex gap-2 opacity-0 group-hover:opacity-100 transition-all duration-300 transform translate-y-4 group-hover:translate-y-0">
            <button
              onClick={handleAddToCart}
              className="flex-1 bg-[#C9A227] text-white py-2.5 rounded-lg text-sm font-medium flex items-center justify-center gap-2 hover:bg-[#B8912D] transition-colors shadow-lg"
            >
              <ShoppingCart className="w-4 h-4" />
              Thêm vào giỏ
            </button>
            <button className="bg-white/90 backdrop-blur-sm text-[#2D2D2D] p-2.5 rounded-lg hover:bg-white transition-colors shadow-lg">
              <Eye className="w-4 h-4" />
            </button>
          </div>
        </div>

        {/* Content */}
        <div className="p-5">
          {/* Brand/Category */}
          {(product.productBrandName || product.categoryName) && (
            <p className="text-xs text-[#C9A227] font-medium uppercase tracking-wide mb-2">
              {product.productBrandName || product.categoryName}
            </p>
          )}

          {/* Product Name */}
          <h3 className="font-semibold text-[#2D2D2D] line-clamp-2 min-h-[2.75rem] text-base leading-snug group-hover:text-[#C9A227] transition-colors">
            {product.name}
          </h3>

          {/* Rating */}
          {product.averageRating !== undefined && product.averageRating > 0 && (
            <div className="flex items-center gap-1 mt-2">
              <div className="flex">
                {[1, 2, 3, 4, 5].map((star) => (
                  <Star
                    key={star}
                    className={`w-3.5 h-3.5 ${star <= Math.round(product.averageRating || 0)
                        ? "text-yellow-400 fill-yellow-400"
                        : "text-gray-300"
                      }`}
                  />
                ))}
              </div>
              {product.totalReview !== undefined && product.totalReview > 0 && (
                <span className="text-xs text-[#6B6B6B]">({product.totalReview})</span>
              )}
            </div>
          )}

          {/* Price */}
          <div className="mt-3 flex items-end gap-2">
            <span className="text-xl font-bold text-[#C9A227]">
              {formatPrice(product.price)}
            </span>
            {hasDiscount && (
              <span className="text-sm text-[#6B6B6B] line-through">
                {formatPrice(product.priceOld!)}
              </span>
            )}
          </div>

          {/* Stats */}
          {(product.quantitySold !== undefined && product.quantitySold > 0) && (
            <p className="text-xs text-[#6B6B6B] mt-2">
              Đã bán: {product.quantitySold}
            </p>
          )}
        </div>
      </Link>
    </div>
  );
}
