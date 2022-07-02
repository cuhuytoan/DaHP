namespace CMS.Website.Areas.Admin.Pages.Product
{
    public partial class ProductRelation
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
        public int ProductId { get; set; }

        [Parameter]
        public EventCallback<bool> OnCloseModal { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; }

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Thông tin";

        public string keyword { get; set; }
        protected string productName { get; set; }
        private List<SpProductSearchResult> lstProduct;
        private List<SpProductSearchResult> lstProductSelected = new List<SpProductSearchResult>();

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        ClaimsPrincipal user;
        #endregion

        #region LifeCycle

        protected override async Task OnInitializedAsync()
        {
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
            var productItem = await Repository.Product.ProductGetById(ProductId);
            if (productItem != null)
            {
                productName = productItem.Name;
                lstProductSelected = await Repository.Product.ProductRelationGetlstByProductId(ProductId);
                //Init List Product
                var modelFilter = new ProductSearchFilter();
                modelFilter.Keyword = "";
                modelFilter.CurrentPage = 1;
                modelFilter.PageSize = 100;
                modelFilter.ProductStatusId = 4; // Bài viết đã sơ duyệt
                modelFilter.FromDate = DateTime.Now.AddYears(-10);
                modelFilter.ToDate = DateTime.Now;
                var resultProduct = await Repository.Product.ProductSearchWithPaging(modelFilter);
                if (resultProduct != null)
                {
                    lstProduct = resultProduct.Items;
                }
            }

        }
        #endregion

        #region Event

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }


        async Task OnRemmoveProductRelationProduct(SpProductSearchResult item)
        {
            if (item == null)
            {
                return;
            }
            else
            {
                if (!lstProductSelected.Any(x => x.Id == item.Id))
                {
                    toastService.ShowWarning("Bài viết không tồn tại", "Thông báo");
                    return;
                }
                lstProductSelected.Remove(item);
                var deleteItem = await Repository.Product.ProductRelationDeleteById(ProductId, item.Id);
                if (deleteItem)
                {
                    toastService.ShowSuccess("Xóa thành công", "Thông báo");
                }
                StateHasChanged();
            }
        }
        async Task OnSearchProduct(string keyword)
        {
            //Init List Product
            var modelFilter = new ProductSearchFilter();
            modelFilter.Keyword = keyword;
            //modelFilter.ProductCategoryId = productCategorySelected;
            modelFilter.CurrentPage = 1;
            modelFilter.PageSize = 100;
            modelFilter.ProductStatusId = 4; // Bài viết đã duyệt
            modelFilter.FromDate = DateTime.Now.AddYears(-10);
            modelFilter.ToDate = DateTime.Now;
            var resultProduct = await Repository.Product.ProductSearchWithPaging(modelFilter);
            if (resultProduct != null)
            {
                lstProduct = resultProduct.Items;
            }
            StateHasChanged();
        }
        async Task OnAddProductRelationProduct(SpProductSearchResult item)
        {
            if (item == null)
            {
                return;
            }
            else
            {
                if (lstProductSelected.Any(x => x.Id == item.Id))
                {
                    toastService.ShowWarning("Bài viết đã tồn tại trong danh sách", "Thông báo");
                    return;
                }
                lstProductSelected.Add(item);
                StateHasChanged();
            }
        }
        protected async Task OnPost()
        {
            List<ProductRelationProduct> lstItem = new();
            foreach (var p in lstProductSelected)
            {
                ProductRelationProduct itemRelation = new();
                itemRelation.ProductId = ProductId;
                itemRelation.ProductRelationId = p.Id;
                lstItem.Add(itemRelation);
            }
            var result = await Repository.Product.ProductRelationInsert(lstItem);
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
