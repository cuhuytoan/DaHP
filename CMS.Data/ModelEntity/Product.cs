﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CMS.Data.ModelEntity
{
    public partial class Product
    {
        /// <summary>
        /// Id tự tăng
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Kiểu sản phẩm
        /// </summary>
        public int? ProductTypeId { get; set; }
        /// <summary>
        /// Chủng loại hàng hóa
        /// </summary>
        [StringLength(200)]
        public string ProductCategoryIds { get; set; }
        /// <summary>
        /// Hãng sản xuất
        /// </summary>
        public int? ProductManufactureId { get; set; }
        /// <summary>
        /// Doanh nghiệp
        /// </summary>
        public int? ProductBrandId { get; set; }
        /// <summary>
        /// Xuất xứ
        /// </summary>
        public int? CountryId { get; set; }
        /// <summary>
        /// Trạng thái sản phẩm
        /// </summary>
        public int? ProductStatusId { get; set; }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        [StringLength(1000)]
        public string Name { get; set; }
        /// <summary>
        /// Mã vạch sản phẩm
        /// </summary>
        [StringLength(50)]
        public string BarCode { get; set; }
        [Column("ManufactureSKU")]
        [StringLength(100)]
        public string ManufactureSku { get; set; }
        [Column("SKU")]
        [StringLength(100)]
        public string Sku { get; set; }
        /// <summary>
        /// Mã QRCode sản phẩm
        /// </summary>
        [Column("QRCodePublic")]
        [StringLength(100)]
        public string QrcodePublic { get; set; }
        /// <summary>
        /// Tiêu đề phụ (chưa dùng)
        /// </summary>
        [StringLength(200)]
        public string SubTitle { get; set; }
        /// <summary>
        /// Tên file ảnh đại diện chính
        /// </summary>
        [StringLength(200)]
        public string Image { get; set; }
        /// <summary>
        /// (chưa dùng)
        /// </summary>
        [StringLength(200)]
        public string ImageDescription { get; set; }
        /// <summary>
        /// (chưa dùng)
        /// </summary>
        [StringLength(200)]
        public string BannerImage { get; set; }
        /// <summary>
        /// Mô tả tóm lược sản phẩm
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Mô tả chi tiết sản phẩm
        /// </summary>
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        /// <summary>
        /// Thông số kỹ thuật
        /// </summary>
        [Column(TypeName = "ntext")]
        public string Specification { get; set; }
        /// <summary>
        /// Các chứng chỉ đạt được
        /// </summary>
        [Column(TypeName = "ntext")]
        public string ProductCertificate { get; set; }
        /// <summary>
        /// Thông tin pháp lý
        /// </summary>
        [StringLength(4000)]
        public string LegalInfo { get; set; }
        /// <summary>
        /// Giá
        /// </summary>
        [Column(TypeName = "money")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Giá cũ (để mờ hoặc gạch ngang)
        /// </summary>
        [Column(TypeName = "money")]
        public decimal? PriceOld { get; set; }
        /// <summary>
        /// Giá
        /// </summary>
        [Column(TypeName = "money")]
        public decimal? PriceWholesale { get; set; }
        public int? WholesaleMin { get; set; }
        /// <summary>
        /// Giảm giá (tính bằng tiền)
        /// </summary>
        [Column(TypeName = "money")]
        public decimal? Discount { get; set; }
        /// <summary>
        /// Giảm giá (tính bằng %)
        /// </summary>
        public int? DiscountRate { get; set; }
        /// <summary>
        /// Hàng cũ
        /// </summary>
        public bool? IsSecondHand { get; set; }
        /// <summary>
        /// Hàng chính hãng
        /// </summary>
        public bool? IsAuthor { get; set; }
        /// <summary>
        /// Hàng bán chạy
        /// </summary>
        public bool? IsBestSale { get; set; }
        /// <summary>
        /// Hàng giảm giá
        /// </summary>
        public bool? IsSaleOff { get; set; }
        /// <summary>
        /// Hàng mới về
        /// </summary>
        public bool? IsNew { get; set; }
        /// <summary>
        /// Hàng sắp về
        /// </summary>
        public bool? IsComming { get; set; }
        /// <summary>
        /// Hết hàng
        /// </summary>
        public bool? IsOutStock { get; set; }
        /// <summary>
        /// Ngừng kinh doanh
        /// </summary>
        public bool? IsDiscontinue { get; set; }
        /// <summary>
        /// Khối lượng
        /// </summary>
        public int? AmountDefault { get; set; }
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        public int? UnitId { get; set; }
        /// <summary>
        /// Hạn sử dụng hiển thị trên web
        /// </summary>
        [StringLength(500)]
        public string ExpiryDisplay { get; set; }
        /// <summary>
        /// Hạn sử dụng tính theo ngày
        /// </summary>
        public int? ExpiryByDay { get; set; }
        /// <summary>
        /// Thời gian bảo hành hiển thị trên web
        /// </summary>
        [StringLength(500)]
        public string WarrantyDisplay { get; set; }
        /// <summary>
        /// Thời gian bảo hành tính theo tháng
        /// </summary>
        public int? WarrantyByMonth { get; set; }
        /// <summary>
        /// Điểm đánh giá
        /// </summary>
        public int? Rate { get; set; }
        /// <summary>
        /// Ngày bắt đầu đăng
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Ngày kết thúc đăng +100 năm
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Hoạt động
        /// </summary>
        public bool? Active { get; set; }
        /// <summary>
        /// Số lượt xem
        /// </summary>
        public int? Counter { get; set; }
        public int? LikeCount { get; set; }
        /// <summary>
        /// Số lượt bán
        /// </summary>
        public int? SellCount { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        [StringLength(450)]
        public string CreateBy { get; set; }
        /// <summary>
        /// Thời gian tạo
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Người chỉnh sửa cuối cùng
        /// </summary>
        [StringLength(450)]
        public string LastEditBy { get; set; }
        /// <summary>
        /// Thời gian chỉnh sửa cuối cùng
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? LastEditDate { get; set; }
        /// <summary>
        /// Trạng thái kiểm tra
        /// </summary>
        public int? Checked { get; set; }
        /// <summary>
        /// Người Kiểm tra
        /// </summary>
        [StringLength(450)]
        public string CheckBy { get; set; }
        /// <summary>
        /// Thời gian kiểm tra
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// Trạng thái duyệt
        /// </summary>
        public int? Approved { get; set; }
        /// <summary>
        /// Người duyệt
        /// </summary>
        [StringLength(450)]
        public string ApproveBy { get; set; }
        /// <summary>
        /// Thời gian duyệt
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? ApproveDate { get; set; }
        /// <summary>
        /// Đường dẫn
        /// </summary>
        [Column("URL")]
        [StringLength(1000)]
        public string Url { get; set; }
        /// <summary>
        /// Tag (cách nhau dấu ,)
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// Cho phép copy trên giao diện người dùng
        /// </summary>
        public bool? CanCopy { get; set; }
        /// <summary>
        /// Cho phép comment
        /// </summary>
        public bool? CanComment { get; set; }
        /// <summary>
        /// Cho phép xóa
        /// </summary>
        public bool? CanDelete { get; set; }
        /// <summary>
        /// Thẻ SEO
        /// </summary>
        [StringLength(500)]
        public string MetaTitle { get; set; }
        /// <summary>
        /// Thẻ SEO
        /// </summary>
        [StringLength(500)]
        public string MetaDescription { get; set; }
        /// <summary>
        /// Thẻ SEO
        /// </summary>
        [StringLength(500)]
        public string MetaKeywords { get; set; }
        /// <summary>
        /// Tài liệu tham khảo
        /// </summary>
        [StringLength(500)]
        public string DocumentRefer { get; set; }
    }
}