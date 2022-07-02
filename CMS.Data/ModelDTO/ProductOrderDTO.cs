using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.ModelDTO
{
    public class ProductOrderDTO
    {
        [Key]
        public int Id { get; set; }
        public int? ProductOrderStatusId { get; set; }
        public int? ProductOrderPaymentStatusId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public int? ProductOrderPaymentMethodId { get; set; }
        [StringLength(200)]
        public string BillNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên người nhận hàng")]
        [StringLength(200)]
        public string CustomerName { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(100)]
        public string Voucher { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [Column(TypeName = "money")]
        public decimal? Sum { get; set; }
        [Column(TypeName = "money")]
        public decimal? DisCount { get; set; }
        [Column(TypeName = "money")]
        public decimal? Total { get; set; }
        [StringLength(450)]
        public string CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [StringLength(450)]
        public string LastEditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditDate { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn tỉnh thành")]
        public int? LocationId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn quận huyện")]
        public int? DistrictId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn phường xã")]
        public int? WardId { get; set; }
        public string ProductOrderCode { get; set; }
    }
    public class ProductOrderGroup
    {
        public int? ProductOrderId { get; set; }
        public int? ProductBrandId { get; set; }
        
        public string ProductBrandName { get; set; }
        public string ProductBrandImage { get; set; }
        public string ProductBrandUrl { get; set; }
        public int? ProductBrandLevelId { get; set; }
        public string ProductBrandLevelName { get; set; }
        public int? ProductOrderStatusId { get; set; }
        public int? ProductOrderPaymentStatusId { get; set; }
        public int? ProductOrderPaymentMethodId { get; set; }
        public string BillNumber { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Voucher { get; set; }
        public string Description { get; set; }
        public decimal? Sum { get; set; }
        public decimal? DisCount { get; set; }
        public decimal? Total { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public int? LocationId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string ProductOrderPaymentMethodName { get; set; }
        public string ProductOrderPaymentStatusName { get; set; }
        public string ProductOrderStatusName { get; set; }
        public List<ProductOrderProduct> ListProductOrderProduct { get; set; } = new();
    }
    public class ProductOrderProductBrand
    {
        public int? ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
        public string ProductBrandImage { get; set; }
        public string ProductBrandUrl { get; set; }
    }
    public class ProductOrderProduct
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? ProductPriceOld { get; set; }
        public int? ProductRate { get; set; }
        public string ProductUrl { get; set; }
        public int? ProductOrderDetailQty { get; set; }
        public decimal? ProductOrderDetailPrice { get; set; }
        public decimal? ProductOrderDetailSum { get; set; }
        public decimal? ProductOrderDetailDiscount { get; set; }
        public decimal? ProductOrderDetailTotal { get; set; }
    }
}
