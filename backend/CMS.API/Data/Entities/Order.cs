using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.API.Data.Entities;

[Table("productorder")]
public class ProductOrder
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("ordercode")]
    public string? OrderCode { get; set; }

    [Column("userid")]
    public string? UserId { get; set; }

    [Column("productbrandid")]
    public long? ProductBrandId { get; set; }

    [Column("productorderstatusid")]
    public long? ProductOrderStatusId { get; set; }

    [Column("productorderpaymentmethodid")]
    public long? ProductOrderPaymentMethodId { get; set; }

    [Column("productorderpaymentstatusid")]
    public long? ProductOrderPaymentStatusId { get; set; }

    [Column("customername")]
    public string? CustomerName { get; set; }

    [Column("customeremail")]
    public string? CustomerEmail { get; set; }

    [Column("customerphone")]
    public string? CustomerPhone { get; set; }

    [Column("customeraddress")]
    public string? CustomerAddress { get; set; }

    [Column("locationid")]
    public long? LocationId { get; set; }

    [Column("districtid")]
    public long? DistrictId { get; set; }

    [Column("wardid")]
    public long? WardId { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [Column("subtotal", TypeName = "numeric(19,4)")]
    public decimal? SubTotal { get; set; }

    [Column("shippingfee", TypeName = "numeric(19,4)")]
    public decimal? ShippingFee { get; set; }

    [Column("discount", TypeName = "numeric(19,4)")]
    public decimal? Discount { get; set; }

    [Column("tax", TypeName = "numeric(19,4)")]
    public decimal? Tax { get; set; }

    [Column("total", TypeName = "numeric(19,4)")]
    public decimal? Total { get; set; }

    [Column("couponcode")]
    public string? CouponCode { get; set; }

    [Column("transactionid")]
    public string? TransactionId { get; set; }

    [Column("paymentdate")]
    public DateTime? PaymentDate { get; set; }

    [Column("shippingdate")]
    public DateTime? ShippingDate { get; set; }

    [Column("deliverydate")]
    public DateTime? DeliveryDate { get; set; }

    [Column("canceldate")]
    public DateTime? CancelDate { get; set; }

    [Column("cancelreason")]
    public string? CancelReason { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [Column("lasteditby")]
    public string? LastEditBy { get; set; }

    [Column("lasteditdate")]
    public DateTime? LastEditDate { get; set; }

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    [ForeignKey("ProductBrandId")]
    public virtual ProductBrand? ProductBrand { get; set; }

    [ForeignKey("ProductOrderStatusId")]
    public virtual ProductOrderStatus? ProductOrderStatus { get; set; }

    [ForeignKey("ProductOrderPaymentMethodId")]
    public virtual ProductOrderPaymentMethod? ProductOrderPaymentMethod { get; set; }

    [ForeignKey("ProductOrderPaymentStatusId")]
    public virtual ProductOrderPaymentStatus? ProductOrderPaymentStatus { get; set; }

    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }

    [ForeignKey("DistrictId")]
    public virtual District? District { get; set; }

    [ForeignKey("WardId")]
    public virtual Ward? Ward { get; set; }

    public virtual ICollection<ProductOrderDetail> OrderDetails { get; set; } = new List<ProductOrderDetail>();
}

[Table("productorderdetail")]
public class ProductOrderDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("productorderid")]
    public long? ProductOrderId { get; set; }

    [Column("productid")]
    public long? ProductId { get; set; }

    [Column("productname")]
    public string? ProductName { get; set; }

    [Column("productimage")]
    public string? ProductImage { get; set; }

    [Column("productcode")]
    public string? ProductCode { get; set; }

    [Column("price", TypeName = "numeric(19,4)")]
    public decimal? Price { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("discount", TypeName = "numeric(19,4)")]
    public decimal? Discount { get; set; }

    [Column("total", TypeName = "numeric(19,4)")]
    public decimal? Total { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [Column("createby")]
    public string? CreateBy { get; set; }

    [Column("createdate")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("ProductOrderId")]
    public virtual ProductOrder? ProductOrder { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}

[Table("productorderstatus")]
public class ProductOrderStatus
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("color")]
    public string? Color { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }
}

[Table("productorderpaymentmethod")]
public class ProductOrderPaymentMethod
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("sort")]
    public int? Sort { get; set; }

    [Column("active")]
    public bool? Active { get; set; }
}

[Table("productorderpaymentstatus")]
public class ProductOrderPaymentStatus
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("color")]
    public string? Color { get; set; }
}