'use client';

import { useState } from 'react';

export default function ContactSection() {
    const [formData, setFormData] = useState({
        name: '',
        phone: '',
        email: '',
        message: '',
    });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        // Handle form submission
        console.log(formData);
        alert('Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi trong thời gian sớm nhất.');
    };

    return (
        <section className="section-padding bg-[#2D2D2D] relative overflow-hidden">
            {/* Background Pattern */}
            <div className="absolute inset-0 opacity-5">
                <div className="absolute inset-0" style={{
                    backgroundImage: `url("data:image/svg+xml,%3Csvg width='60' height='60' viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'%3E%3Cg fill='none' fill-rule='evenodd'%3E%3Cg fill='%23C9A227' fill-opacity='0.4'%3E%3Cpath d='M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E")`,
                }} />
            </div>

            <div className="max-w-7xl mx-auto relative z-10">
                <div className="grid lg:grid-cols-2 gap-12 items-start">
                    {/* Contact Info */}
                    <div>
                        <div className="mb-8">
                            <span className="text-[#C9A227] text-sm font-semibold tracking-widest uppercase">Liên Hệ</span>
                            <h2 className="text-3xl md:text-4xl font-bold text-white mt-2 mb-4" style={{ fontFamily: 'var(--font-serif)' }}>
                                Tư Vấn <span className="text-[#C9A227]">Miễn Phí</span>
                            </h2>
                            <p className="text-white/70 text-lg">
                                Để lại thông tin, chúng tôi sẽ liên hệ tư vấn trong vòng 15 phút
                            </p>
                        </div>

                        {/* Contact Cards */}
                        <div className="space-y-4 mb-8">
                            <a href="tel:0987654321" className="flex items-center gap-4 bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-4 hover:bg-white/10 transition-colors group">
                                <div className="w-14 h-14 bg-[#C9A227] rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform">
                                    <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
                                    </svg>
                                </div>
                                <div>
                                    <p className="text-white/60 text-sm">Hotline 24/7</p>
                                    <p className="text-white font-bold text-xl">0987.654.321</p>
                                </div>
                            </a>

                            <a href="https://zalo.me/0987654321" className="flex items-center gap-4 bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-4 hover:bg-white/10 transition-colors group">
                                <div className="w-14 h-14 bg-blue-500 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform">
                                    <span className="text-white font-bold text-lg">Zalo</span>
                                </div>
                                <div>
                                    <p className="text-white/60 text-sm">Chat Zalo</p>
                                    <p className="text-white font-bold text-xl">Trường Thành Stone</p>
                                </div>
                            </a>

                            <div className="flex items-center gap-4 bg-white/5 backdrop-blur-sm border border-white/10 rounded-xl p-4">
                                <div className="w-14 h-14 bg-[#C9A227]/20 rounded-xl flex items-center justify-center">
                                    <svg className="w-6 h-6 text-[#C9A227]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                                    </svg>
                                </div>
                                <div>
                                    <p className="text-white/60 text-sm">Địa chỉ xưởng</p>
                                    <p className="text-white font-medium">Xã Ninh Vân, Hoa Lư, Ninh Bình</p>
                                </div>
                            </div>
                        </div>

                        {/* Working Hours */}
                        <div className="bg-[#C9A227]/10 border border-[#C9A227]/30 rounded-xl p-6">
                            <h4 className="text-[#C9A227] font-bold mb-3">Giờ làm việc</h4>
                            <div className="space-y-2 text-white/80">
                                <div className="flex justify-between">
                                    <span>Thứ 2 - Thứ 7</span>
                                    <span className="font-medium text-white">7:00 - 18:00</span>
                                </div>
                                <div className="flex justify-between">
                                    <span>Chủ nhật</span>
                                    <span className="font-medium text-white">8:00 - 12:00</span>
                                </div>
                                <div className="flex justify-between">
                                    <span>Hotline</span>
                                    <span className="font-medium text-[#C9A227]">24/7</span>
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Contact Form */}
                    <div className="bg-white rounded-2xl p-8 shadow-2xl">
                        <h3 className="text-2xl font-bold text-[#2D2D2D] mb-6">Gửi yêu cầu tư vấn</h3>

                        <form onSubmit={handleSubmit} className="space-y-5">
                            <div>
                                <label className="block text-sm font-medium text-[#4A4A4A] mb-2">Họ và tên *</label>
                                <input
                                    type="text"
                                    required
                                    value={formData.name}
                                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                                    className="w-full px-4 py-3 border border-[#E5E2DC] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#C9A227] focus:border-transparent"
                                    placeholder="Nhập họ tên của bạn"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-[#4A4A4A] mb-2">Số điện thoại *</label>
                                <input
                                    type="tel"
                                    required
                                    value={formData.phone}
                                    onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                                    className="w-full px-4 py-3 border border-[#E5E2DC] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#C9A227] focus:border-transparent"
                                    placeholder="Nhập số điện thoại"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-[#4A4A4A] mb-2">Email</label>
                                <input
                                    type="email"
                                    value={formData.email}
                                    onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                                    className="w-full px-4 py-3 border border-[#E5E2DC] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#C9A227] focus:border-transparent"
                                    placeholder="Nhập email (không bắt buộc)"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-[#4A4A4A] mb-2">Nội dung tư vấn</label>
                                <textarea
                                    rows={4}
                                    value={formData.message}
                                    onChange={(e) => setFormData({ ...formData, message: e.target.value })}
                                    className="w-full px-4 py-3 border border-[#E5E2DC] rounded-lg focus:outline-none focus:ring-2 focus:ring-[#C9A227] focus:border-transparent resize-none"
                                    placeholder="Mô tả nhu cầu của bạn..."
                                />
                            </div>

                            <button type="submit" className="btn-primary w-full text-lg py-4">
                                Gửi Yêu Cầu Tư Vấn
                            </button>

                            <p className="text-center text-sm text-[#6B6B6B]">
                                Chúng tôi sẽ liên hệ lại trong vòng <span className="text-[#C9A227] font-medium">15 phút</span>
                            </p>
                        </form>
                    </div>
                </div>
            </div>
        </section>
    );
}
