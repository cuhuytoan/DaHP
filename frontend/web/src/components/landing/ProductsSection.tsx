'use client';

import Link from 'next/link';

export default function ProductsSection() {
    const categories = [
        {
            id: 1,
            name: 'Lăng Mộ Đá',
            description: 'Khu lăng mộ đá nguyên khối cao cấp',
            image: '/images/lang-mo-da-complex.png',
            count: 150,
            slug: 'lang-mo-da',
        },
        {
            id: 2,
            name: 'Mộ Đá',
            description: 'Mộ đá đơn, đôi, gia tộc',
            image: '/images/mo-da-single.png',
            count: 200,
            slug: 'mo-da',
        },
        {
            id: 3,
            name: 'Mộ Tháp',
            description: 'Mộ tháp Phật giáo tinh xảo',
            image: '/images/mo-thap-stupa.png',
            count: 80,
            slug: 'mo-thap',
        },
        {
            id: 4,
            name: 'Bàn Thờ Đá',
            description: 'Bàn thờ thiên, thờ gia tiên',
            image: '/images/ban-tho-da.png',
            count: 120,
            slug: 'ban-tho-da',
        },
        {
            id: 5,
            name: 'Tượng Đá',
            description: 'Tượng Phật, tượng linh vật',
            image: '/images/tuong-phat-quan-am.png',
            count: 90,
            slug: 'tuong-da',
        },
        {
            id: 6,
            name: 'Cuốn Thư Đá',
            description: 'Cuốn thư, cột đá, lan can',
            image: '/images/lang-mo-da-complex.png',
            count: 60,
            slug: 'cuon-thu-da',
        },
    ];

    return (
        <section className="section-padding bg-[#F9F7F4]">
            <div className="max-w-7xl mx-auto">
                {/* Section Header */}
                <div className="text-center mb-16">
                    <div className="divider-ornament mb-4">
                        <span className="text-[#C9A227] text-sm font-semibold tracking-widest uppercase">Sản Phẩm</span>
                    </div>
                    <h2 className="heading-primary text-3xl md:text-4xl lg:text-5xl mb-4">
                        Danh Mục <span className="heading-accent">Sản Phẩm</span>
                    </h2>
                    <p className="text-[#6B6B6B] max-w-2xl mx-auto text-lg">
                        Đa dạng các dòng sản phẩm lăng mộ đá, phục vụ mọi nhu cầu của quý khách hàng
                    </p>
                </div>

                {/* Categories Grid */}
                <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-6">
                    {categories.map((category, index) => (
                        <Link
                            key={category.id}
                            href={`/san-pham/${category.slug}`}
                            className="group card-hover bg-white rounded-2xl overflow-hidden shadow-lg"
                            style={{ animationDelay: `${index * 0.1}s` }}
                        >
                            {/* Image */}
                            <div className="relative h-56 overflow-hidden">
                                <img
                                    src={category.image}
                                    alt={category.name}
                                    className="w-full h-full object-cover transition-transform duration-500 group-hover:scale-110"
                                />
                                <div className="absolute inset-0 bg-gradient-to-t from-black/60 via-transparent to-transparent" />

                                {/* Count Badge */}
                                <div className="absolute top-4 right-4 bg-[#C9A227] text-white px-3 py-1 rounded-full text-sm font-medium">
                                    {category.count}+ mẫu
                                </div>

                                {/* Category Name Overlay */}
                                <div className="absolute bottom-4 left-4 right-4">
                                    <h3 className="text-xl font-bold text-white mb-1">{category.name}</h3>
                                    <p className="text-white/80 text-sm">{category.description}</p>
                                </div>
                            </div>

                            {/* Hover Indicator */}
                            <div className="p-4 flex items-center justify-between bg-white group-hover:bg-[#C9A227] transition-colors">
                                <span className="font-medium text-[#2D2D2D] group-hover:text-white transition-colors">
                                    Xem chi tiết
                                </span>
                                <svg
                                    className="w-5 h-5 text-[#C9A227] group-hover:text-white transition-all group-hover:translate-x-2"
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                >
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" />
                                </svg>
                            </div>
                        </Link>
                    ))}
                </div>

                {/* View All Button */}
                <div className="text-center mt-12">
                    <Link href="/san-pham" className="btn-primary inline-flex items-center gap-2">
                        Xem Tất Cả Sản Phẩm
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" />
                        </svg>
                    </Link>
                </div>
            </div>
        </section>
    );
}
