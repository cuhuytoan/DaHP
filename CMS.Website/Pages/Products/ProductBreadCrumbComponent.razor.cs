namespace CMS.Website.Pages.Products
{
    public partial class ProductBreadCrumbComponent : IDisposable
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
        public string url { get; set; }
        [Parameter]
        public string breadCrumbType { get; set; }

        #endregion Parameter

        #region Model

        private List<CMS.Data.ModelEntity.SpProductBreadcrumbResult> lstProductBreadcrumb { get; set; } = new();



        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }


        #endregion Model

        #region LifeCycle

        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
            NavigationManager.LocationChanged += HandleLocationChanged;
        }
        protected override async Task OnInitializedAsync()
        {
            await InitData();
        }
        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitData()
        {
            if (breadCrumbType == "productCategory")
            {
                var item = await Repository.ProductCategory.FirstOrDefaultAsync(p => p.Url == url);
                if (item == null)
                {
                    NavigationManager.NavigateTo("/Error404");
                    return;
                }
                lstProductBreadcrumb = await Repository.Product.ProductBreadCrumbGetlst(item.Id);
            }
            else if (breadCrumbType == "product")
            {
                var item = await Repository.Product.FirstOrDefaultAsync(p => p.Url == url);
                if (item == null)
                {
                    NavigationManager.NavigateTo("/Error404");
                    return;
                }
                else
                {
                    var itemCategory = await Repository.ProductCategory.ProductCategoryGetByProductId(item.Id);
                    if (itemCategory == null)
                    {
                        NavigationManager.NavigateTo("/Error404");
                        return;
                    }
                    lstProductBreadcrumb = await Repository.Product.ProductBreadCrumbGetlst(itemCategory.Id);
                }

            }
            StateHasChanged();
        }

        #endregion Init

        #region Event
        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var item = await Repository.ProductCategory.FirstOrDefaultAsync(p => p.Url == url);

            await InitData();
        }

        #endregion

    }
}
