namespace CMS.Website.Areas.Admin.Pages.ProductBrand
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
        [CascadingParameter]
        protected HubConnection hubConnection { get; set; }
        [Parameter]
        [SupplyParameterFromQuery]
        public string keyword { get; set; }


        [Parameter]
        [SupplyParameterFromQuery]
        public int? productBrandStatusId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? p { get; set; }
        #endregion Parameter

        #region Model

        private List<SpProductBrandSearchResult> lstProductBrand;

        public int totalCount { get; set; }
        public int pageSize { get; set; } = 30;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));
        public ProductBrandSearchFilter modelFilter { get; set; }
        private List<ProductBrandStatus> lstProductBrandStatus { get; set; }

        public int? setProductBrandStatusSelected { get; set; }

        public string outMessage = "";
        protected ConfirmBase DeleteConfirmation { get; set; }
        private List<int> listProductBrandSelected { get; set; } = new List<int>();
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
            //Binding Status
            var lstStatus = await Repository.ProductBrand.ProductBrandStatusGetLst();
            if (lstStatus != null)
            {
                lstProductBrandStatus = lstStatus.Select(x => new ProductBrandStatus { Id = x.Id, Name = x.Name }).ToList();
            }

        }

        protected async Task InitData()
        {
            ProductBrandSearchFilter modelFilter = new()
            {
                Keyword = keyword,
                CurrentPage = p ?? 1,
                PageSize = 30,
                ProductBrandStatusId = productBrandStatusId,
                FromDate = DateTime.Now.AddYears(-10),
                ToDate = DateTime.Now.AddYears(10)

            };

            var result = await Repository.ProductBrand.ProductBrandSearchWithPaging(modelFilter);

            lstProductBrand = result.Items;
            totalCount = result.TotalSize;

            //Init Selected
            listProductBrandSelected.Clear();
            StateHasChanged();
        }

        #endregion Init

        #region Event

        protected async Task OnPostDemand(int postType)
        {
            if (listProductBrandSelected.Count == 0)
            {
                toastService.ShowToast(ToastLevel.Warning, "Chưa chọn cửa hàng thực thi", "Thông báo");
                return;
            }
            else
            {
                try
                {
                    foreach (var item in listProductBrandSelected)
                    {
                        await Repository.ProductBrand.ProductBrandUpdateStatusType(globalModel.userId, item, postType);
                        if (postType == 4) //Sent noti
                        {
                            //Noti for user
                            var itemProductBrand = lstProductBrand.FirstOrDefault(x => x.Id == item);
                            if (itemProductBrand != null)
                            {
                                await hubConnection.SendAsync("SendNotification",
                                    itemProductBrand.CreateBy,
                                    $"Cửa hàng {itemProductBrand.Name} vừa được phê duyệt",
                                    $"Cửa hàng {itemProductBrand.Name} vừa được quản trị viên phê duyệt. Bạn có thể đăng sản phẩm lên cửa hàng.",
                                    $"/Shopman/Dashboard/Index", itemProductBrand.Image);
                            }


                        }
                    }

                    toastService.ShowToast(ToastLevel.Success, "Cập nhật thành công", "Thành công!");

                }
                catch (Exception ex)
                {
                    toastService.ShowToast(ToastLevel.Warning, $"Có lỗi trong quá trình thực thi {ex}", "Lỗi!");
                }

                StateHasChanged();
                await InitData();
            }
        }
        protected async Task OnPostActive(int item)
        {

            try
            {
                await Repository.ProductBrand.ProductBrandUpdateStatusType(globalModel.userId, item, 4);
                //Noti for user
                var itemProductBrand = lstProductBrand.FirstOrDefault(x => x.Id == item);
                if (itemProductBrand != null)
                {
                    await hubConnection.SendAsync("SendNotification",
                        itemProductBrand.CreateBy,
                        $"Cửa hàng {itemProductBrand.Name} vừa được phê duyệt",
                        $"Cửa hàng {itemProductBrand.Name} vừa được quản trị viên phê duyệt. Bạn có thể đăng sản phẩm lên cửa hàng.",
                        $"/Shopman/Dashboard/Index",
                        itemProductBrand.Image);
                }

                toastService.ShowToast(ToastLevel.Success, "Cập nhật thành công", "Thành công!");

            }
            catch (Exception ex)
            {
                toastService.ShowToast(ToastLevel.Warning, $"Có lỗi trong quá trình thực thi {ex}", "Lỗi!");
            }

            StateHasChanged();
            await InitData();

        }
        protected void OnCheckBoxChange(bool headerChecked, int ProductBrandId, object isChecked)
        {
            if (headerChecked)
            {
                if ((bool)isChecked)
                {
                    listProductBrandSelected.AddRange(lstProductBrand.Select(x => x.Id));
                    isCheck = true;
                }
                else
                {
                    isCheck = false;
                    listProductBrandSelected.Clear();
                }
            }
            else
            {
                if ((bool)isChecked)
                {
                    if (!listProductBrandSelected.Contains(ProductBrandId))
                    {
                        listProductBrandSelected.Add(ProductBrandId);
                    }
                }
                else
                {
                    if (listProductBrandSelected.Contains(ProductBrandId))
                    {
                        listProductBrandSelected.Remove(ProductBrandId);
                    }
                }
            }
            StateHasChanged();
        }

        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitData();
        }

        #endregion Event
    }
}
