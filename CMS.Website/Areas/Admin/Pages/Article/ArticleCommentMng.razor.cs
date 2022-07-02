namespace CMS.Website.Areas.Admin.Pages.Article
{
    public partial class ArticleCommentMng
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
        public int articleId { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; }

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Thông tin";



        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }

        #endregion

        #region Model
        List<SpArticleCommentSearchResult> lstArticleComment { get; set; } = new();

        protected int articleCommentSelected { get; set; }

        protected ConfirmBase DeleteConfirmation { get; set; }

        protected string articleName { get; set; }
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
            var item = await Repository.Article.ArticleGetById(articleId);
            if (item != null)
            {
                articleName = item.Name;
            }
            ArticleCommentSearchFilter model = new ArticleCommentSearchFilter();
            model.Keyword = "";
            model.ArticleId = (int)articleId;
            model.CreateBy = null;
            model.PageSize = 1000;
            model.CurrentPage = 1;

            var lstResult = await Repository.ArticleComment.ArticleCommentSearch(model);
            if (lstResult != null)
            {
                lstArticleComment = lstResult.Items;
            }
        }
        #endregion

        #region Event
        async Task OnChangeCommentStatus(int commentId, bool active)
        {
            var item = await Repository.ArticleComment.FindAsync(commentId);
            if (item != null)
            {
                item.Active = active;
                await Repository.ArticleComment.ArticleCommentPostComment(item);
                StateHasChanged();
            }
        }

        protected void Deletecomment(int? commentId)
        {
            articleCommentSelected = (int)commentId;

            DeleteConfirmation.Show();
        }
        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                try
                {
                    await Repository.ArticleComment.ArticleCommentDelete(articleCommentSelected);

                    toastService.ShowToast(ToastLevel.Success, "Xóa bình luận thành công", "Thành công");
                }
                catch
                {
                    toastService.ShowToast(ToastLevel.Warning, "Có lỗi trong quá trình thực thi", "Lỗi!");
                }
                StateHasChanged();
                await InitData();
            }
        }
        #endregion
    }
}
