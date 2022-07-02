namespace CMS.Website.Areas.Admin.Pages.LogVisit
{
    public partial class Index
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


        [Parameter]
        [SupplyParameterFromQuery]
        public int? p { get; set; } = 1;
        #endregion Parameter

        #region Model

        private List<SpLogVisitSearchResult> lstLogVisit;

        public int totalCount { get; set; }
        public int pageSize { get; set; } = 100;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));
        

        #endregion Model

        #region LifeCycle

        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
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
            GC.SuppressFinalize(this);
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitControl()
        {
              
            
        }

        protected async Task InitData()
        {
            var result = await Repository.LogVisit.lstLogVisitPaging(p ?? 1, 100);
            if (result != null)
            {
                lstLogVisit = result.Items;
                totalCount = result.TotalSize;
            }
            StateHasChanged();
        }

        #endregion Init
        #region Event


        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitData();
        }

        #endregion Event
    }
}
