namespace CMS.Website.Areas.Admin.Pages.Article
{
    public partial class ArticleRelation
    {
        #region Inject   
        [Inject]
        IMapper Mapper { get; set; }
        [Inject]
        ILoggerManager Logger { get; set; }
        [Inject]
        UserManager<IdentityUser> UserManager { get; set; }

        #endregion

        #region Parameter

        [Parameter]
        public int ArticleId { get; set; }

        [Parameter]
        public EventCallback<bool> OnCloseModal { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; }

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Thông tin";

        public string keyword { get; set; }
        protected string articleName { get; set; }
        private List<SpArticleSearchResult> lstArticle;
        private List<SpArticleSearchResult> lstArticleSelected = new List<SpArticleSearchResult>();

        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }

        #endregion

        #region LifeCycle

        protected override async Task OnInitializedAsync()
        {

            await InitControl();
            await InitData();
            StateHasChanged();
        }
        #endregion

        #region Init
        protected async Task InitControl()
        {



        }
        protected async Task InitData()
        {
            var articleItem = await Repository.Article.ArticleGetById(ArticleId);
            if (articleItem != null)
            {
                articleName = articleItem.Name;
                lstArticleSelected = await Repository.Article.ArticleRelationGetlstByArticleId(ArticleId);
                //Init List Article
                var modelFilter = new ArticleSearchFilter();
                modelFilter.Keyword = "";
                modelFilter.CurrentPage = 1;
                modelFilter.PageSize = 100;
                modelFilter.ArticleStatusId = 4; // Bài viết đã sơ duyệt
                modelFilter.FromDate = DateTime.Now.AddYears(-10);
                modelFilter.ToDate = DateTime.Now;
                var resultArticle = await Repository.Article.ArticleSearchWithPaging(modelFilter);
                if (resultArticle != null)
                {
                    lstArticle = resultArticle.Items;
                }
            }

        }
        #endregion

        #region Event

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }


        async Task OnRemmoveArticleRelationArticle(SpArticleSearchResult item)
        {
            if (item == null)
            {
                return;
            }
            else
            {
                if (!lstArticleSelected.Any(x => x.Id == item.Id))
                {
                    toastService.ShowWarning("Bài viết không tồn tại", "Thông báo");
                    return;
                }
                lstArticleSelected.Remove(item);
                var deleteItem = await Repository.Article.ArticleRelationDeleteById(ArticleId, item.Id);
                if (deleteItem)
                {
                    toastService.ShowSuccess("Xóa thành công", "Thông báo");
                }
                StateHasChanged();
            }
        }
        async Task OnSearchArticle(string keyword)
        {
            //Init List Article
            var modelFilter = new ArticleSearchFilter();
            modelFilter.Keyword = keyword;
            //modelFilter.ArticleCategoryId = articleCategorySelected;
            modelFilter.CurrentPage = 1;
            modelFilter.PageSize = 100;
            modelFilter.ArticleStatusId = 4; // Bài viết đã duyệt
            modelFilter.FromDate = DateTime.Now.AddYears(-10);
            modelFilter.ToDate = DateTime.Now;
            var resultArticle = await Repository.Article.ArticleSearchWithPaging(modelFilter);
            if (resultArticle != null)
            {
                lstArticle = resultArticle.Items;
            }
            StateHasChanged();
        }
        async Task OnAddArticleRelationArticle(SpArticleSearchResult item)
        {
            if (item == null)
            {
                return;
            }
            else
            {
                if (lstArticleSelected.Any(x => x.Id == item.Id))
                {
                    toastService.ShowWarning("Bài viết đã tồn tại trong danh sách", "Thông báo");
                    return;
                }
                lstArticleSelected.Add(item);
                StateHasChanged();
            }
        }
        protected async Task OnPost()
        {
            List<ArticleRelationArticle> lstItem = new();
            foreach (var p in lstArticleSelected)
            {
                ArticleRelationArticle itemRelation = new();
                itemRelation.ArticleId = ArticleId;
                itemRelation.ArticleRelationId = p.Id;
                lstItem.Add(itemRelation);
            }
            var result = await Repository.Article.ArticleRelationInsert(lstItem);
            if (result)
            {
                toastService.ShowSuccess("Cập nhật thành công", "Thông báo");
            }
            else
            {
                toastService.ShowError("Có lỗi khi cập nhật", "Thông báo");

            }
        }
        #endregion

    }
}
