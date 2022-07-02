namespace CMS.Website.Pages.Article
{
    public partial class ArticleBreadCrumbComponent
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

        private List<CMS.Data.ModelEntity.SpArticleBreadcrumbResult> lstArticleBreadcrumb { get; set; } = new();



        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }


        #endregion Model

        #region LifeCycle

        protected override async Task OnParametersSetAsync()
        {
            await InitData();
        }
        protected override async Task OnInitializedAsync()
        {
            await InitData();
        }


        #endregion LifeCycle

        #region Init

        protected async Task InitData()
        {
            if (breadCrumbType == "articleCategory")
            {
                var item = await Repository.ArticleCategory.FirstOrDefaultAsync(p => p.Url == url);
                if (item == null)
                {
                    NavigationManager.NavigateTo("/Error404");
                    return;
                }
                lstArticleBreadcrumb = await Repository.Article.ArticleBreadCrumbGetlst(item.Id);
            }
            else if (breadCrumbType == "article")
            {
                var item = await Repository.Article.FirstOrDefaultAsync(p => p.Url == url);
                if (item == null)
                {
                    NavigationManager.NavigateTo("/Error404");
                    return;
                }
                else
                {
                    var itemCategory = await Repository.ArticleCategory.ArticleCategoryGetByArticleId(item.Id);
                    if (itemCategory == null)
                    {
                        NavigationManager.NavigateTo("/Error404");
                        return;
                    }
                    lstArticleBreadcrumb = await Repository.Article.ArticleBreadCrumbGetlst(itemCategory.Id);
                }

            }

        }

        #endregion Init

    }
}
