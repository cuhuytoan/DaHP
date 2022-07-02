namespace CMS.Website.Areas.Admin.Pages.Article
{
    public partial class ArticleBlockIndex
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
        protected GlobalModel globalModel { get; set; }
        #endregion Parameter

        #region Model
        protected List<ArticleBlock> lstArticleBlock { get; set; } = new();
        protected ModalDynamicComponent PropertiesDynamicEdit { get; set; }
        #endregion Model

        #region LifeCycle



        protected override async Task OnInitializedAsync()
        {
            //await InitControl();
            await InitData();
            StateHasChanged();
        }

        #endregion LifeCycle

        #region Init


        protected async Task InitData()
        {
            lstArticleBlock = await Repository.ArticleBlock.ArticleBlockGetAll();
        }

        #endregion Init

        #region Event

        protected async Task ArticleBlockArticleShow(int articleBlockId)
        {
            var itemBlock = await Repository.ArticleBlock.FindAsync(articleBlockId);
            if (itemBlock != null)
            {
                ComponentMetadata componentMeta = new();
                componentMeta.ComponentType = typeof(ArticleBlockArticleEdit);
                componentMeta.ComponentParameters = new Dictionary<string, object>()
            {
                {"articleBlockId",articleBlockId }
            };
                PropertiesDynamicEdit.ConfirmationTitle = $"Chỉnh sửa bài viết trong khối block";
                PropertiesDynamicEdit.Component = componentMeta;

                PropertiesDynamicEdit.Show();
            }

        }
        #endregion
    }
}
