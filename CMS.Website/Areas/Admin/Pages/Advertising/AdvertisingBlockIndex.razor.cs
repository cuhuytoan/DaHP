namespace CMS.Website.Areas.Admin.Pages.Advertising
{
    public partial class AdvertisingBlockIndex
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
        protected List<AdvertisingBlock> lstAdvertisingBlock { get; set; } = new();
        protected ModalDynamicComponent PropertiesDynamicEdit { get; set; }
        #endregion Model

        #region LifeCycle



        protected override async Task OnInitializedAsync()
        {

            await InitData();
            StateHasChanged();
        }

        #endregion LifeCycle

        #region Init


        protected async Task InitData()
        {
            lstAdvertisingBlock = await Repository.Advertising.AdvertisingBlockGetAll();
        }

        #endregion Init

        #region Event

        #endregion
    }
}
