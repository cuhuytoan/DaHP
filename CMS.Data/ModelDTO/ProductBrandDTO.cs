using CMS.Data.ModelEntity;
using CMS.Data.ValidationCustomize;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.ModelDTO
{
    public class ProductBrandDTO : ProductBrand
    {
        public string LocationName { get; set; }
        public string ProductBrandLevelName { get; set; }
        public int? ProductCount { get; set; }
    }
    public class ProductBrandEditDTO
    {
        public int Id { get; set; }
        public int? ProductBrandCategoryId { get; set; }
        public int? ProductBrandTypeId { get; set; }
        public int? DepartmentManId { get; set; } 
        public int? ProductBrandModelManagementId { get; set; }
        public int? ProductBrandStatusId { get; set; }
        public int? CountryId { get; set; }
        [Required(ErrorMessage ="Chọn tỉnh , thành phố")]
        public int? LocationId { get; set; }
        [Required(ErrorMessage = "Chọn quận huyện")]
        public int? DistrictId { get; set; }
        [Required(ErrorMessage = "Chọn phường xã")]
        public int? WardId { get; set; }
        [StringLength(1000)]
        public string Code { get; set; }
        /// <summary>
        /// Mã QRCode sản phẩm
        /// </summary>
        [Column("QRCodePublic")]
        [StringLength(100)]
        public string QrcodePublic { get; set; }
        [Required(ErrorMessage ="Nhập tên cửa hàng")]
        public string Name { get; set; }
        [StringLength(1000)]
        public string TradingName { get; set; }
        [StringLength(200)]
        public string BrandName { get; set; }
        [StringLength(50)]
        public string TaxCode { get; set; }
        [StringLength(50)]
        public string RegistrationNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IssuedDate { get; set; }
        [Required(ErrorMessage ="Nhập lĩnh vực kinh doanh")]
        public string BusinessArea { get; set; }
        [Required(ErrorMessage = "Nhập số nhà, đường")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Nhập số điện thoại")]
        [PhoneNumber(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Telephone { get; set; }
        [StringLength(100)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(200)]
        public string Facebook { get; set; }
        [StringLength(200)]
        public string Zalo { get; set; }
        [StringLength(200)]
        public string Hotline { get; set; }
        [StringLength(200)]
        public string Skype { get; set; }
        [Column("PRInfo")]
        [StringLength(4000)]
        public string Prinfo { get; set; }
        [StringLength(4000)]
        public string Agency { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(500)]
        public string Image { get; set; }
        [StringLength(200)]
        public string PersonSurName { get; set; }
        [StringLength(200)]
        public string PersonName { get; set; }
        [StringLength(500)]
        public string PersonAddress { get; set; }
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [PhoneNumber(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PersonMobile { get; set; }
        [Required(ErrorMessage = "Nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string PersonEmail { get; set; }
        [StringLength(200)]
        public string PersonPosition { get; set; }
        public int? Sort { get; set; }
        [Column("URL")]
        [StringLength(200)]
        public string Url { get; set; }
        public bool? Active { get; set; }
        [Column("HasQRCode")]
        public bool? HasQrcode { get; set; }
        public int? ViewCount { get; set; }
        public int? ViewPageCount { get; set; }
        public int? FollowCount { get; set; }
        public int? SellCount { get; set; }
        [StringLength(50)]
        public string AccountUserName { get; set; }
        [StringLength(200)]
        public string AccountEmail { get; set; }
        [StringLength(200)]
        public string DirectorName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DirectorBirthday { get; set; }
        [StringLength(500)]
        public string DirectorAddress { get; set; }
        [StringLength(200)]
        public string DirectorMobile { get; set; }
        [StringLength(200)]
        public string DirectorEmail { get; set; }
        [StringLength(200)]
        public string DirectorPosition { get; set; }
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
        public string PersonZalo { get; set; }
        public string GoogleMapCode { get; set; }
        public string BankAcc { get; set; }
        public int? BankId { get; set; }
        public string LegalDocument { get; set; }
        public int? ProductBrandLevelId { get; set; }
  
    }
    public class ProductBrandContactDTO
    {
        [Required(ErrorMessage ="Nhập họ tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [PhoneNumber(ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string Phone { get; set; }
        public int? LocationId { get; set; }
        public int? DistrictId { get; set; }
        [Required(ErrorMessage = "Nhập nội dung lời nhắn")]
        public string Content { get; set; }
    }
    public class ProductBrandDashBoardTotal
    {
        public int ProductBrandFollowCount { get; set; }
        public int ProductBrandViewPageCount { get; set; }
        public int ProductBrandProductCount { get; set; }
        public int ProductBrandReview { get; set; }
    }
    public class AdminDashBoardTotal
    {
        public int ViewCount { get; set; }
        public int ProductBrandCount { get; set; }
        public int ProductCount { get; set; }
        public int ProductReviewCount { get; set; }
    }
    public class ProductBrandAttachFileDTO
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int? ProductBrandId { get; set; }
        [StringLength(500)]
        public string AttachFileName { get; set; }
        [StringLength(50)]
        public string FileType { get; set; }
        public double? FileSize { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [StringLength(200)]
        public string CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastEditDate { get; set; }
        [StringLength(200)]
        public string LastEditBy { get; set; }
    }
    public class FromToDateDTO
    {
        [Required(ErrorMessage = "Nhập từ ngày")]
        [CompareDate(nameof(FromDate), "Less", ErrorMessage = "Từ ngày đến ngày không hợp lệ")]
        public DateTime FromDate { get; set; } = DateTime.Now.AddMonths(-1);
        [Required(ErrorMessage = "Nhập đến ngày")]
        [CompareDate(nameof(FromDate), "Less", ErrorMessage = "Từ ngày đến ngày không hợp lệ")]
        public DateTime ToDate { get; set; } = DateTime.Now;
    }
}
