﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.ModelEntity
{
    public partial class SpProductBrandSearchResult
    {
        public int NoItem { get; set; }
        public int Id { get; set; }
        public int? ProductBrandCategoryId { get; set; }
        public int? ProductBrandTypeId { get; set; }
        public int? DepartmentManId { get; set; }
        public int? ProductBrandModelManagement_ID { get; set; }
        public int? ProductBrandStatusId { get; set; }
        public int? LocationId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TradingName { get; set; }
        public string BrandName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Zalo { get; set; }
        public string Hotline { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }
        public bool? Active { get; set; }
        public bool? HasQRCode { get; set; }
        public int? ViewCount { get; set; }
        public int? SellCount { get; set; }
        public string AccountUserName { get; set; }
        public string AccountEmail { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string LastEditBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public int? Checked { get; set; }
        public string CheckBy { get; set; }
        public DateTime? CheckDate { get; set; }
        public int? Approved { get; set; }
        public string ApproveBy { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string CreateByName { get; set; }
        public string StatusName { get; set; }
        public string LocationName { get; set; }
        public int ProductCount { get; set; }
        public int? ProductBrandLevelId { get; set; }
        public string ProductBrandLevelName { get; set; }
    }
}
