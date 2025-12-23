'use client';

export default function AboutSection() {
    const features = [
        {
            icon: (
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z" />
                </svg>
            ),
            title: 'Uy Tín 30 Năm',
            desc: 'Thương hiệu hàng đầu được hàng ngàn gia đình tin tưởng',
        },
        {
            icon: (
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
                </svg>
            ),
            title: 'Đá Nguyên Khối',
            desc: 'Chất liệu đá tự nhiên cao cấp, bền vững vĩnh cửu',
        },
        {
            icon: (
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9.663 17h4.673M12 3v1m6.364 1.636l-.707.707M21 12h-1M4 12H3m3.343-5.657l-.707-.707m2.828 9.9a5 5 0 117.072 0l-.548.547A3.374 3.374 0 0014 18.469V19a2 2 0 11-4 0v-.531c0-.895-.356-1.754-.988-2.386l-.548-.547z" />
                </svg>
            ),
            title: 'Thiết Kế Độc Đáo',
            desc: 'Mẫu mã đa dạng, thiết kế theo yêu cầu riêng',
        },
    ];

    return (
        <section className="section-padding bg-white">
            <div className="max-w-7xl mx-auto">
                {/* Section Header */}
                <div className="text-center mb-16">
                    <div className="divider-ornament mb-4">
                        <span className="text-[#C9A227] text-sm font-semibold tracking-widest uppercase">Về Chúng Tôi</span>
                    </div>
                    <h2 className="heading-primary text-3xl md:text-4xl lg:text-5xl mb-4">
                        Truyền Thống <span className="heading-accent">Tinh Hoa</span>
                    </h2>
                    <p className="text-[#6B6B6B] max-w-2xl mx-auto text-lg">
                        Ba thế hệ nghệ nhân với tâm huyết và kỹ năng điêu luyện,
                        mang đến những công trình lăng mộ đá đẳng cấp nhất.
                    </p>
                </div>

                {/* Content Grid */}
                <div className="grid lg:grid-cols-2 gap-12 items-center">
                    {/* Image Side */}
                    <div className="relative">
                        <div className="relative z-10 rounded-2xl overflow-hidden shadow-2xl">
                            <img
                                src="/images/about.png"
                                alt="Nghệ nhân Trường Thành đang chế tác"
                                className="w-full h-[500px] object-cover"
                            />
                        </div>
                        {/* Decorative Elements */}
                        <div className="absolute -bottom-6 -right-6 w-48 h-48 bg-[#C9A227]/10 rounded-2xl -z-10" />
                        <div className="absolute -top-6 -left-6 w-32 h-32 border-2 border-[#C9A227]/30 rounded-2xl -z-10" />

                        {/* Experience Badge */}
                        <div className="absolute -bottom-8 left-8 bg-[#2D2D2D] text-white px-8 py-6 rounded-xl shadow-xl">
                            <div className="text-4xl font-bold text-[#C9A227]">30+</div>
                            <div className="text-sm text-white/80">Năm Kinh Nghiệm</div>
                        </div>
                    </div>

                    {/* Content Side */}
                    <div className="lg:pl-8">
                        <h3 className="text-2xl md:text-3xl font-bold text-[#2D2D2D] mb-6">
                            Nghệ Thuật Điêu Khắc Đá <br />
                            <span className="text-[#C9A227]">Truyền Thống & Hiện Đại</span>
                        </h3>

                        <p className="text-[#6B6B6B] mb-6 leading-relaxed">
                            Lăng Mộ Đá Trường Thành tự hào là đơn vị tiên phong trong lĩnh vực
                            thiết kế và thi công lăng mộ đá cao cấp tại Việt Nam. Với đội ngũ
                            nghệ nhân lành nghề và công nghệ hiện đại, chúng tôi cam kết mang
                            đến những công trình hoàn hảo nhất.
                        </p>

                        <p className="text-[#6B6B6B] mb-8 leading-relaxed">
                            Mỗi sản phẩm đều được chế tác từ đá tự nhiên nguyên khối,
                            kết hợp hài hòa giữa nghệ thuật điêu khắc truyền thống và
                            phong cách thiết kế đương đại.
                        </p>

                        {/* Features */}
                        <div className="space-y-4 mb-8">
                            {features.map((feature, index) => (
                                <div key={index} className="flex items-start gap-4">
                                    <div className="w-14 h-14 bg-[#F9F7F4] rounded-xl flex items-center justify-center text-[#C9A227] flex-shrink-0">
                                        {feature.icon}
                                    </div>
                                    <div>
                                        <h4 className="font-semibold text-[#2D2D2D] mb-1">{feature.title}</h4>
                                        <p className="text-[#6B6B6B] text-sm">{feature.desc}</p>
                                    </div>
                                </div>
                            ))}
                        </div>

                        <a href="/gioi-thieu" className="btn-primary inline-flex items-center gap-2">
                            Tìm Hiểu Thêm
                            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" />
                            </svg>
                        </a>
                    </div>
                </div>
            </div>
        </section>
    );
}
