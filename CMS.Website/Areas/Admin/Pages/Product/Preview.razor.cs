namespace CMS.Website.Areas.Admin.Pages.Product
{
    public partial class Preview : IDisposable
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
        [SupplyParameterFromQuery]
        public int? productId { get; set; }
        public ProductDTO product { get; set; } = new ProductDTO();
        public int ProductStatusId { get; set; } = 0;
        public string productTypeName { get; set; }
        [Required(ErrorMessage = "Nhập bình luận")]
        [MinLength(50, ErrorMessage = "Bình luận tối thiểu 50 kí tự")]
        public string comment { get; set; }
        public int editCommentId { get; set; }
        //List ProductComment 
        List<SpProductCommentStaffSearchResult> lstProductComment { get; set; }
        ////List FileAttach binding
        List<ProductAttachFile> lstAttachFileBinding { get; set; } = new List<ProductAttachFile>();
        //Noti Hub
        [CascadingParameter]
        protected HubConnection hubConnection { get; set; }
        [CascadingParameter]
        private GlobalModel globalModel { get; set; }
        string outMessage = "";

        #endregion

        #region LifeCycle      
        protected override void OnInitialized()
        {

            NavigationManager.LocationChanged += HandleLocationChanged;
        }
        protected override async Task OnInitializedAsync()
        {

            await InitControl();
            await InitData();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }
        #endregion

        #region Init
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async Task InitControl()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {


        }
        protected async Task InitData()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
            if (queryStrings.TryGetValue("productId", out var _productId))
            {
                this.productId = Convert.ToInt32(_productId);
            }

            if (productId != null)
            {
                if (!Repository.Permission.CanViewProduct(globalModel.user, globalModel.userId, (int)productId, ref outMessage))
                {
                    toastService.ShowError(outMessage, "Thông báo");
                    return;
                }
                var result = await Repository.Product.ProductGetById((int)productId);
                if (result != null)
                {
                    product = Mapper.Map<ProductDTO>(result);
                }
                lstAttachFileBinding = await Repository.Product.ProductAttachGetLstByProductId((int)productId);
                ProductCommentStaffSearchFilter model = new()
                {
                    Keyword = "",
                    ProductId = (int)productId,
                    Active = true,
                    CreateBy = null,
                    PageSize = 100,
                    CurrentPage = 1
                };

                var lstResult = await Repository.ProductCommentStaff.ProductCommentStaffSearch(model);
                if (lstResult != null)
                {
                    lstProductComment = lstResult.Items;
                }

            }
        }
        #endregion

        #region Event
        async Task OnPostComment()
        {
            if (!Repository.Permission.CanCommentProduct(globalModel.user, globalModel.userId, (int)productId, ref outMessage))
            {
                toastService.ShowToast(ToastLevel.Error, outMessage, "Thông báo");
                return;
            }
            ProductCommentStaff item = new()
            {
                Id = editCommentId,
                ProductId = productId,
                Content = comment,
                Name = globalModel.user.Identity.Name,
                CreateBy = globalModel.userId,
                CreateDate = DateTime.Now,
                Email = UserManager.GetUserName(globalModel.user),
                Active = true
            };


            try
            {
                await Repository.ProductCommentStaff.ProductCommentStaffPostComment(item);
                await InitData();
                comment = "";
                editCommentId = 0;
                StateHasChanged();

                toastService.ShowToast(ToastLevel.Success, "Bạn dã gửi bình luận thành công", "Thành công");


            }
            catch (Exception ex)
            {
                toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình gửi bình luận {ex}", "Thất bại");
            }

        }


        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitData();
        }

        protected async Task OnPostDemand(int postType)
        {

            try
            {
                await Repository.Product.ProductUpdateStatusType(globalModel.userId, (int)productId, postType);

                toastService.ShowToast(ToastLevel.Success, "Sơ duyệt thành công", "Thông báo");
                //Noti for user
                await hubConnection.SendAsync("SendNotification", product.CreateBy, "Sơ duyệt thành công", $"Ban biên tập đã sơ duyệt thành công bài viết của bạn", $"/Admin/Product/Preview?productId={product.Id}", product.Image);
            }
            catch
            {
                toastService.ShowToast(ToastLevel.Warning, "Có lỗi trong quá trình thực thi", "Lỗi!");
            }
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            InitData();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            StateHasChanged();


        }

        protected void OnEditComment(int commentId, string content)
        {
            if (Repository.Permission.CanEditCommentProduct(globalModel.user, globalModel.userId, commentId, ref outMessage))
            {
                comment = content;
                editCommentId = commentId;
                StateHasChanged();
            }
        }

    }
    #endregion
}
