﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.ModelEntity
{
    public partial class SpLogVisitSearchResult
    {
        public int NoItem { get; set; }
        public string Id { get; set; }
        public string ObjectType { get; set; }
        public string ObjectTypeName { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public int? ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
        public string URL { get; set; }
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string AgentId { get; set; }
    }
}