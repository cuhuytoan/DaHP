namespace CMS.Data.ModelFilter
{
    public class ProductOrderSearchFilter
    {
        public int? ProductOrderId { get; set; }
        public string CreateBy { get; set; }
        public int? ProductOrderStatusId { get; set; }
        public string OrderBy { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}
