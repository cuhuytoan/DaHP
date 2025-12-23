'use client';

import { useState } from 'react';

export default function TestimonialsSection() {
    const [activeIndex, setActiveIndex] = useState(0);

    const testimonials = [
        {
            id: 1,
            name: 'Ông Nguyễn Văn Hùng',
            location: 'Ninh Bình',
            avatar: 'https://randomuser.me/api/portraits/men/1.jpg',
            rating: 5,
            content: 'Chúng tôi rất hài lòng với công trình lăng mộ do Trường Thành thi công. Chất lượng đá rất đẹp, đường nét điêu khắc tinh xảo. Đội ngũ làm việc chuyên nghiệp, đúng tiến độ.',
        },
        {
            id: 2,
            name: 'Bà Trần Thị Mai',
            location: 'Nam Định',
            avatar: 'https://randomuser.me/api/portraits/women/2.jpg',
            rating: 5,
            content: 'Gia đình tôi đã đặt làm khu lăng mộ gia tộc. Giá cả hợp lý, chất lượng vượt mong đợi. Cảm ơn Trường Thành đã giúp gia đình hoàn thành tâm nguyện.',
        },
        {
            id: 3,
            name: 'Ông Phạm Đức Thắng',
            location: 'Hà Nội',
            avatar: 'https://randomuser.me/api/portraits/men/3.jpg',
            rating: 5,
            content: 'Được bạn bè giới thiệu và thực sự không thất vọng. Đá nguyên khối, điêu khắc rồng phượng rất sống động. Nhân viên tư vấn nhiệt tình, hỗ trợ xem phong thủy.',
        },
        {
            id: 4,
            name: 'Ông Lê Minh Tuấn',
            location: 'Thái Bình',
            avatar: 'https://randomuser.me/api/portraits/men/4.jpg',
            rating: 5,
            content: 'Đã làm 2 công trình với Trường Thành, lần nào cũng ưng ý. Bảo hành tốt, mấy năm sau vẫn đến hỗ trợ khi cần. Xứng đáng là đơn vị uy tín hàng đầu.',
        },
    ];

    return (
        <section className="section-padding bg-white">
            <div className="max-w-7xl mx-auto">
                {/* Section Header */}
                <div className="text-center mb-16">
                    <div className="divider-ornament mb-4">
                        <span className="text-[#C9A227] text-sm font-semibold tracking-widest uppercase">Đánh Giá</span>
                    </div>
                    <h2 className="heading-primary text-3xl md:text-4xl lg:text-5xl mb-4">
                        Khách Hàng <span className="heading-accent">Nói Gì</span>
                    </h2>
                    <p className="text-[#6B6B6B] max-w-2xl mx-auto text-lg">
                        Niềm tin và sự hài lòng của quý khách hàng là động lực lớn nhất của chúng tôi
                    </p>
                </div>

                {/* Testimonials Slider */}
                <div className="relative">
                    <div className="grid md:grid-cols-2 gap-6">
                        {testimonials.slice(activeIndex, activeIndex + 2).map((testimonial) => (
                            <div
                                key={testimonial.id}
                                className="bg-[#F9F7F4] rounded-2xl p-8 relative"
                            >
                                {/* Quote Icon */}
                                <div className="absolute top-6 right-6 text-[#C9A227]/20">
                                    <svg className="w-12 h-12" fill="currentColor" viewBox="0 0 24 24">
                                        <path d="M14.017 21v-7.391c0-5.704 3.731-9.57 8.983-10.609l.995 2.151c-2.432.917-3.995 3.638-3.995 5.849h4v10h-9.983zm-14.017 0v-7.391c0-5.704 3.748-9.57 9-10.609l.996 2.151c-2.433.917-3.996 3.638-3.996 5.849h3.983v10h-9.983z" />
                                    </svg>
                                </div>

                                {/* Rating */}
                                <div className="flex gap-1 mb-4">
                                    {[...Array(testimonial.rating)].map((_, i) => (
                                        <svg key={i} className="w-5 h-5 text-[#C9A227]" fill="currentColor" viewBox="0 0 20 20">
                                            <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                                        </svg>
                                    ))}
                                </div>

                                {/* Content */}
                                <p className="text-[#4A4A4A] leading-relaxed mb-6 italic">
                                    "{testimonial.content}"
                                </p>

                                {/* Author */}
                                <div className="flex items-center gap-4">
                                    <img
                                        src={testimonial.avatar}
                                        alt={testimonial.name}
                                        className="w-14 h-14 rounded-full object-cover border-2 border-[#C9A227]"
                                    />
                                    <div>
                                        <h4 className="font-bold text-[#2D2D2D]">{testimonial.name}</h4>
                                        <p className="text-[#6B6B6B] text-sm flex items-center gap-1">
                                            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                            </svg>
                                            {testimonial.location}
                                        </p>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>

                    {/* Navigation Dots */}
                    <div className="flex justify-center gap-2 mt-8">
                        {[0, 2].map((index) => (
                            <button
                                key={index}
                                onClick={() => setActiveIndex(index)}
                                className={`w-3 h-3 rounded-full transition-all ${activeIndex === index ? 'bg-[#C9A227] w-8' : 'bg-[#C9A227]/30'
                                    }`}
                            />
                        ))}
                    </div>
                </div>

                {/* Trust Stats */}
                <div className="mt-16 grid grid-cols-2 md:grid-cols-4 gap-6 text-center">
                    {[
                        { number: '10.000+', label: 'Khách hàng' },
                        { number: '5.0', label: 'Đánh giá trung bình' },
                        { number: '99%', label: 'Hài lòng' },
                        { number: '95%', label: 'Giới thiệu bạn bè' },
                    ].map((stat, index) => (
                        <div key={index} className="p-6 bg-[#F9F7F4] rounded-xl">
                            <div className="text-3xl font-bold text-[#C9A227] mb-1">{stat.number}</div>
                            <div className="text-[#6B6B6B] text-sm">{stat.label}</div>
                        </div>
                    ))}
                </div>
            </div>
        </section>
    );
}
