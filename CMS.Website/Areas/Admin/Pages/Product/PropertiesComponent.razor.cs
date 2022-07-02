namespace CMS.Website.Areas.Admin.Pages.Product
{
    public partial class PropertiesComponent
    {
        #region Inject

        [Inject]
        private IMapper Mapper { get; set; }

        [Inject]
        private ILoggerManager Logger { get; set; }

        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }

        #endregion Inject

        [Parameter]
        public int ProductId { get; set; }

        [Parameter]
        public EventCallback<bool> OnCloseModal { get; set; }

        [CascadingParameter]
        private GlobalModel globalModel { get; set; }

        private string productName = "";

        private ProductPropertiesDTO productPropertyModel { get; set; } = new();
        private List<ProductPropertyValue> lstProductPropertyValue { get; set; } = new();
        private List<KeyValuePair<string, string>> lstYesNo = new();

        #region LifeCycle
        protected override async Task OnInitializedAsync()
        {
            await InitControl();
            await InitData();
        }
        #endregion

        #region Init
        protected async Task InitControl()
        {
            lstYesNo = new();
            lstYesNo.Add(new KeyValuePair<string, string>("1", "Có"));
            lstYesNo.Add(new KeyValuePair<string, string>("0", "Không"));
        }
        protected async Task InitData()
        {
            if (ProductId > 0)
            {
                var productItem = await Repository.Product.FindAsync(ProductId);
                if (productItem != null)
                {
                    productName = productItem.Name;
                    var lstProductCategory = await Repository.ProductCategory.GetLstProductCatebyProductId(ProductId);
                    if (lstProductCategory != null)
                    {
                        int productCategoryId = lstProductCategory.Select(x => x.ProductCategoryId).FirstOrDefault();
                        var lstProdProCate = await Repository.ProductProperties.ProductPropertyCategoryGetLstByCategoryId(productCategoryId);

                        productPropertyModel.lstProductPropertyCate = Mapper.Map<List<ProductPropertiesCategoryDTO>>(lstProdProCate);
                        foreach (var p in productPropertyModel.lstProductPropertyCate)
                        {
                            p.lstProductProperties = await Repository.ProductProperties.ProductPropertiesSpGetLst(ProductId, p.Id);
                        }
                    }
                }
            }
            StateHasChanged();
        }
        private async Task OnPostData()
        {
            var result = await Repository.ProductProperties.ProductPropertyValueInsertOrUpdate(productPropertyModel, globalModel.userId, ProductId);
            if (result)
            {
                toastService.ShowSuccess("Cập nhật thành công", "Thông báo");
            }
            else
            {
                toastService.ShowError("Có lỗi trong quá trình cập nhật", "Thông báo");
            }
        }
        #endregion
    }
}
