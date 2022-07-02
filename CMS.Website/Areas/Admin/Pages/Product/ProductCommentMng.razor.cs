namespace CMS.Website.Areas.Admin.Pages.Product
{
    public partial class ProductCommentMng
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
        public int productId { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; }

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Thông tin";



        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }

        #endregion

        #region Model
        List<SpProductCommentSearchResult> lstProductComment { get; set; } = new();

        protected int productCommentSelected { get; set; }

        protected ConfirmBase DeleteConfirmation { get; set; }

        protected string productName { get; set; }
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
            var item = await Repository.Product.ProductGetById(productId);
            if (item != null)
            {
                productName = item.Name;
            }
            ProductCommentSearchFilter model = new ProductCommentSearchFilter();
            model.Keyword = "";
            model.ProductId = (int)productId;
            model.CreateBy = null;
            model.PageSize = 1000;
            model.CurrentPage = 1;

            var lstResult = await Repository.ProductComment.ProductCommentSearch(model);
            if (lstResult != null)
            {
                lstProductComment = lstResult.Items;
            }
        }
        #endregion

        #region Event
        async Task OnChangeCommentStatus(int commentId, bool active)
        {
            var item = await Repository.ProductComment.FindAsync(commentId);
            if (item != null)
            {
                item.Active = active;
                await Repository.ProductComment.ProductCommentPostComment(item);
                StateHasChanged();
            }
        }

        protected void Deletecomment(int? commentId)
        {
            productCommentSelected = (int)commentId;

            DeleteConfirmation.Show();
        }
        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                try
                {
                    await Repository.ProductComment.ProductCommentDelete(productCommentSelected);

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
