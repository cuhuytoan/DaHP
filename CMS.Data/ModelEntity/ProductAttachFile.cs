﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CMS.Data.ModelEntity
{
    public partial class ProductAttachFile
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int? ProductId { get; set; }
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
}