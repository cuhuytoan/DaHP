namespace CMS.Website.Areas.Admin.Pages.DashBoard
{
    public partial class Index : IDisposable
    {
        #region Inject

        [Inject]
        private IMapper Mapper { get; set; }

        [Inject]
        private ILoggerManager Logger { get; set; }

        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }

        #endregion Inject

        #region Parameter
        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? p { get; set; }

        #endregion Parameter

        #region Model
        private List<SpProductSearchResult> lstProduct;
        public int currentPage { get; set; }
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 30;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));
        private AdminDashBoardTotal adminDashBoard { get; set; } = new();
        protected ConfirmBase DeleteConfirmation { get; set; }
        private List<int> listProductSelected { get; set; } = new();

        private FromToDateDTO dateQry { get; set; } = new();
        List<KeyValuePair<int, string>> dateOptions { get; set; } = new();
        private int dateOptionSelected { get; set; } = 1;
        #endregion Model

        #region LifeCycle

        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
            NavigationManager.LocationChanged += HandleLocationChanged;
        }

        protected override async Task OnInitializedAsync()
        {
            await InitControl();
            await InitData();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
            GC.SuppressFinalize(this);
        }

        #endregion LifeCycle

        #region Init
        protected async Task InitControl()
        {
            dateOptions = new();
            dateOptions.Add(new KeyValuePair<int, string>(1, "Tùy chọn"));
            dateOptions.Add(new KeyValuePair<int, string>(2, "Hôm nay"));
            dateOptions.Add(new KeyValuePair<int, string>(3, "Tháng này"));
            dateOptions.Add(new KeyValuePair<int, string>(4, "Tháng trước"));
            dateOptions.Add(new KeyValuePair<int, string>(5, "Tất cả"));

        }
        protected async Task InitData()
        {

            ProductSearchFilter modelFilter = new()
            {
                CurrentPage = p ?? 1,
                PageSize = 30,
                FromDate = dateQry.FromDate,
                ToDate = dateQry.ToDate
            };
            var result = await Repository.Product.ProductSearchWithPaging(modelFilter);

            lstProduct = result.Items;
            totalCount = result.TotalSize;




            adminDashBoard.ViewCount = await Repository.LogVisit.LogVisitCountByDateQry(dateQry.FromDate,dateQry.ToDate);
            adminDashBoard.ProductBrandCount = await Repository.ProductBrand.ProductBrandCountByDateQry(dateQry.FromDate, dateQry.ToDate);
            adminDashBoard.ProductCount = await Repository.Product.ProductCountByDateQry(dateQry.FromDate, dateQry.ToDate);
            adminDashBoard.ProductReviewCount = await Repository.ProductReview.ProductReviewCountByDateQry(dateQry.FromDate, dateQry.ToDate);


            StateHasChanged();
        }
        #endregion

        #region Event
        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitData();
        }
        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                try
                {
                    foreach (var item in listProductSelected)
                    {
                        await Repository.Product.ProductDelete(item);
                    }
                    toastService.ShowToast(ToastLevel.Success, "Xóa bài viết thành công", "Thành công");
                }
                catch
                {
                    toastService.ShowToast(ToastLevel.Warning, "Có lỗi trong quá trình thực thi", "Lỗi!");
                }
                StateHasChanged();
                await InitData();
            }
        }
        protected void DeleteProduct(int? productId)
        {
            listProductSelected.Clear();
            listProductSelected.Add((int)productId);
            DeleteConfirmation.Show();
        }
        private void OnChangeDateOptionSelected()
        {
            if (dateOptionSelected == 2)
            {
                dateQry.FromDate = DateTime.Now;
                dateQry.ToDate = DateTime.Now;
            }
            if (dateOptionSelected == 3)
            {
                dateQry.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dateQry.ToDate = DateTime.Now;
            }
            if (dateOptionSelected == 4)
            {
                DateTime firstDayLastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
                dateQry.FromDate = firstDayLastMonth;
                dateQry.ToDate = firstDayLastMonth.AddMonths(1).AddDays(-1);
            }
            if (dateOptionSelected == 5)
            {
                dateQry.FromDate = new DateTime(2021, 1, 1);
                dateQry.ToDate = DateTime.Now;
            }
            StateHasChanged();
        }
        #endregion
    }
}
