"use client";

import Link from "next/link";
import Image from "next/image";
import { Minus, Plus, Trash2, ShoppingBag, ArrowRight } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useCart } from "@/hooks/use-cart";
import { formatPrice } from "@/lib/utils";

export default function CartPage() {
  const { items, updateQuantity, removeItem, getTotalPrice, clearCart } = useCart();

  const totalPrice = getTotalPrice();
  const shippingFee = totalPrice >= 500000 ? 0 : 30000;
  const finalTotal = totalPrice + shippingFee;

  if (items.length === 0) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center bg-[#F9F7F4]">
        <div className="max-w-md mx-auto text-center px-4">
          <div className="w-24 h-24 bg-white rounded-full flex items-center justify-center mx-auto mb-6 shadow-lg">
            <ShoppingBag className="h-12 w-12 text-[#C9A227]" />
          </div>
          <h1 className="text-2xl font-bold text-[#2D2D2D] mb-4">
            Giỏ hàng trống
          </h1>
          <p className="text-[#6B6B6B] mb-8">
            Bạn chưa có sản phẩm nào trong giỏ hàng. Hãy khám phá các sản phẩm đá mỹ nghệ cao cấp của chúng tôi!
          </p>
          <Link href="/san-pham">
            <Button size="lg" className="bg-gradient-to-r from-[#C9A227] to-[#D4AF37] hover:from-[#B8912D] hover:to-[#C9A227]">
              Khám phá sản phẩm
              <ArrowRight className="ml-2 h-4 w-4" />
            </Button>
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-[#F9F7F4]">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="flex items-center justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold text-[#2D2D2D]">Giỏ hàng của bạn</h1>
            <p className="text-[#6B6B6B] mt-1">{items.length} sản phẩm</p>
          </div>
          <Link href="/san-pham" className="text-[#C9A227] hover:underline font-medium">
            ← Tiếp tục mua sắm
          </Link>
        </div>

        <div className="grid lg:grid-cols-3 gap-8">
          {/* Cart items */}
          <div className="lg:col-span-2">
            <div className="bg-white rounded-2xl shadow-lg overflow-hidden">
              {/* Header */}
              <div className="hidden md:grid grid-cols-12 gap-4 p-5 bg-[#2D2D2D] text-white text-sm font-medium">
                <div className="col-span-6">Sản phẩm</div>
                <div className="col-span-2 text-center">Đơn giá</div>
                <div className="col-span-2 text-center">Số lượng</div>
                <div className="col-span-2 text-right">Thành tiền</div>
              </div>

              {/* Items */}
              <div className="divide-y divide-gray-100">
                {items.map((item) => (
                  <div key={item.productId} className="p-5 hover:bg-[#F9F7F4] transition-colors">
                    <div className="grid grid-cols-12 gap-4 items-center">
                      {/* Product info */}
                      <div className="col-span-12 md:col-span-6 flex gap-4">
                        <div className="w-24 h-24 bg-[#F9F7F4] rounded-xl flex-shrink-0 flex items-center justify-center overflow-hidden">
                          {item.productImage ? (
                            <Image
                              src={item.productImage}
                              alt={item.productName}
                              width={96}
                              height={96}
                              className="object-cover"
                            />
                          ) : (
                            <span className="text-4xl">🗿</span>
                          )}
                        </div>
                        <div className="flex-1 min-w-0">
                          <Link
                            href={`/san-pham/${item.productId}`}
                            className="font-semibold text-[#2D2D2D] hover:text-[#C9A227] line-clamp-2 transition-colors"
                          >
                            {item.productName}
                          </Link>
                          {item.productCode && (
                            <p className="text-sm text-[#6B6B6B] mt-1">
                              Mã: {item.productCode}
                            </p>
                          )}
                          <button
                            onClick={() => removeItem(item.productId)}
                            className="text-[#8B0000] text-sm flex items-center gap-1 mt-2 hover:underline md:hidden"
                          >
                            <Trash2 className="h-4 w-4" />
                            Xóa
                          </button>
                        </div>
                      </div>

                      {/* Price - Mobile */}
                      <div className="col-span-4 md:hidden">
                        <span className="text-xs text-[#6B6B6B]">Đơn giá:</span>
                        <p className="font-semibold text-[#C9A227]">{formatPrice(item.price)}</p>
                      </div>

                      {/* Price - Desktop */}
                      <div className="hidden md:block col-span-2 text-center">
                        <span className="font-semibold text-[#C9A227]">{formatPrice(item.price)}</span>
                      </div>

                      {/* Quantity */}
                      <div className="col-span-4 md:col-span-2">
                        <div className="flex items-center justify-center">
                          <button
                            onClick={() => updateQuantity(item.productId, item.quantity - 1)}
                            className="w-9 h-9 flex items-center justify-center border border-gray-200 rounded-l-lg hover:bg-[#F9F7F4] transition-colors"
                          >
                            <Minus className="h-4 w-4" />
                          </button>
                          <span className="w-12 h-9 flex items-center justify-center border-t border-b border-gray-200 text-sm font-medium">
                            {item.quantity}
                          </span>
                          <button
                            onClick={() => updateQuantity(item.productId, item.quantity + 1)}
                            className="w-9 h-9 flex items-center justify-center border border-gray-200 rounded-r-lg hover:bg-[#F9F7F4] transition-colors"
                          >
                            <Plus className="h-4 w-4" />
                          </button>
                        </div>
                      </div>

                      {/* Subtotal - Mobile */}
                      <div className="col-span-4 md:hidden text-right">
                        <span className="text-xs text-[#6B6B6B]">Thành tiền:</span>
                        <p className="font-bold text-[#C9A227]">
                          {formatPrice(item.price * item.quantity)}
                        </p>
                      </div>

                      {/* Subtotal - Desktop */}
                      <div className="hidden md:flex col-span-2 items-center justify-end gap-3">
                        <span className="font-bold text-[#C9A227]">
                          {formatPrice(item.price * item.quantity)}
                        </span>
                        <button
                          onClick={() => removeItem(item.productId)}
                          className="p-2 text-gray-400 hover:text-[#8B0000] hover:bg-red-50 rounded-lg transition-all"
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                      </div>
                    </div>
                  </div>
                ))}
              </div>

              {/* Actions */}
              <div className="p-5 border-t border-gray-100 flex flex-wrap gap-4 justify-between bg-[#F9F7F4]">
                <Link href="/san-pham">
                  <Button variant="outline" className="border-[#C9A227] text-[#C9A227] hover:bg-[#C9A227] hover:text-white">
                    ← Tiếp tục mua sắm
                  </Button>
                </Link>
                <Button
                  variant="outline"
                  onClick={clearCart}
                  className="border-[#8B0000] text-[#8B0000] hover:bg-[#8B0000] hover:text-white"
                >
                  <Trash2 className="h-4 w-4 mr-2" />
                  Xóa tất cả
                </Button>
              </div>
            </div>
          </div>

          {/* Order summary */}
          <div className="lg:col-span-1">
            <div className="bg-white rounded-2xl shadow-lg p-6 sticky top-24">
              <h2 className="text-xl font-bold text-[#2D2D2D] mb-6">Tóm tắt đơn hàng</h2>

              <div className="space-y-4 text-sm">
                <div className="flex justify-between">
                  <span className="text-[#6B6B6B]">Tạm tính ({items.length} sản phẩm)</span>
                  <span className="font-medium text-[#2D2D2D]">{formatPrice(totalPrice)}</span>
                </div>
                <div className="flex justify-between">
                  <span className="text-[#6B6B6B]">Phí vận chuyển</span>
                  <span className="font-medium">
                    {shippingFee === 0 ? (
                      <span className="text-green-600">Miễn phí</span>
                    ) : (
                      formatPrice(shippingFee)
                    )}
                  </span>
                </div>
                {shippingFee > 0 && (
                  <p className="text-xs text-[#6B6B6B] bg-[#F9F7F4] p-3 rounded-lg">
                    💡 Miễn phí vận chuyển cho đơn từ {formatPrice(500000)}
                  </p>
                )}
              </div>

              <div className="h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent my-6" />

              <div className="flex justify-between text-lg font-bold mb-6">
                <span className="text-[#2D2D2D]">Tổng cộng</span>
                <span className="text-[#C9A227]">{formatPrice(finalTotal)}</span>
              </div>

              <Link href="/thanh-toan" className="block">
                <Button className="w-full bg-gradient-to-r from-[#C9A227] to-[#D4AF37] hover:from-[#B8912D] hover:to-[#C9A227] py-6 text-base" size="lg">
                  Tiến hành thanh toán
                  <ArrowRight className="ml-2 h-5 w-5" />
                </Button>
              </Link>

              <p className="text-xs text-[#6B6B6B] text-center mt-4">
                🔒 Thanh toán an toàn & bảo mật
              </p>

              {/* Trust badges */}
              <div className="mt-6 pt-6 border-t border-gray-100">
                <div className="grid grid-cols-2 gap-3 text-xs text-[#6B6B6B]">
                  <div className="flex items-center gap-2">
                    <span className="text-lg">✅</span>
                    <span>Hàng chính hãng</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <span className="text-lg">🚚</span>
                    <span>Giao hàng toàn quốc</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <span className="text-lg">💰</span>
                    <span>Giá cả cạnh tranh</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <span className="text-lg">📞</span>
                    <span>Hỗ trợ 24/7</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
