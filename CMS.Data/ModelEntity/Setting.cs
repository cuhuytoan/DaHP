// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CMS.Data.ModelEntity
{
    public partial class Setting
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Domain { get; set; }
        [StringLength(500)]
        public string WebsiteName { get; set; }
        [StringLength(500)]
        public string AdminName { get; set; }
        [StringLength(500)]
        public string EmailSupport { get; set; }
        [StringLength(500)]
        public string EmailOrder { get; set; }
        [Column("EmailSenderSMTP")]
        [StringLength(500)]
        public string EmailSenderSmtp { get; set; }
        [StringLength(500)]
        public string EmailSenderPort { get; set; }
        [Column("EmailSenderSSL")]
        public bool? EmailSenderSsl { get; set; }
        [StringLength(500)]
        public string EmailSender { get; set; }
        [StringLength(500)]
        public string EmailSenderPassword { get; set; }
        [StringLength(500)]
        public string Telephone { get; set; }
        public bool AppStatus { get; set; }
        public int? Counter { get; set; }
        [Column("DefaultLanguage_ID")]
        public int DefaultLanguageId { get; set; }
        [Column("DefaultSkin_ID")]
        public int DefaultSkinId { get; set; }
        [StringLength(500)]
        public string MetaDescriptionDefault { get; set; }
        [StringLength(500)]
        public string MetaKeywordsDefault { get; set; }
        [StringLength(500)]
        public string MetaTitleDefault { get; set; }
        [StringLength(1000)]
        public string GoogleAnalyticsCode { get; set; }
        [StringLength(1000)]
        public string OtherCode { get; set; }
        [Column("FacebookPageID")]
        [StringLength(50)]
        public string FacebookPageId { get; set; }
        [Column("FacebookAppID")]
        [StringLength(50)]
        public string FacebookAppId { get; set; }
        [StringLength(50)]
        public string FacebookAdmin { get; set; }
        [Column("TwitterID")]
        [StringLength(50)]
        public string TwitterId { get; set; }
        [Column("VBeeApp_ID")]
        [StringLength(200)]
        public string VbeeAppId { get; set; }
        [Column("VBeeUser_ID")]
        [StringLength(50)]
        public string VbeeUserId { get; set; }
        public string vnp_Returnurl { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_HashSecret { get; set; }
        public string vnp_Url { get; set; }
    }
}