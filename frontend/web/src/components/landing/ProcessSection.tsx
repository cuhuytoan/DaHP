'use client';

export default function ProcessSection() {
    const steps = [
        {
            number: '01',
            title: 'Tư Vấn & Thiết Kế',
            description: 'Đội ngũ tư vấn viên lắng nghe nhu cầu, đề xuất phương án thiết kế phù hợp phong thủy và ngân sách',
            icon: (
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z" />
                </svg>
            ),
        },
        {
            number: '02',
            title: 'Báo Giá Chi Tiết',
            description: 'Báo giá minh bạch, chi tiết từng hạng mục, không phát sinh chi phí ngoài hợp đồng',
            icon: (
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9 7h6m0 10v-3m-3 3h.01M9 17h.01M9 14h.01M12 14h.01M15 11h.01M12 11h.01M9 11h.01M7 21h10a2 2 0 002-2V5a2 2 0 00-2-2H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
                </svg>
            ),
        },
        {
            number: '03',
            title: 'Sản Xuất Tại Xưởng',
            description: 'Chế tác tại xưởng chính, nghệ nhân lành nghề với kỹ thuật truyền thống kết hợp công nghệ hiện đại',
            icon: (
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L6.05 15.21a2 2 0 00-1.806.547M8 4h8l-1 1v5.172a2 2 0 00.586 1.414l5 5c1.26 1.26.367 3.414-1.415 3.414H4.828c-1.782 0-2.674-2.154-1.414-3.414l5-5A2 2 0 009 10.172V5L8 4z" />
                </svg>
            ),
        },
        {
            number: '04',
            title: 'Vận Chuyển & Lắp Đặt',
            description: 'Đội xe chuyên dụng vận chuyển an toàn, đội ngũ kỹ thuật lắp đặt chuyên nghiệp tận nơi',
            icon: (
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M8.111 16.404a5.5 5.5 0 017.778 0M12 20h.01m-7.08-7.071c3.904-3.905 10.236-3.905 14.141 0M1.394 9.393c5.857-5.857 15.355-5.857 21.213 0" />
                </svg>
            ),
        },
        {
            number: '05',
            title: 'Nghiệm Thu & Bàn Giao',
            description: 'Kiểm tra chất lượng kỹ lưỡng, bàn giao công trình hoàn hảo, hướng dẫn bảo quản',
            icon: (
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
            ),
        },
    ];

    return (
        <section className="section-padding bg-white">
            <div className="max-w-7xl mx-auto">
                {/* Section Header */}
                <div className="text-center mb-16">
                    <div className="divider-ornament mb-4">
                        <span className="text-[#C9A227] text-sm font-semibold tracking-widest uppercase">Quy Trình</span>
                    </div>
                    <h2 className="heading-primary text-3xl md:text-4xl lg:text-5xl mb-4">
                        Quy Trình <span className="heading-accent">Làm Việc</span>
                    </h2>
                    <p className="text-[#6B6B6B] max-w-2xl mx-auto text-lg">
                        5 bước đơn giản để sở hữu công trình lăng mộ đá ưng ý
                    </p>
                </div>

                {/* Process Timeline */}
                <div className="relative">
                    {/* Timeline Line (Desktop) */}
                    <div className="hidden lg:block absolute top-24 left-0 right-0 h-0.5 bg-gradient-to-r from-transparent via-[#C9A227] to-transparent" />

                    <div className="grid md:grid-cols-2 lg:grid-cols-5 gap-8">
                        {steps.map((step, index) => (
                            <div
                                key={index}
                                className="relative text-center group"
                            >
                                {/* Step Circle */}
                                <div className="relative z-10 mx-auto w-20 h-20 bg-[#F9F7F4] border-2 border-[#C9A227] rounded-full flex items-center justify-center mb-6 group-hover:bg-[#C9A227] transition-colors duration-300">
                                    <div className="text-[#C9A227] group-hover:text-white transition-colors">
                                        {step.icon}
                                    </div>
                                </div>

                                {/* Step Number */}
                                <div className="absolute -top-2 left-1/2 -translate-x-1/2 w-8 h-8 bg-[#C9A227] text-white rounded-full flex items-center justify-center text-sm font-bold shadow-lg">
                                    {step.number}
                                </div>

                                {/* Content */}
                                <h3 className="text-lg font-bold text-[#2D2D2D] mb-3">{step.title}</h3>
                                <p className="text-[#6B6B6B] text-sm leading-relaxed">{step.description}</p>
                            </div>
                        ))}
                    </div>
                </div>

                {/* CTA */}
                <div className="text-center mt-16 bg-[#F9F7F4] rounded-2xl p-8 md:p-12">
                    <h3 className="text-2xl md:text-3xl font-bold text-[#2D2D2D] mb-4">
                        Bắt đầu ngay hôm nay
                    </h3>
                    <p className="text-[#6B6B6B] mb-6 max-w-xl mx-auto">
                        Liên hệ với chúng tôi để được tư vấn miễn phí và nhận báo giá chi tiết
                    </p>
                    <a href="/lien-he" className="btn-primary inline-flex items-center gap-2">
                        Nhận Tư Vấn Miễn Phí
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" />
                        </svg>
                    </a>
                </div>
            </div>
        </section>
    );
}
