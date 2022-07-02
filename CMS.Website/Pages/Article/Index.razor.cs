namespace CMS.Website.Pages.Article
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


        #endregion Parameter

        #region Model

        private SpArticleSearchResult articleHightLight { get; set; } = new();
        private List<SpArticleSearchResult> lstArticleTop3 { get; set; } = new();

        private List<SpArticleSearchResult> lstArticleVertical { get; set; } = new();


        private List<Advertising> lstAdvertisingRight { get; set; } = new();




        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }


        #endregion Model

        #region LifeCycle
        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
            // NavigationManager.LocationChanged += HandleLocationChanged;
        }
        protected override async Task OnInitializedAsync()
        {
            await InitData();
            StateHasChanged();

        }

        public void Dispose()
        {
            //NavigationManager.LocationChanged -= HandleLocationChanged;
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitData()
        {
            //Khối HightLight
            var lstHightLight = await Repository.ArticleBlock.ArticleBlockArticleGetLstByArticleBlockId(1);
            if (lstHightLight != null)
            {
                articleHightLight = lstHightLight.FirstOrDefault();
            }
            //Khối dọc 
            lstArticleVertical = await Repository.ArticleBlock.ArticleBlockArticleGetLstByArticleBlockId(3);

            //Khối 3 bài ngang
            var lstArticleHorizon = await Repository.ArticleBlock.ArticleBlockArticleGetLstByArticleBlockId(2);
            if (lstArticleHorizon != null)
            {
                lstArticleTop3 = lstArticleHorizon.Take(3).ToList();
            }



            //Quảng cáo khối bên phải
            lstAdvertisingRight = await Repository.Advertising.AdvertisingGetLstByBlockId(8);

        }

        #endregion Init

        #region Event

        #endregion
    }
}
