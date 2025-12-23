import Link from "next/link";
import { Facebook, Instagram, Youtube, Mail, Phone, MapPin } from "lucide-react";

export function Footer() {
  return (
    <footer className="bg-gray-900 text-gray-300">
      <div className="container mx-auto px-4 py-12">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          {/* Company info */}
          <div>
            <h3 className="text-white text-xl font-bold mb-4">DaHP Shop</h3>
            <p className="text-sm mb-4">
              Chuyên cung cấp sản phẩm chất lượng cao với giá cả cạnh tranh.
              Cam kết mang đến trải nghiệm mua sắm tốt nhất cho khách hàng.
            </p>
            <div className="flex gap-4">
              <a href="#" className="hover:text-white transition-colors">
                <Facebook className="h-5 w-5" />
              </a>
              <a href="#" className="hover:text-white transition-colors">
                <Instagram className="h-5 w-5" />
              </a>
              <a href="#" className="hover:text-white transition-colors">
                <Youtube className="h-5 w-5" />
              </a>
            </div>
          </div>

          {/* Quick links */}
          <div>
            <h4 className="text-white font-semibold mb-4">Liên kết nhanh</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <Link href="/san-pham" className="hover:text-white transition-colors">
                  Sản phẩm
                </Link>
              </li>
              <li>
                <Link href="/tin-tuc" className="hover:text-white transition-colors">
                  Tin tức
                </Link>
              </li>
              <li>
                <Link href="/gioi-thieu" className="hover:text-white transition-colors">
                  Giới thiệu
                </Link>
              </li>
              <li>
                <Link href="/lien-he" className="hover:text-white transition-colors">
                  Liên hệ
                </Link>
              </li>
            </ul>
          </div>

          {/* Policies */}
          <div>
            <h4 className="text-white font-semibold mb-4">Chính sách</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <Link href="/chinh-sach-bao-hanh" className="hover:text-white transition-colors">
                  Chính sách bảo hành
                </Link>
              </li>
              <li>
                <Link href="/chinh-sach-doi-tra" className="hover:text-white transition-colors">
                  Chính sách đổi trả
                </Link>
              </li>
              <li>
                <Link href="/chinh-sach-van-chuyen" className="hover:text-white transition-colors">
                  Chính sách vận chuyển
                </Link>
              </li>
              <li>
                <Link href="/dieu-khoan-su-dung" className="hover:text-white transition-colors">
                  Điều khoản sử dụng
                </Link>
              </li>
            </ul>
          </div>

          {/* Contact */}
          <div>
            <h4 className="text-white font-semibold mb-4">Liên hệ</h4>
            <ul className="space-y-3 text-sm">
              <li className="flex items-start gap-2">
                <MapPin className="h-4 w-4 mt-1 shrink-0" />
                <span>123 Đường ABC, Quận XYZ, TP. Hồ Chí Minh</span>
              </li>
              <li className="flex items-center gap-2">
                <Phone className="h-4 w-4 shrink-0" />
                <span>1900 xxxx</span>
              </li>
              <li className="flex items-center gap-2">
                <Mail className="h-4 w-4 shrink-0" />
                <span>support@example.com</span>
              </li>
            </ul>
          </div>
        </div>

        {/* Bottom bar */}
        <div className="border-t border-gray-800 mt-8 pt-8 text-center text-sm">
          <p>© 2024 DaHP Shop. Tất cả quyền được bảo lưu.</p>
        </div>
      </div>
    </footer>
  );
}
