'use client';

import { useEffect, useRef } from 'react';

export default function HeroSection() {
    const heroRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const handleScroll = () => {
            if (heroRef.current) {
                const scrollY = window.scrollY;
                const overlay = heroRef.current.querySelector('.hero-overlay') as HTMLElement;
                if (overlay) {
                    overlay.style.transform = `translateY(${scrollY * 0.3}px)`;
                }
            }
        };
        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    }, []);

    return (
        <section
            ref={heroRef}
            className="relative min-h-screen flex items-center justify-center overflow-hidden"
        >
            {/* Background Image */}
            <div
                className="absolute inset-0 bg-cover bg-center bg-no-repeat"
                style={{ backgroundImage: "url('/images/hero-bg.png')" }}
            />

            {/* Dark Overlay with Gradient */}
            <div className="hero-overlay absolute inset-0 bg-gradient-to-b from-black/70 via-black/50 to-black/80" />

            {/* Animated Pattern Overlay */}
            <div className="absolute inset-0 opacity-10">
                <div className="absolute inset-0" style={{
                    backgroundImage: `url("data:image/svg+xml,%3Csvg width='60' height='60' viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'%3E%3Cg fill='none' fill-rule='evenodd'%3E%3Cg fill='%23C9A227' fill-opacity='0.4'%3E%3Cpath d='M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E")`,
                }} />
            </div>

            {/* Content */}
            <div className="relative z-10 text-center px-4 max-w-5xl mx-auto">
                {/* Badge */}
                <div className="inline-flex items-center gap-2 bg-white/10 backdrop-blur-sm border border-white/20 rounded-full px-4 py-2 mb-8 animate-fadeInUp">
                    <span className="w-2 h-2 bg-[#C9A227] rounded-full animate-pulse" />
                    <span className="text-white/90 text-sm font-medium">Uy tín hàng đầu Việt Nam từ năm 1995</span>
                </div>

                {/* Main Heading */}
                <h1 className="text-4xl md:text-6xl lg:text-7xl font-bold text-white mb-6 animate-fadeInUp" style={{ animationDelay: '0.2s' }}>
                    <span className="block mb-2">Lăng Mộ Đá</span>
                    <span className="text-gold-gradient">Trường Thành</span>
                </h1>

                {/* Tagline */}
                <p className="text-xl md:text-2xl text-white/80 mb-4 animate-fadeInUp" style={{ animationDelay: '0.4s' }}>
                    Nghệ Thuật Tâm Linh Vĩnh Cửu
                </p>

                {/* Subheading */}
                <p className="text-lg text-white/60 mb-10 max-w-2xl mx-auto animate-fadeInUp" style={{ animationDelay: '0.5s' }}>
                    Chuyên thiết kế, chế tác và lắp đặt lăng mộ đá nguyên khối cao cấp.<br />
                    Cam kết chất lượng - Bảo hành trọn đời - Giá gốc từ xưởng.
                </p>

                {/* CTA Buttons */}
                <div className="flex flex-col sm:flex-row items-center justify-center gap-4 mb-16 animate-fadeInUp" style={{ animationDelay: '0.6s' }}>
                    <a href="/san-pham" className="btn-primary text-lg px-8 py-4 flex items-center gap-2">
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
                        </svg>
                        Xem Sản Phẩm
                    </a>
                    <a href="tel:0987654321" className="btn-outline border-white text-white hover:bg-white hover:text-[#2D2D2D] text-lg px-8 py-4 flex items-center gap-2">
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
                        </svg>
                        Gọi Ngay: 0987.654.321
                    </a>
                </div>

                {/* Stats */}
                <div className="grid grid-cols-2 md:grid-cols-4 gap-6 md:gap-12 animate-fadeInUp" style={{ animationDelay: '0.8s' }}>
                    {[
                        { number: '30+', label: 'Năm Kinh Nghiệm' },
                        { number: '10.000+', label: 'Công Trình' },
                        { number: '63', label: 'Tỉnh Thành' },
                        { number: '100%', label: 'Bảo Hành' },
                    ].map((stat, index) => (
                        <div key={index} className="text-center">
                            <div className="text-3xl md:text-4xl font-bold text-[#C9A227] mb-1">{stat.number}</div>
                            <div className="text-white/70 text-sm">{stat.label}</div>
                        </div>
                    ))}
                </div>
            </div>

            {/* Scroll Indicator */}
            <div className="absolute bottom-8 left-1/2 -translate-x-1/2 animate-bounce">
                <div className="w-6 h-10 border-2 border-white/50 rounded-full flex justify-center pt-2">
                    <div className="w-1 h-3 bg-white/70 rounded-full animate-pulse" />
                </div>
            </div>
        </section>
    );
}
