"use client";

import { useState } from "react";
import { Minus, Plus, ShoppingCart, Check } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useCart } from "@/hooks/use-cart";
import type { Product } from "@/lib/api";

interface AddToCartButtonProps {
  product: Product;
}

export function AddToCartButton({ product }: AddToCartButtonProps) {
  const [quantity, setQuantity] = useState(1);
  const [added, setAdded] = useState(false);
  const { addItem } = useCart();

  const handleAddToCart = () => {
    addItem({
      productId: product.id,
      productName: product.name,
      productImage: product.image,
      productCode: product.code,
      price: product.price,
      quantity: quantity,
    });
    setAdded(true);
    setTimeout(() => setAdded(false), 2000);
  };

  const isOutOfStock = product.quantity !== undefined && product.quantity <= 0;
  const maxQuantity = product.quantity || 99;

  return (
    <div className="space-y-4">
      {/* Quantity selector */}
      <div className="flex items-center gap-4">
        <span className="text-sm font-medium text-[#2D2D2D]">Số lượng:</span>
        <div className="flex items-center border border-gray-200 rounded-xl overflow-hidden">
          <Button
            variant="ghost"
            size="icon"
            onClick={() => setQuantity(Math.max(1, quantity - 1))}
            disabled={quantity <= 1}
            className="h-10 w-10 rounded-none hover:bg-[#F9F7F4]"
          >
            <Minus className="h-4 w-4" />
          </Button>
          <span className="w-14 text-center font-semibold text-[#2D2D2D] border-x border-gray-200">{quantity}</span>
          <Button
            variant="ghost"
            size="icon"
            onClick={() => setQuantity(Math.min(maxQuantity, quantity + 1))}
            disabled={quantity >= maxQuantity}
            className="h-10 w-10 rounded-none hover:bg-[#F9F7F4]"
          >
            <Plus className="h-4 w-4" />
          </Button>
        </div>
        {product.quantity !== undefined && product.quantity > 0 && (
          <span className="text-sm text-[#6B6B6B]">Còn {product.quantity} sản phẩm</span>
        )}
      </div>

      {/* Add to cart button */}
      <div className="flex gap-4">
        <Button
          size="lg"
          className={`flex-1 py-6 text-base rounded-xl transition-all ${added
              ? "bg-green-600 hover:bg-green-700"
              : "bg-gradient-to-r from-[#C9A227] to-[#D4AF37] hover:from-[#B8912D] hover:to-[#C9A227]"
            }`}
          onClick={handleAddToCart}
          disabled={isOutOfStock || added}
        >
          {added ? (
            <>
              <Check className="mr-2 h-5 w-5" />
              Đã thêm vào giỏ hàng
            </>
          ) : isOutOfStock ? (
            "Hết hàng"
          ) : (
            <>
              <ShoppingCart className="mr-2 h-5 w-5" />
              Thêm vào giỏ hàng
            </>
          )}
        </Button>
      </div>
    </div>
  );
}
