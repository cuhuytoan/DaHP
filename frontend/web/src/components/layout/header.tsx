"use client";

import Link from "next/link";
import { useState, useSyncExternalStore } from "react";
import { ShoppingCart, User, Menu, X, Search } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useCart } from "@/hooks/use-cart";
import { useAuth } from "@/hooks/use-auth";

// Helper to check if we're on the client
const emptySubscribe = () => () => {};
const getSnapshot = () => true;
const getServerSnapshot = () => false;

export function Header() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const mounted = useSyncExternalStore(emptySubscribe, getSnapshot, getServerSnapshot);
  const { getTotalItems } = useCart();
  const { isAuthenticated, user } = useAuth();

  const cartItemCount = mounted ? getTotalItems() : 0;

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    if (searchQuery.trim()) {
      window.location.href = `/san-pham?search=${encodeURIComponent(searchQuery)}`;
    }
  };

  return (
    <header className="sticky top-0 z-50 w-full border-b bg-white/95 backdrop-blur supports-[backdrop-filter]:bg-white/60">
      <div className="container mx-auto px-4">
        {/* Top bar */}
        <div className="hidden md:flex items-center justify-between py-2 text-sm border-b">
          <div className="flex items-center gap-4 text-gray-600">
            <span>📞 Hotline: 1900 xxxx</span>
            <span>📧 support@example.com</span>
          </div>
          <div className="flex items-center gap-4">
            {isAuthenticated ? (
              <Link href="/tai-khoan" className="hover:text-primary">
                Xin chào, {user?.fullName}
              </Link>
            ) : (
              <>
                <Link href="/dang-nhap" className="hover:text-primary">
                  Đăng nhập
                </Link>
                <Link href="/dang-ky" className="hover:text-primary">
                  Đăng ký
                </Link>
              </>
            )}
          </div>
        </div>

        {/* Main header */}
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link href="/" className="flex items-center gap-2">
            <span className="text-2xl font-bold text-primary">DaHP</span>
            <span className="hidden sm:inline text-sm text-gray-600">Shop</span>
          </Link>

          {/* Search bar - Desktop */}
          <form onSubmit={handleSearch} className="hidden md:flex flex-1 max-w-md mx-8">
            <div className="relative w-full">
              <Input
                type="search"
                placeholder="Tìm kiếm sản phẩm..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="pr-10"
              />
              <Button
                type="submit"
                variant="ghost"
                size="icon"
                className="absolute right-0 top-0"
              >
                <Search className="h-4 w-4" />
              </Button>
            </div>
          </form>

          {/* Navigation - Desktop */}
          <nav className="hidden md:flex items-center gap-6">
            <Link href="/san-pham" className="text-gray-700 hover:text-primary font-medium">
              Sản phẩm
            </Link>
            <Link href="/tin-tuc" className="text-gray-700 hover:text-primary font-medium">
              Tin tức
            </Link>
            <Link href="/lien-he" className="text-gray-700 hover:text-primary font-medium">
              Liên hệ
            </Link>
          </nav>

          {/* Actions */}
          <div className="flex items-center gap-2">
            <Link href="/gio-hang">
              <Button variant="ghost" size="icon" className="relative">
                <ShoppingCart className="h-5 w-5" />
                {cartItemCount > 0 && (
                  <span className="absolute -top-1 -right-1 h-5 w-5 rounded-full bg-primary text-white text-xs flex items-center justify-center">
                    {cartItemCount}
                  </span>
                )}
              </Button>
            </Link>

            <Link href={isAuthenticated ? "/tai-khoan" : "/dang-nhap"}>
              <Button variant="ghost" size="icon">
                <User className="h-5 w-5" />
              </Button>
            </Link>

            {/* Mobile menu button */}
            <Button
              variant="ghost"
              size="icon"
              className="md:hidden"
              onClick={() => setIsMenuOpen(!isMenuOpen)}
            >
              {isMenuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
            </Button>
          </div>
        </div>

        {/* Mobile menu */}
        {isMenuOpen && (
          <div className="md:hidden py-4 border-t">
            <form onSubmit={handleSearch} className="mb-4">
              <div className="relative">
                <Input
                  type="search"
                  placeholder="Tìm kiếm sản phẩm..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="pr-10"
                />
                <Button
                  type="submit"
                  variant="ghost"
                  size="icon"
                  className="absolute right-0 top-0"
                >
                  <Search className="h-4 w-4" />
                </Button>
              </div>
            </form>
            <nav className="flex flex-col gap-2">
              <Link
                href="/san-pham"
                className="py-2 text-gray-700 hover:text-primary font-medium"
                onClick={() => setIsMenuOpen(false)}
              >
                Sản phẩm
              </Link>
              <Link
                href="/tin-tuc"
                className="py-2 text-gray-700 hover:text-primary font-medium"
                onClick={() => setIsMenuOpen(false)}
              >
                Tin tức
              </Link>
              <Link
                href="/lien-he"
                className="py-2 text-gray-700 hover:text-primary font-medium"
                onClick={() => setIsMenuOpen(false)}
              >
                Liên hệ
              </Link>
              {!isAuthenticated && (
                <>
                  <Link
                    href="/dang-nhap"
                    className="py-2 text-gray-700 hover:text-primary font-medium"
                    onClick={() => setIsMenuOpen(false)}
                  >
                    Đăng nhập
                  </Link>
                  <Link
                    href="/dang-ky"
                    className="py-2 text-gray-700 hover:text-primary font-medium"
                    onClick={() => setIsMenuOpen(false)}
                  >
                    Đăng ký
                  </Link>
                </>
              )}
            </nav>
          </div>
        )}
      </div>
    </header>
  );
}
