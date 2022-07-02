namespace CMS.Website.Pages.Home
{
    public partial class Index : IDisposable
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


        #endregion Parameter

        #region Model

        private List<CMS.Data.ModelEntity.Advertising> lstAdBannerFull { get; set; } = new();
        private List<CMS.Data.ModelEntity.Advertising> lstBanner3Item { get; set; } = new();

        private CMS.Data.ModelEntity.Setting setting { get; set; } = new();

        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }


        #endregion Model

        #region LifeCycle
        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
            //NavigationManager.LocationChanged += HandleLocationChanged;
        }
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine();
            await InitData();
            StateHasChanged();

        }


        public void Dispose()
        {
            //NavigationManager.LocationChanged -= HandleLocationChanged;
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitData()
        {
            lstAdBannerFull = await Repository.Advertising.AdvertisingGetLstByBlockId(3);
            lstBanner3Item = await Repository.Advertising.AdvertisingGetLstByBlockId(4);

            setting = await Repository.Setting.GetSetting();
        }

        #endregion Init

        #region Event

        //protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        //{
        //    var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        //    Console.WriteLine(uri);
        //}
        #endregion
    }
}
