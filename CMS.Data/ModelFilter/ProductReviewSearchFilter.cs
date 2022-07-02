using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.ModelFilter
{
    public class ProductReviewSearchFilter
    {
        public string Keyword { get; set; }
        public int? ProductReviewTypeId { get; set; }
        public int? LocationId { get; set; }
        public int? DepartmentManId { get; set; }
        public int? ProductBrandId { get; set; }
        public int? ProductId { get; set; }
        public bool? Active { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? CreateBy { get; set; }
        public int? PageSize = 100;
        public int? CurrentPage = 1;
    }
}
