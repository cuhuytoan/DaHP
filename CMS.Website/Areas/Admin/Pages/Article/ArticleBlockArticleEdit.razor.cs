namespace CMS.Website.Areas.Admin.Pages.Article
{
    public partial class ArticleBlockArticleEdit
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
        public int articleBlockId { get; set; }

        [Parameter]
        public EventCallback<bool> OnCloseModal { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; }

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Thông tin";

        public string keyword { get; set; }
        protected string articleBlockName { get; set; }
        private List<SpArticleSearchResult> lstArticle;
        private List<SpArticleSearchResult> lstArticleSelected = new List<SpArticleSearchResult>();

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        ClaimsPrincipal user;
        #endregion

        #region LifeCycle

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine(articleBlockId);
            //get claim principal
            var authState = await authenticationStateTask;
            user = authState.User;
            //
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
            var articleBlockItem = await Repository.ArticleBlock.FindAsync(articleBlockId);
            if (articleBlockItem != null)
            {
                articleBlockName = articleBlockItem.Name;
                lstArticleSelected = await Repository.ArticleBlock.ArticleBlockArticleGetLstByArticleBlockId(articleBlockId);
                //Init List Article
                var modelFilter = new ArticleSearchFilter();
                modelFilter.Keyword = "";
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
                var deleteItem = await Repository.ArticleBlock.ArticleBlockArticleDeleteById(articleBlockId, item.Id);
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
            List<ArticleBlockArticle> lstItem = new();
            foreach (var p in lstArticleSelected)
            {
                ArticleBlockArticle itemRelation = new();
                itemRelation.ArticleBlockId = articleBlockId;
                itemRelation.ArticleId = p.Id;
                lstItem.Add(itemRelation);
            }
            var result = await Repository.ArticleBlock.ArticleBlockArticleInsert(lstItem);
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
