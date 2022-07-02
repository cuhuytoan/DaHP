namespace CMS.Website.Pages.Products
{
    public partial class ProductOthersComponent
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

        #endregion Parameter

        #region Model

        private List<CMS.Data.ModelEntity.SpProductSearchResult> lstProductRelated { get; set; } = new();



        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }


        #endregion Model

        #region LifeCycle
        protected override async Task OnInitializedAsync()
        {
            await InitData();
            StateHasChanged();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (lstProductRelated.Count > 0)
            {
                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/assets/themes/front/assets/js/script.js");

                await JSRuntime.InvokeVoidAsync("slickFunctions.init6Col");

            }


        }

        #endregion LifeCycle

        #region Init

        protected async Task InitData()
        {
            var productDetailItem = await Repository.Product.ProductGetByUrl(url);
            if (productDetailItem == null)
            {
                NavigationManager.NavigateTo("/Error404");
                return;
            }

            var prodCategory = await Repository.ProductCategory.ProductCategoryGetByProductId((int)productDetailItem.Id);


            ProductSearchFilter modelFilter = new()
            {
                ProductCategoryId = prodCategory.Id,
                CurrentPage = 1,
                PageSize = 10,
                ProductStatusId = 4
            };

            var result = await Repository.Product.ProductSearchWithPaging(modelFilter);

            lstProductRelated = result.Items;

            StateHasChanged();

        }

        #endregion Init
    }
}
