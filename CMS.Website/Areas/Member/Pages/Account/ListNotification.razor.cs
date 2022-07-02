namespace CMS.Website.Areas.Member.Pages.Account
{
    public partial class ListNotification : IDisposable
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
        public int? p { get; set; }
        #endregion

        #region Model
        List<SpUserNotifySearchResult> lstUserNoti { get; set; } = new List<SpUserNotifySearchResult>();
        public int? totalUnread { get; set; }
        public int currentPage { get; set; }
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 30;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));


        [CascadingParameter]
        private GlobalModel globalModel { get; set; }
        protected ConfirmBase DeleteConfirmation { get; set; }

        #endregion

        #region LifeCycle
        protected override async Task OnParametersSetAsync()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);

            if (queryStrings.TryGetValue("p", out var _p))
            {
                this.currentPage = Convert.ToInt32(_p);
                this.p = Convert.ToInt32(_p);
            }

            await InitData();
        }

        protected override async Task OnInitializedAsync()
        {
            //get claim principal
            //var authState = await authenticationStateTask;
            //user = authState.User;
            //userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            await InitData();


        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Init
        private async Task InitData()
        {
            var result = await Repository.UserNoti.GetAllNoti(null, globalModel.userId, null, 10, 1);
            lstUserNoti = result.Items;
            totalUnread = result.TotalSize;
        }
        #endregion
    }
}
