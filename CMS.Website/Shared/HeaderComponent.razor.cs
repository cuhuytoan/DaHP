namespace CMS.Website.Shared
{
    public partial class HeaderComponent : IDisposable
    {
        #region Inject

        [Inject]
        private IMapper Mapper { get; set; }

        [Inject]
        private ILoggerManager Logger { get; set; }

        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        private SignInManager<IdentityUser> SignInManager { get; set; }


        #endregion Inject

        #region Parameter
        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }
        #endregion Parameter

        #region Model
        List<SpUserNotifySearchResult> lstUserNoti { get; set; } = new List<SpUserNotifySearchResult>();
        public int? totalUnread { get; set; }
        private List<ProductCategoryTreeGroup> lstProductCategoryGroups { get; set; } = new();
        private List<SpProductCategoryTreeResult> lstMenuHorizontal { get; set; } = new();
        private ForgotComponent forgotModal { get; set; }
        public LoginComponent loginModal { get; set; }
        private RegisterComponent registerModal { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        private string keyword { get; set; }

        #endregion Model

        #region LifeCycle
        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
            NavigationManager.LocationChanged += HandleLocationChanged;
        }
        protected override async Task OnInitializedAsync()
        {

            BindingSearchControl();
            await InitData();

            globalModel.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            globalModel.OnChange -= StateHasChanged;
            NavigationManager.LocationChanged -= HandleLocationChanged;
            GC.SuppressFinalize(this);
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitData()
        {
            //List Notification
            if (globalModel.user.Identity.IsAuthenticated)
            {
                var result = await Repository.UserNoti.GetAllNoti(null, globalModel.userId, null, 4, 1);
                lstUserNoti = result.Items;
                totalUnread = result.TotalSize;
            }



            var lstProductCategory = await Repository.ProductCategory.ProductCategoryTreeGetLst();
            if (lstProductCategory != null)
            {
                
                foreach (var item in lstProductCategory)
                {
                    //Add menu horizontal
                    if ((bool)item.DisplayMenuHorizontal == true && item.Active == true)
                    {

                        lstMenuHorizontal.Add(item);
                    }
                    if (item.Id < 0 || item.Active == false || item.DisplayMenu == false) continue;
                   

                    ProductCategoryTreeGroup itemGroup = new();
                    if (item.ParentId == null)
                    {
                        if ((bool)item.HaveChild)
                        {
                            itemGroup.ParentItem = item;
                            itemGroup.ChildItem = lstProductCategory.Where(x => x.ParentId == item.Id).ToList();
                            lstProductCategoryGroups.Add(itemGroup);
                        }
                        else
                        {
                            itemGroup.ParentItem = item;
                            itemGroup.ChildItem = new List<SpProductCategoryTreeResult>();
                            lstProductCategoryGroups.Add(itemGroup);
                        }
                    }

                }
            }

        }

        #endregion Init

        #region Event      
        private async Task OnLogout()
        {
            await SignInManager.SignOutAsync();
            StateHasChanged();
            NavigationManager.NavigateTo("/", true);
        }
        private async Task OnEnterPress(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                NavigationManager.NavigateTo($"/tim-kiem?q={keyword}");
            }
        }
        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            BindingSearchControl();
        }
        private void BindingSearchControl()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (uri.ToString().Contains("tim-kiem?q="))
            {
                var queryStrings = QueryHelpers.ParseQuery(uri.Query);

                if (queryStrings.TryGetValue("q", out var _keyword))
                {
                    if (!String.IsNullOrEmpty(_keyword))
                    {
                        this.keyword = _keyword;
                        StateHasChanged();
                    }
                }
            }
        }
        private async Task OnRedirectShopMan()
        {
            if (globalModel.user.Identity.IsAuthenticated)
            {
                if (globalModel.productBrandId is null || globalModel.productBrandId == 0)
                {
                    NavigationManager.NavigateTo("/thong-bao-dang-ky-cua-hang", true);
                }
                else
                {
                    if (globalModel.productBrandStatusId != 4)
                    {
                        NavigationManager.NavigateTo("/cua-hang-cho-duyet", true);
                    }
                    else
                    {
                        NavigationManager.NavigateTo("/Shopman/Dashboard/Index", true);
                    }

                }
            }
            else
            {
                loginModal.Open();
            }
        }
      
        #endregion
    }

}