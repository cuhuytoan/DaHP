namespace CMS.Website.Areas.Admin.Pages.Advertising
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

        [Parameter]
        [SupplyParameterFromQuery]
        public int advertisingBlockId { get; set; }


        #endregion Parameter

        #region Model

        private List<CMS.Data.ModelEntity.Advertising> lstAdvertising { get; set; } = new();


        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }

        protected ConfirmBase DeleteConfirmation { get; set; }

        private List<int> listAdvertisingSelected { get; set; } = new List<int>();

        private bool isCheck { get; set; }
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

            lstAdvertising = await Repository.Advertising.AdvertisingGetLstByBlockId(advertisingBlockId);
        }

        #endregion Init

        #region Event
        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitData();
        }

        protected void DeleteAdvertising(int? advertisingId)
        {

            if (advertisingId == null) // Delete Demand
            {
                if (listAdvertisingSelected.Count == 0)
                {
                    toastService.ShowToast(ToastLevel.Warning, "Chưa chọn quảng cáo để xóa", "Thông báo");
                    return;
                }
            }
            else
            {
                listAdvertisingSelected.Clear();
                listAdvertisingSelected.Add((int)advertisingId);


            }

            DeleteConfirmation.Show();
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                try
                {
                    foreach (var item in listAdvertisingSelected)
                    {
                        await Repository.Advertising.AdvertisingDeleteById(item);
                    }
                    toastService.ShowToast(ToastLevel.Success, "Xóa quảng cáo thành công", "Thành công");
                }
                catch
                {
                    toastService.ShowToast(ToastLevel.Warning, "Có lỗi trong quá trình thực thi", "Lỗi!");
                }
                StateHasChanged();
                await InitData();
            }
        }
        protected void OnCheckBoxChange(bool headerChecked, int AdvertisingId, object isChecked)
        {
            if (headerChecked)
            {
                if ((bool)isChecked)
                {
                    listAdvertisingSelected.AddRange(lstAdvertising.Select(x => x.Id));
                    isCheck = true;
                }
                else
                {
                    isCheck = false;
                    listAdvertisingSelected.Clear();
                }
            }
            else
            {
                if ((bool)isChecked)
                {
                    if (!listAdvertisingSelected.Contains(AdvertisingId))
                    {
                        listAdvertisingSelected.Add(AdvertisingId);
                    }
                }
                else
                {
                    if (listAdvertisingSelected.Contains(AdvertisingId))
                    {
                        listAdvertisingSelected.Remove(AdvertisingId);
                    }
                }
            }
            StateHasChanged();
        }
        #endregion
    }
}
