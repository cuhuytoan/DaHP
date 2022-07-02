namespace CMS.Website.Areas.Admin.Pages.Product
{
    public partial class ProductBlockIndex
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
        protected List<ProductBlock> lstProductBlock { get; set; } = new();
        protected ModalDynamicComponent PropertiesDynamicEdit { get; set; }
        #endregion Model

        #region LifeCycle



        protected override async Task OnInitializedAsync()
        {
            await InitControl();
            await InitData();
            StateHasChanged();
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitControl()
        {


        }

        protected async Task InitData()
        {
            lstProductBlock = await Repository.ProductBlock.ProductBlockGetAll();
        }

        #endregion Init

        #region Event

        protected async Task ProductBlockProductShow(int ProductBlockId)
        {
            var itemBlock = await Repository.ProductBlock.FindAsync(ProductBlockId);
            if (itemBlock != null)
            {
                ComponentMetadata componentMeta = new();
                componentMeta.ComponentType = typeof(ProductBlockProductEdit);
                componentMeta.ComponentParameters = new Dictionary<string, object>()
            {
                {"ProductBlockId",ProductBlockId }
            };
                PropertiesDynamicEdit.ConfirmationTitle = $"Chỉnh sửa sản phẩm trong khối block";
                PropertiesDynamicEdit.Component = componentMeta;

                PropertiesDynamicEdit.Show();
            }

        }
        #endregion
    }
}
