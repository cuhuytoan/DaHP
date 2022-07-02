namespace CMS.Website.Pages.Article
{
    public partial class ArticleMostReadComponent : IDisposable
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
        public string url { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? articleCategoryId { get; set; }
        #endregion Parameter

        #region Model

        private List<SpArticleSearchResult> lstArticle { get; set; } = new();

        private Advertising advertisingTop { get; set; } = new();

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
            if (url != null)
            {
                var item = await Repository.ArticleCategory.FirstOrDefaultAsync(p => p.Url == url);
                if (item == null)
                {
                    NavigationManager.NavigateTo("/Error404");
                    return;
                }
                //first init
                articleCategoryId = item.Id;

            }
            await InitData();
            StateHasChanged();

        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitData()
        {
            ArticleSearchFilter model = new();
            model.CurrentPage = 1;
            model.PageSize = 8;
            model.ArticleCategoryId = articleCategoryId;
            model.FromDate = DateTime.Now.AddYears(-10);
            model.ToDate = DateTime.Now.AddYears(10);
            var lstItem = await Repository.Article.ArticleSearchWithPaging(model);
            if (lstItem != null)
            {
                lstArticle = lstItem.Items;
            }

            var lstAd = await Repository.Advertising.AdvertisingGetLstByBlockId(11);
            if (lstAd != null)
            {
                advertisingTop = lstAd.FirstOrDefault();
            }

            StateHasChanged();
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
