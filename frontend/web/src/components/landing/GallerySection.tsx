'use client';

import { useState } from 'react';

export default function GallerySection() {
    const [activeFilter, setActiveFilter] = useState('all');
    const [lightboxImage, setLightboxImage] = useState<string | null>(null);

    const filters = [
        { id: 'all', label: 'Tất cả' },
        { id: 'lang-mo', label: 'Lăng mộ' },
        { id: 'mo-da', label: 'Mộ đá' },
        { id: 'mo-thap', label: 'Mộ tháp' },
        { id: 'tuong-da', label: 'Tượng đá' },
    ];

    const projects = [
        { id: 1, image: '/images/lang-mo-da-complex.png', category: 'lang-mo', title: 'Lăng mộ đá xanh Thanh Hóa', location: 'Ninh Bình' },
        { id: 2, image: '/images/mo-da-single.png', category: 'mo-da', title: 'Mộ đá granite đen', location: 'Hà Nội' },
        { id: 3, image: '/images/mo-thap-stupa.png', category: 'mo-thap', title: 'Mộ tháp Phật giáo', location: 'Huế' },
        { id: 4, image: '/images/tuong-phat-quan-am.png', category: 'tuong-da', title: 'Tượng Phật bà Quan Âm', location: 'TP.HCM' },
        { id: 5, image: '/images/lang-mo-da-complex.png', category: 'lang-mo', title: 'Khu lăng mộ gia tộc', location: 'Nam Định' },
        { id: 6, image: '/images/mo-da-single.png', category: 'mo-da', title: 'Mộ đá trắng nguyên khối', location: 'Bắc Ninh' },
        { id: 7, image: '/images/mo-da-single.png', category: 'lang-mo', title: 'Lăng mộ đá hoa cương', location: 'Thái Bình' },
        { id: 8, image: '/images/tuong-phat-quan-am.png', category: 'tuong-da', title: 'Tượng rồng phong thủy', location: 'Hải Phòng' },
    ];

    const filteredProjects = activeFilter === 'all'
        ? projects
        : projects.filter(p => p.category === activeFilter);

    return (
        <section className="section-padding bg-[#F9F7F4]">
            <div className="max-w-7xl mx-auto">
                {/* Section Header */}
                <div className="text-center mb-12">
                    <div className="divider-ornament mb-4">
                        <span className="text-[#C9A227] text-sm font-semibold tracking-widest uppercase">Công Trình</span>
                    </div>
                    <h2 className="heading-primary text-3xl md:text-4xl lg:text-5xl mb-4">
                        Công Trình <span className="heading-accent">Tiêu Biểu</span>
                    </h2>
                    <p className="text-[#6B6B6B] max-w-2xl mx-auto text-lg">
                        Những công trình lăng mộ đá cao cấp đã được hoàn thành trên khắp cả nước
                    </p>
                </div>

                {/* Filter Tabs */}
                <div className="flex flex-wrap justify-center gap-2 mb-10">
                    {filters.map((filter) => (
                        <button
                            key={filter.id}
                            onClick={() => setActiveFilter(filter.id)}
                            className={`px-6 py-2 rounded-full font-medium transition-all ${activeFilter === filter.id
                                ? 'bg-[#C9A227] text-white shadow-lg'
                                : 'bg-white text-[#6B6B6B] hover:bg-[#C9A227]/10'
                                }`}
                        >
                            {filter.label}
                        </button>
                    ))}
                </div>

                {/* Gallery Grid */}
                <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-4">
                    {filteredProjects.map((project, index) => (
                        <div
                            key={project.id}
                            className={`group relative overflow-hidden rounded-xl cursor-pointer ${index === 0 ? 'sm:col-span-2 sm:row-span-2' : ''
                                }`}
                            onClick={() => setLightboxImage(project.image)}
                        >
                            <img
                                src={project.image}
                                alt={project.title}
                                className={`w-full object-cover transition-transform duration-500 group-hover:scale-110 ${index === 0 ? 'h-full min-h-[400px]' : 'h-64'
                                    }`}
                            />
                            {/* Overlay */}
                            <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/30 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                                <div className="absolute bottom-4 left-4 right-4">
                                    <h3 className="text-white font-bold text-lg mb-1">{project.title}</h3>
                                    <p className="text-white/80 text-sm flex items-center gap-1">
                                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                                        </svg>
                                        {project.location}
                                    </p>
                                </div>
                                {/* Zoom Icon */}
                                <div className="absolute top-4 right-4 w-10 h-10 bg-white/20 backdrop-blur-sm rounded-full flex items-center justify-center">
                                    <svg className="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0zM10 7v3m0 0v3m0-3h3m-3 0H7" />
                                    </svg>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>

                {/* View More */}
                <div className="text-center mt-10">
                    <a href="/cong-trinh" className="btn-outline border-[#C9A227] text-[#C9A227] hover:bg-[#C9A227] hover:text-white inline-flex items-center gap-2">
                        Xem Tất Cả Công Trình
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" />
                        </svg>
                    </a>
                </div>
            </div>

            {/* Lightbox */}
            {lightboxImage && (
                <div
                    className="fixed inset-0 z-50 bg-black/90 flex items-center justify-center p-4"
                    onClick={() => setLightboxImage(null)}
                >
                    <button
                        className="absolute top-4 right-4 w-10 h-10 bg-white/10 rounded-full flex items-center justify-center text-white hover:bg-white/20"
                        onClick={() => setLightboxImage(null)}
                    >
                        <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                        </svg>
                    </button>
                    <img
                        src={lightboxImage}
                        alt="Gallery"
                        className="max-w-full max-h-[90vh] object-contain rounded-lg"
                        onClick={(e) => e.stopPropagation()}
                    />
                </div>
            )}
        </section>
    );
}
