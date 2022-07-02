namespace CMS.Website.Pages.Article
{
    public partial class ArticleDetails : IDisposable
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

        private ArticleDTO articleDetail { get; set; } = new();

        private List<SpArticleSearchResult> lstArticleRelated { get; set; } = new();

        private List<Advertising> lstAdvertisingRight { get; set; } = new();


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
            var articleDetailItem = await Repository.Article.ArticleGetByUrl(url);
            if (articleDetailItem == null)
            {
                NavigationManager.NavigateTo("/Error404");
                return;
            }
            articleDetail = Mapper.Map<ArticleDTO>(articleDetailItem);

            //Article Related
            lstArticleRelated = await Repository.Article.ArticleRelationGetlstByArticleId(articleDetailItem.Id);

            //Quảng cáo khối bên phải
            lstAdvertisingRight = await Repository.Advertising.AdvertisingGetLstByBlockId(8);
            // Increase Counter
            await Repository.Article.ArticleIncreaseCount(articleDetailItem.Id);
            if (articleDetailItem.ProductBrandId is not null)
            {
                await Repository.ProductBrand.ProductBrandIncreaseViewPageCount((int)articleDetailItem.ProductBrandId);
            }
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
