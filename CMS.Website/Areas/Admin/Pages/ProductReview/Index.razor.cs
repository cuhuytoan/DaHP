namespace CMS.Website.Areas.Admin.Pages.ProductReview
{
    public partial class Index
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

        [Parameter]
        [SupplyParameterFromQuery]
        public string keyword { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? p { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public bool? active { get; set; }

        #endregion Parameter

        #region Model

        private List<SpProductReviewSearchResult> lstProductReview;
        public int currentPage { get; set; }
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 30;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));
        public ProductReviewSearchFilter modelFilter { get; set; }

        public List<CommonDropdownBoolValue> lstProductReviewStatus { get; set; } = new();
        public int? setProductStatusSelected { get; set; }


        public string outMessage = "";

        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }

        protected ConfirmBase DeleteConfirmation { get; set; }
        protected ModalDynamicComponent PropertiesDynamicEdit { get; set; }
        private List<int> listProductReviewSelected { get; set; } = new List<int>();
        private bool _forceRerender;
        private bool isCheck { get; set; }

        #endregion Model

        #region LifeCycle

        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
            NavigationManager.LocationChanged += HandleLocationChanged;
        }

        protected override async Task OnInitializedAsync()
        {
            //var authState = await authenticationStateTask;
            //user = authState.User;
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
            lstProductReviewStatus = new();
            lstProductReviewStatus.Add(new CommonDropdownBoolValue { Id = false, Name = "Chưa duyệt" });
            lstProductReviewStatus.Add(new CommonDropdownBoolValue { Id = true, Name = "Duyệt đăng" });

        }

        protected async Task InitData()
        {
            Logger.LogDebug("Init");
            var modelFilter = new ProductReviewSearchFilter();
            modelFilter.Keyword = keyword;
            modelFilter.Active = active;
            modelFilter.CurrentPage = p ?? 1;
            modelFilter.PageSize = 30;
            modelFilter.FromDate = DateTime.Now.AddYears(-10);
            modelFilter.ToDate = DateTime.Now;

            var result = await Repository.ProductReview.ProductReviewSearchWithPaging(modelFilter);

            lstProductReview = result.Items;
            totalCount = result.TotalSize;

            //Init Selected
            listProductReviewSelected.Clear();
            StateHasChanged();
        }

        #endregion Init

        #region Event

        protected async Task OnPostDemand(int postType)
        {
            if (listProductReviewSelected.Count == 0)
            {
                toastService.ShowToast(ToastLevel.Warning, "Chưa chọn đánh giá thực thi", "Thông báo");
                return;
            }
            else
            {
                try
                {
                    foreach (var item in listProductReviewSelected)
                    {
                        await Repository.ProductReview.ProductReviewUpdateStatus(item, (bool)active, globalModel.userId);
                    }

                    toastService.ShowToast(ToastLevel.Success, "Cập nhật thành công", "Thành công!");

                }
                catch (Exception ex)
                {
                    toastService.ShowToast(ToastLevel.Warning, $"Có lỗi trong quá trình thực thi {ex.ToString()}", "Lỗi!");
                }
                StateHasChanged();
                await InitData();
            }
        }

        protected void DeleteProductReview(int? productReviewId)
        {

            if (productReviewId == null) // Delete Demand
            {
                if (listProductReviewSelected.Count == 0)
                {
                    toastService.ShowToast(ToastLevel.Warning, "Chưa chọn đánh giá để xóa", "Thông báo");
                    return;
                }
            }
            else
            {
                listProductReviewSelected.Clear();
                listProductReviewSelected.Add((int)productReviewId);

            }

            DeleteConfirmation.Show();
        }
        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                try
                {
                    foreach (var item in listProductReviewSelected)
                    {
                        await Repository.ProductReview.ProductReviewDelete(item);
                    }
                    toastService.ShowToast(ToastLevel.Success, "Xóa đánh giá thành công", "Thành công");
                }
                catch
                {
                    toastService.ShowToast(ToastLevel.Warning, "Có lỗi trong quá trình thực thi", "Lỗi!");
                }
                StateHasChanged();
                await InitData();
            }
        }

        protected void OnCheckBoxChange(bool headerChecked, int ProductId, object isChecked)
        {
            if (headerChecked)
            {
                if ((bool)isChecked)
                {
                    listProductReviewSelected.AddRange(lstProductReview.Select(x => x.Id));
                    isCheck = true;
                }
                else
                {
                    isCheck = false;
                    listProductReviewSelected.Clear();
                }
            }
            else
            {
                if ((bool)isChecked)
                {
                    if (!listProductReviewSelected.Contains(ProductId))
                    {
                        listProductReviewSelected.Add(ProductId);
                    }
                }
                else
                {
                    if (listProductReviewSelected.Contains(ProductId))
                    {
                        listProductReviewSelected.Remove(ProductId);
                    }
                }
            }
            StateHasChanged();
        }
        protected async Task OnChangeCommentStatus(int productReviewId, bool active)
        {
            var item = await Repository.ProductReview.FindAsync(productReviewId);
            if (item != null)
            {
                item.Active = active;
                await Repository.ProductReview.ProductReviewUpdateStatus(productReviewId, active, globalModel.userId);
                StateHasChanged();
            }
        }
        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitData();
        }

        #endregion Event
    }
}
