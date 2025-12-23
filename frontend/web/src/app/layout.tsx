import type { Metadata } from "next";
import { Inter, Playfair_Display } from "next/font/google";
import "./globals.css";

const inter = Inter({
  subsets: ["latin", "vietnamese"],
  variable: "--font-inter",
});

const playfair = Playfair_Display({
  subsets: ["latin", "vietnamese"],
  variable: "--font-playfair",
});

export const metadata: Metadata = {
  title: {
    default: "Lăng Mộ Đá Trường Thành | Uy Tín Hàng Đầu Việt Nam",
    template: "%s | Lăng Mộ Đá Trường Thành",
  },
  description:
    "Lăng Mộ Đá Trường Thành - Chuyên thiết kế, chế tác lăng mộ đá nguyên khối cao cấp. 30+ năm kinh nghiệm, 10.000+ công trình. Bảo hành trọn đời.",
  keywords: ["lăng mộ đá", "mộ đá", "mộ tháp", "bàn thờ đá", "tượng đá", "lăng mộ đá ninh bình"],
  authors: [{ name: "Trường Thành Stone" }],
  openGraph: {
    type: "website",
    locale: "vi_VN",
    url: "https://langmodatruongthanh.vn",
    siteName: "Lăng Mộ Đá Trường Thành",
    title: "Lăng Mộ Đá Trường Thành | Uy Tín Hàng Đầu Việt Nam",
    description: "Chuyên thiết kế, chế tác lăng mộ đá nguyên khối cao cấp. 30+ năm kinh nghiệm.",
  },
  robots: {
    index: true,
    follow: true,
  },
  icons: {
    icon: '/favicon.png',
  },
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="vi" suppressHydrationWarning>
      <body className={`${inter.variable} ${playfair.variable} font-sans antialiased`}>
        {children}
      </body>
    </html>
  );
}
