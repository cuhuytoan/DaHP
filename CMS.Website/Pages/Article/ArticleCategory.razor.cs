namespace CMS.Website.Pages.Article
{
    public partial class ArticleCategory
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

        #endregion Parameter

        #region Model

        private SpArticleGetTopByCategoryIdResult articleHightLight { get; set; } = new();

        private List<SpArticleGetNewByCategoryIdResult> lstArticleNew { get; set; } = new();

        private List<Advertising> lstAdvertisingRight { get; set; } = new();
        public int currentPage { get; set; }
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 9;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));

        public int articleCategoryId { get; set; }


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


        #endregion LifeCycle

        #region Init



        protected async Task InitData()
        {
            var item = await Repository.ArticleCategory.FirstOrDefaultAsync(p => p.Url == url);
            if (item == null)
            {
                NavigationManager.NavigateTo("/Error404");
                return;
            }
            var itemTop = await Repository.Article.ArticleGetTopByCategoryId(articleCategoryId);
            if (itemTop != null)
            {
                articleHightLight = itemTop.FirstOrDefault();
            }
            var lstItemNew = await Repository.Article.ArticleGetNewByCategoryId(articleCategoryId, 9);
            if (lstItemNew != null)
            {
                lstArticleNew = lstItemNew;
            }
            //Quảng cáo khối bên phải
            lstAdvertisingRight = await Repository.Advertising.AdvertisingGetLstByBlockId(8);
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

            await InitData();
        }

        #endregion
    }
}
