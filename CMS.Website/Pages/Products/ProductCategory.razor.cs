namespace CMS.Website.Pages.Products
{
    public partial class ProductCategory : IDisposable
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
        public string url { get; set; }
        [Parameter]
        [SupplyParameterFromQuery]
        public int? p { get; set; } = 1;

        [SupplyParameterFromQuery]
        public int? fromPrice { get; set; }

        [SupplyParameterFromQuery]
        public int? toPrice { get; set; }

        [SupplyParameterFromQuery]
        public string orderBy { get; set; } = "CREATEDATE DESC";

        [SupplyParameterFromQuery]
        public int? locationId { get; set; }
        //[Parameter]        
        public int? productCategoryId { get; set; }
        #endregion Parameter

        #region Model

        private List<SpProductSearchResult> lstProduct { get; set; } = new();
        public int currentPage { get; set; }
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 30;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));
        public ProductSearchFilter modelFilter { get; set; } = new();

        public bool viewMoreExpanded { get; set; } = true;

        public Dictionary<string, string> lstSort { get; set; } = new();

        public List<CMS.Data.ModelEntity.ProductCategory> lstProductCategoryChild { get; set; } = new();

        public List<CMS.Data.ModelEntity.Location> lstLocation { get; set; } = new();

        private CMS.Data.ModelEntity.Setting setting { get; set; } = new();
        private CMS.Data.ModelEntity.ProductCategory productCategory { get; set; } = new();

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
            var item = await Repository.ProductCategory.FirstOrDefaultAsync(p => p.Url == url);
            if (item == null)
            {
                NavigationManager.NavigateTo("/Error404");
                return;
            }
            else
            {
                productCategory = item;
                productCategoryId = item.Id;
            }
            await InitControl();
            await InitData();
        }


        #endregion LifeCycle

        #region Init

        protected async Task InitControl()
        {

            lstProductCategoryChild = await Repository.ProductCategory.GetProductCategoryChildByParentId((int)productCategoryId);

            lstLocation = await Repository.MasterData.LocationGetLstByCountryId(1);

            lstSort = new();
            lstSort.Add("CREATEDATE DESC", "Mới nhất");
            lstSort.Add("PRICE DESC", "Giá thấp nhất");
            lstSort.Add("PRICE ASC", "Giá cao nhất");


            StateHasChanged();
        }

        protected async Task InitData()
        {
            setting = await Repository.Setting.GetSetting();
            ProductSearchFilter modelFilter = new()
            {
                ProductCategoryId = productCategoryId,
                LocationId = locationId,
                OrderBy = orderBy,
                FromPrice = fromPrice,
                ToPrice = toPrice,
                CurrentPage = p ?? 1,
                PageSize = 20,
                ProductStatusId = 4
            };

            var result = await Repository.Product.ProductSearchWithPaging(modelFilter);

            lstProduct = result.Items;
            totalCount = result.TotalSize;

            StateHasChanged();
        }
        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }
        #endregion Init

        #region Event
        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var item = await Repository.ProductCategory.FirstOrDefaultAsync(p => p.Url == url);
            if (item == null)
            {
                NavigationManager.NavigateTo("/Error404");
                return;
            }
            else
            {
                productCategoryId = item.Id;
            }
            await InitControl();
            await InitData();
        }

        protected async Task ColapsedViewMore()
        {
            viewMoreExpanded = !viewMoreExpanded;
        }

        protected async Task OnSlectedLocation(int? Id)
        {
            locationId = Id;
        }

        protected async Task OnChangeOrder(ChangeEventArgs e)
        {
            var urlRouting = $"/chung-loai/{url}?";
            if (locationId != null)
            {
                urlRouting = urlRouting + $"locationId={locationId}&";
            }
            if (fromPrice != null)
            {
                urlRouting = urlRouting + $"fromPrice={fromPrice}&";
            }
            if (toPrice != null)
            {
                urlRouting = urlRouting + $"toPrice={toPrice}&";
            }
            if (!String.IsNullOrEmpty(orderBy))
            {
                urlRouting = urlRouting + $"orderBy={e.Value}&";
            }
            if (p != null)
            {
                urlRouting = urlRouting + $"p={p}";
            }
            NavigationManager.NavigateTo(urlRouting);
        }


        #endregion
    }
}
