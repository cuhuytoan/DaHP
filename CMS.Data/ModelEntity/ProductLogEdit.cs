﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CMS.Data.ModelEntity
{
    public partial class ProductLogEdit
    {
        [Key]
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ProductTypeId { get; set; }
        [StringLength(200)]
        public string ProductCategoryIds { get; set; }
        public int? ProductManufactureId { get; set; }
        public int? ProductBrandId { get; set; }
        public int? CountryId { get; set; }
        public int? ProductStatusId { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        [StringLength(50)]
        public string BarCode { get; set; }
        [Column("ManufactureSKU")]
        [StringLength(100)]
        public string ManufactureSku { get; set; }
        [Column("SKU")]
        [StringLength(100)]
        public string Sku { get; set; }
        [Column("QRCodePublic")]
        [StringLength(100)]
        public string QrcodePublic { get; set; }
        [StringLength(200)]
        public string SubTitle { get; set; }
        [StringLength(200)]
        public string Image { get; set; }
        [StringLength(200)]
        public string ImageDescription { get; set; }
        [StringLength(200)]
        public string BannerImage { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        [Column(TypeName = "ntext")]
        public string Specification { get; set; }
        [Column(TypeName = "ntext")]
        public string ProductCertificate { get; set; }
        [StringLength(4000)]
        public string LegalInfo { get; set; }
        [Column(TypeName = "money")]
        public decimal? Price { get; set; }
        [Column(TypeName = "money")]
        public decimal? PriceOld { get; set; }
        [Column(TypeName = "money")]
        public decimal? PriceWholesale { get; set; }
        public int? WholesaleMin { get; set; }
        [Column(TypeName = "money")]
        public decimal? Discount { get; set; }
        public int? DiscountRate { get; set; }
        public bool? IsSecondHand { get; set; }
        public bool? IsAuthor { get; set; }
        public bool? IsBestSale { get; set; }
        public bool? IsSaleOff { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsComming { get; set; }
        public bool? IsOutStock { get; set; }
        public bool? IsDiscontinue { get; set; }
        public int? AmountDefault { get; set; }
        public int? UnitId { get; set; }
        [StringLength(500)]
        public string ExpiryDisplay { get; set; }
        public int? ExpiryByDay { get; set; }
        [StringLength(500)]
        public string WarrantyDisplay { get; set; }
        public int? WarrantyByMonth { get; set; }
        public int? Rate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        public bool? Active { get; set; }
        public int? Counter { get; set; }
        public int? LikeCount { get; set; }
        public int? SellCount { get; set; }
        [StringLength(450)]
        public string CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [StringLength(450)]
        public string LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditDate { get; set; }
        public int? Checked { get; set; }
        [StringLength(450)]
        public string CheckBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CheckDate { get; set; }
        public int? Approved { get; set; }
        [StringLength(450)]
        public string ApproveBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApproveDate { get; set; }
        [Column("URL")]
        [StringLength(1000)]
        public string Url { get; set; }
        public string Tags { get; set; }
        public bool? CanCopy { get; set; }
        public bool? CanComment { get; set; }
        public bool? CanDelete { get; set; }
        [StringLength(500)]
        public string MetaTitle { get; set; }
        [StringLength(500)]
        public string MetaDescription { get; set; }
        [StringLength(500)]
        public string MetaKeywords { get; set; }
        [StringLength(500)]
        public string DocumentRefer { get; set; }
    }
}