using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.ModelFilter
{
    public class ProductBrandSearchFilter
    {
        public string Keyword { get; set; }
        public int? ProductBrandCategoryId { get; set; }
        public int? ProductBrandStatusId { get; set; }
        public int? ProductBrandTypeId { get; set; }
        public int? DepartmentManId { get; set; }
        public int? CountryId { get; set; }
        public int? LocationId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public int? ExceptionId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool? Efficiency { get; set; }
        public bool? Active { get; set; }
        public string CreateBy { get; set; }
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }

    }
}
