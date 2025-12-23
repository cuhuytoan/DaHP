'use client';

import Header from '@/components/landing/Header';
import HeroSection from '@/components/landing/HeroSection';
import AboutSection from '@/components/landing/AboutSection';
import ProductsSection from '@/components/landing/ProductsSection';
import WhyChooseUsSection from '@/components/landing/WhyChooseUsSection';
import ProcessSection from '@/components/landing/ProcessSection';
import GallerySection from '@/components/landing/GallerySection';
import TestimonialsSection from '@/components/landing/TestimonialsSection';
import ContactSection from '@/components/landing/ContactSection';
import Footer from '@/components/landing/Footer';

export default function HomePage() {
  return (
    <main className="min-h-screen">
      <Header />
      <HeroSection />
      <AboutSection />
      <ProductsSection />
      <WhyChooseUsSection />
      <ProcessSection />
      <GallerySection />
      <TestimonialsSection />
      <ContactSection />
      <Footer />

      {/* Floating CTA Buttons */}
      <div className="fixed bottom-6 right-6 z-50 flex flex-col gap-3">
        {/* Phone Button */}
        <a
          href="tel:0987654321"
          className="w-14 h-14 bg-[#C9A227] rounded-full flex items-center justify-center shadow-lg hover:scale-110 transition-transform animate-pulse"
          title="Gọi ngay"
        >
          <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
          </svg>
        </a>

        {/* Zalo Button */}
        <a
          href="https://zalo.me/0987654321"
          className="w-14 h-14 bg-blue-500 rounded-full flex items-center justify-center shadow-lg hover:scale-110 transition-transform"
          title="Chat Zalo"
        >
          <span className="text-white font-bold text-sm">Zalo</span>
        </a>
      </div>

      {/* Back to Top Button */}
      <BackToTopButton />
    </main>
  );
}

function BackToTopButton() {
  const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  return (
    <button
      onClick={scrollToTop}
      className="fixed bottom-6 left-6 z-50 w-12 h-12 bg-[#2D2D2D] text-white rounded-full flex items-center justify-center shadow-lg hover:bg-[#C9A227] transition-colors"
      title="Lên đầu trang"
    >
      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 10l7-7m0 0l7 7m-7-7v18" />
      </svg>
    </button>
  );
}
