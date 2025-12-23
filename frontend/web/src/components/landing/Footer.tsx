'use client';

import Link from 'next/link';

export default function Footer() {
    // Use static year to avoid hydration mismatch
    const currentYear = 2024;

    const productLinks = [
        { href: '/san-pham/lang-mo-da', label: 'Lăng mộ đá' },
        { href: '/san-pham/mo-da', label: 'Mộ đá' },
        { href: '/san-pham/mo-thap', label: 'Mộ tháp' },
        { href: '/san-pham/ban-tho-da', label: 'Bàn thờ đá' },
        { href: '/san-pham/tuong-da', label: 'Tượng đá' },
        { href: '/san-pham/cuon-thu-da', label: 'Cuốn thư đá' },
    ];

    const policyLinks = [
        { href: '/chinh-sach/bao-hanh', label: 'Chính sách bảo hành' },
        { href: '/chinh-sach/van-chuyen', label: 'Vận chuyển lắp đặt' },
        { href: '/chinh-sach/thanh-toan', label: 'Hình thức thanh toán' },
        { href: '/chinh-sach/bao-mat', label: 'Bảo mật thông tin' },
    ];

    return (
        <footer className="bg-[#1A1A1A] text-white">
            {/* Main Footer */}
            <div className="max-w-7xl mx-auto px-4 py-16">
                <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-10">
                    {/* Company Info */}
                    <div className="lg:col-span-1">
                        <div className="flex items-center gap-3 mb-6">
                            <div className="w-12 h-12 bg-gradient-to-br from-[#C9A227] to-[#A68921] rounded-lg flex items-center justify-center">
                                <span className="text-white font-bold text-xl">TT</span>
                            </div>
                            <div>
                                <h3 className="font-bold text-lg">TRƯỜNG THÀNH</h3>
                                <p className="text-white/60 text-xs">Lăng Mộ Đá Cao Cấp</p>
                            </div>
                        </div>
                        <p className="text-white/70 text-sm leading-relaxed mb-6">
                            Chuyên thiết kế, chế tác và lắp đặt lăng mộ đá nguyên khối cao cấp.
                            30+ năm kinh nghiệm với 10.000+ công trình trên toàn quốc.
                        </p>

                        {/* Certifications */}
                        <div className="flex items-center gap-4">
                            <img
                                src="https://via.placeholder.com/80x30/2D2D2D/C9A227?text=BCT"
                                alt="Bộ Công Thương"
                                className="h-8 opacity-70 hover:opacity-100 transition-opacity"
                            />
                        </div>
                    </div>

                    {/* Products */}
                    <div>
                        <h4 className="font-bold text-lg mb-6 text-[#C9A227]">Sản Phẩm</h4>
                        <ul className="space-y-3">
                            {productLinks.map((link) => (
                                <li key={link.href}>
                                    <Link
                                        href={link.href}
                                        className="text-white/70 hover:text-[#C9A227] transition-colors text-sm flex items-center gap-2"
                                    >
                                        <svg className="w-3 h-3" fill="currentColor" viewBox="0 0 20 20">
                                            <path fillRule="evenodd" d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z" clipRule="evenodd" />
                                        </svg>
                                        {link.label}
                                    </Link>
                                </li>
                            ))}
                        </ul>
                    </div>

                    {/* Policies */}
                    <div>
                        <h4 className="font-bold text-lg mb-6 text-[#C9A227]">Chính Sách</h4>
                        <ul className="space-y-3">
                            {policyLinks.map((link) => (
                                <li key={link.href}>
                                    <Link
                                        href={link.href}
                                        className="text-white/70 hover:text-[#C9A227] transition-colors text-sm flex items-center gap-2"
                                    >
                                        <svg className="w-3 h-3" fill="currentColor" viewBox="0 0 20 20">
                                            <path fillRule="evenodd" d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z" clipRule="evenodd" />
                                        </svg>
                                        {link.label}
                                    </Link>
                                </li>
                            ))}
                        </ul>
                    </div>

                    {/* Contact */}
                    <div>
                        <h4 className="font-bold text-lg mb-6 text-[#C9A227]">Liên Hệ</h4>
                        <div className="space-y-4">
                            <div className="flex items-start gap-3">
                                <svg className="w-5 h-5 text-[#C9A227] mt-0.5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                                </svg>
                                <div className="text-sm text-white/70">
                                    <p className="font-medium text-white">Xưởng sản xuất:</p>
                                    <p>Xã Ninh Vân, Hoa Lư, Ninh Bình</p>
                                </div>
                            </div>

                            <div className="flex items-center gap-3">
                                <svg className="w-5 h-5 text-[#C9A227] flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
                                </svg>
                                <a href="tel:0987654321" className="text-white font-bold hover:text-[#C9A227] transition-colors">
                                    0987.654.321
                                </a>
                            </div>

                            <div className="flex items-center gap-3">
                                <svg className="w-5 h-5 text-[#C9A227] flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                                </svg>
                                <a href="mailto:info@truongthanh.vn" className="text-white/70 hover:text-[#C9A227] transition-colors text-sm">
                                    info@truongthanh.vn
                                </a>
                            </div>

                            {/* Social Links */}
                            <div className="flex items-center gap-3 pt-4">
                                <a href="#" className="w-10 h-10 bg-white/10 rounded-full flex items-center justify-center hover:bg-[#C9A227] transition-colors">
                                    <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                                        <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z" />
                                    </svg>
                                </a>
                                <a href="#" className="w-10 h-10 bg-white/10 rounded-full flex items-center justify-center hover:bg-[#C9A227] transition-colors">
                                    <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
                                        <path d="M19.615 3.184c-3.604-.246-11.631-.245-15.23 0-3.897.266-4.356 2.62-4.385 8.816.029 6.185.484 8.549 4.385 8.816 3.6.245 11.626.246 15.23 0 3.897-.266 4.356-2.62 4.385-8.816-.029-6.185-.484-8.549-4.385-8.816zm-10.615 12.816v-8l8 3.993-8 4.007z" />
                                    </svg>
                                </a>
                                <a href="#" className="w-10 h-10 bg-white/10 rounded-full flex items-center justify-center hover:bg-[#C9A227] transition-colors">
                                    <span className="text-xs font-bold">Zalo</span>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Copyright */}
            <div className="border-t border-white/10">
                <div className="max-w-7xl mx-auto px-4 py-6 flex flex-col md:flex-row items-center justify-between gap-4">
                    <p className="text-white/50 text-sm text-center md:text-left">
                        © {currentYear} Lăng Mộ Đá Trường Thành. All rights reserved.
                    </p>
                    <p className="text-white/50 text-sm">
                        Thiết kế bởi <a href="#" className="text-[#C9A227] hover:underline">HayTech</a>
                    </p>
                </div>
            </div>
        </footer>
    );
}
