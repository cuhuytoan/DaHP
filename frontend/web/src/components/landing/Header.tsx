'use client';

import Link from 'next/link';
import { useState, useEffect } from 'react';

export default function Header() {
    const [isScrolled, setIsScrolled] = useState(false);
    const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

    useEffect(() => {
        const handleScroll = () => {
            setIsScrolled(window.scrollY > 50);
        };
        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    }, []);

    const navLinks = [
        { href: '/', label: 'Trang chủ' },
        { href: '/san-pham', label: 'Sản phẩm' },
        { href: '/gioi-thieu', label: 'Giới thiệu' },
        { href: '/cong-trinh', label: 'Công trình' },
        { href: '/tin-tuc', label: 'Tin tức' },
        { href: '/lien-he', label: 'Liên hệ' },
    ];

    return (
        <header
            className={`fixed top-0 left-0 right-0 z-50 transition-all duration-300 ${isScrolled
                ? 'bg-white/95 backdrop-blur-md shadow-lg py-3'
                : 'bg-transparent py-5'
                }`}
        >
            <div className="max-w-7xl mx-auto px-4 flex items-center justify-between">
                {/* Logo */}
                <Link href="/" className="flex items-center gap-3">
                    {/* Logo Image */}
                    <div className="relative w-12 h-12 overflow-hidden rounded-lg">
                        <img
                            src="/images/logo.png"
                            alt="Trường Thành Stone Logo"
                            className="w-full h-full object-cover"
                        />
                    </div>
                    <div className="hidden sm:block">
                        <h1 className={`font-bold text-lg ${isScrolled ? 'text-[#2D2D2D]' : 'text-white'}`}>
                            TRƯỜNG THÀNH
                        </h1>
                        <p className={`text-xs ${isScrolled ? 'text-[#6B6B6B]' : 'text-white/80'}`}>
                            Lăng Mộ Đá Cao Cấp
                        </p>
                    </div>
                </Link>

                {/* Desktop Navigation */}
                <nav className="hidden lg:flex items-center gap-8">
                    {navLinks.map((link) => (
                        <Link
                            key={link.href}
                            href={link.href}
                            className={`font-medium transition-colors hover:text-[#C9A227] ${isScrolled ? 'text-[#2D2D2D]' : 'text-white'
                                }`}
                        >
                            {link.label}
                        </Link>
                    ))}
                </nav>

                {/* CTA & Hotline */}
                <div className="hidden lg:flex items-center gap-4">
                    <div className={`text-right ${isScrolled ? 'text-[#2D2D2D]' : 'text-white'}`}>
                        <p className="text-xs opacity-80">Hotline 24/7</p>
                        <a href="tel:0987654321" className="font-bold text-lg text-[#C9A227]">
                            0987.654.321
                        </a>
                    </div>
                    <button className="btn-primary text-sm">
                        Tư vấn miễn phí
                    </button>
                </div>

                {/* Mobile Menu Button */}
                <button
                    className="lg:hidden p-2"
                    onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
                >
                    <svg
                        className={`w-6 h-6 ${isScrolled ? 'text-[#2D2D2D]' : 'text-white'}`}
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                    >
                        {isMobileMenuOpen ? (
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                        ) : (
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
                        )}
                    </svg>
                </button>
            </div>

            {/* Mobile Menu */}
            {isMobileMenuOpen && (
                <div className="lg:hidden bg-white shadow-xl border-t">
                    <nav className="flex flex-col p-4">
                        {navLinks.map((link) => (
                            <Link
                                key={link.href}
                                href={link.href}
                                className="py-3 px-4 text-[#2D2D2D] font-medium hover:bg-[#F9F7F4] hover:text-[#C9A227] rounded-lg"
                                onClick={() => setIsMobileMenuOpen(false)}
                            >
                                {link.label}
                            </Link>
                        ))}
                        <div className="mt-4 pt-4 border-t">
                            <a href="tel:0987654321" className="block text-center font-bold text-xl text-[#C9A227] mb-3">
                                📞 0987.654.321
                            </a>
                            <button className="btn-primary w-full">Tư vấn miễn phí</button>
                        </div>
                    </nav>
                </div>
            )}
        </header>
    );
}
