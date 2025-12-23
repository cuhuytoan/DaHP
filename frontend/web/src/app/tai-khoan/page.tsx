"use client";

import { useEffect } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { User, Package, MapPin, Heart, LogOut, ChevronRight } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useAuth } from "@/hooks/use-auth";

export default function AccountPage() {
  const router = useRouter();
  const { isAuthenticated, user, logout } = useAuth();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push("/dang-nhap?redirect=/tai-khoan");
    }
  }, [isAuthenticated, router]);

  const handleLogout = () => {
    logout();
    router.push("/");
  };

  if (!isAuthenticated) {
    return null;
  }

  const menuItems = [
    {
      icon: User,
      label: "Thông tin cá nhân",
      href: "/tai-khoan/thong-tin",
      description: "Cập nhật thông tin và mật khẩu",
    },
    {
      icon: Package,
      label: "Đơn hàng của tôi",
      href: "/tai-khoan/don-hang",
      description: "Xem lịch sử và theo dõi đơn hàng",
    },
    {
      icon: MapPin,
      label: "Sổ địa chỉ",
      href: "/tai-khoan/dia-chi",
      description: "Quản lý địa chỉ giao hàng",
    },
    {
      icon: Heart,
      label: "Sản phẩm yêu thích",
      href: "/tai-khoan/yeu-thich",
      description: "Danh sách sản phẩm đã lưu",
    },
  ];

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold text-gray-900 mb-8">Tài khoản của tôi</h1>

      <div className="grid md:grid-cols-3 gap-8">
        {/* User info card */}
        <div className="md:col-span-1">
          <div className="bg-white rounded-lg border p-6">
            <div className="text-center">
              <div className="w-20 h-20 bg-primary/10 rounded-full flex items-center justify-center mx-auto mb-4">
                <User className="h-10 w-10 text-primary" />
              </div>
              <h2 className="text-xl font-bold">{user?.fullName}</h2>
              <p className="text-gray-500 text-sm">{user?.email}</p>
            </div>

            <div className="mt-6 pt-6 border-t">
              <Button
                variant="outline"
                className="w-full text-red-500 hover:text-red-600 hover:bg-red-50"
                onClick={handleLogout}
              >
                <LogOut className="h-4 w-4 mr-2" />
                Đăng xuất
              </Button>
            </div>
          </div>
        </div>

        {/* Menu items */}
        <div className="md:col-span-2">
          <div className="bg-white rounded-lg border divide-y">
            {menuItems.map((item) => (
              <Link
                key={item.href}
                href={item.href}
                className="flex items-center gap-4 p-4 hover:bg-gray-50 transition-colors"
              >
                <div className="w-12 h-12 bg-gray-100 rounded-lg flex items-center justify-center">
                  <item.icon className="h-6 w-6 text-gray-600" />
                </div>
                <div className="flex-1">
                  <h3 className="font-medium">{item.label}</h3>
                  <p className="text-sm text-gray-500">{item.description}</p>
                </div>
                <ChevronRight className="h-5 w-5 text-gray-400" />
              </Link>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
