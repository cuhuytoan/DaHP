namespace CMS.Website.Areas.Admin.Pages.Employee
{
    public partial class Index : IDisposable
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
        public string keyword { get; set; }
        [Parameter]
        [SupplyParameterFromQuery]
        public int? roleId { get; set; }
        [Parameter]
        [SupplyParameterFromQuery]
        public int? p { get; set; }

        [CascadingParameter]
        public GlobalModel globalModel { get; set; }
        #endregion

        #region Model
        private List<SpAccountSearchResult> lstAccount;
        public int currentPage { get; set; }
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 30;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));
        public ArticleSearchFilter modelFilter { get; set; }
        public string roleSelected { get; set; }
        List<AspNetRoles> lstRoles { get; set; }
        protected SetPwdModel setPwdModel { get; set; } = new();

        protected ConfirmBase DeleteConfirmation { get; set; }
        List<string> lstAccountSelected { get; set; } = new List<string>();

        bool isCheck { get; set; }
        bool showSetPwdModal { get; set; }
        #endregion


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



        #endregion

        #region Init
        protected async Task InitControl()
        {
            //Binding Category
            var ListRoles = await Repository.AspNetUsers.AspNetRolesGetAll();
            if (ListRoles != null)
            {
                lstRoles = ListRoles.Select(x => new AspNetRoles { Id = x.Id.ToString(), Name = x.Name }).ToList();
            }

        }
        protected async Task InitData()
        {
            AccountSearchFilter modelFilter = new();
            modelFilter.Keyword = keyword;
            var pRole = Guid.TryParse(roleSelected, out Guid role);
            if (pRole)
            {
                modelFilter.RoleId = role;
            }

            modelFilter.CurrentPage = p ?? 1;
            modelFilter.PageSize = 30;
            var result = await Repository.AspNetUsers.GetLstUsersPaging(modelFilter);

            lstAccount = result.Items;
            totalCount = result.TotalSize;

            //Init Selected 
            lstAccountSelected.Clear();
            StateHasChanged();
        }

        #endregion

        #region Event
        protected async Task DeleteAccount(string userId)

        {
            if (userId == null) // Delete Demand
            {
                if (lstAccountSelected.Count == 0)
                {
                    toastService.ShowToast(ToastLevel.Warning, "Chưa chọn thành viên để xóa", "Thông báo");
                    return;
                }
            }
            else
            {
                lstAccountSelected.Clear();
                lstAccountSelected.Add(userId);
            }
            DeleteConfirmation.Show();
        }
        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                try
                {
                    foreach (var item in lstAccountSelected)
                    {
                        var currentUser = await Repository.AspNetUsers.FindAsync(item);
                        var currentProfile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(item);
                        if (currentProfile != null)
                        {
                            if (currentProfile.ProductBrandId != globalModel.productBrandId)
                            {
                                toastService.ShowError("Tài khoản xóa không hợp lệ", "Thông báo");
                                return;
                            }
                        }
                        if (currentUser != null)
                        {
                            //Không được xóa tài khoản hiện tại
                            if (currentUser.Id == globalModel.userId)
                            {
                                toastService.ShowError("Không thể xóa tài khoản của cửa hàng", "Thông báo");
                                return;
                            }

                            await Repository.AspNetUsers.AspNetUsersDelete(item);
                            await Repository.AspNetUsers.AspNetUserProfilesDeleteByUserId(item);
                            await Repository.AspNetUsers.AspNetUserRolesDelete(item);
                        }

                    }
                    toastService.ShowToast(ToastLevel.Success, "Xóa tài khoản thành công", "Thành công");
                }
                catch (Exception ex)
                {
                    Logger.LogError($"ConfirmDelete_Click: {ex}");
                    toastService.ShowToast(ToastLevel.Warning, "Có lỗi trong quá trình thực thi", "Lỗi!");
                }
                StateHasChanged();
                await InitData();
            }
        }

        protected void OnCheckBoxChange(bool headerChecked, string AspnetUserId, object isChecked)
        {
            if (headerChecked)
            {
                if ((bool)isChecked)
                {
                    lstAccountSelected.AddRange(lstAccount.Select(x => x.Id));
                    isCheck = true;
                }
                else
                {
                    isCheck = false;
                    lstAccountSelected.Clear();
                }
            }
            else
            {
                if ((bool)isChecked)
                {
                    if (!lstAccountSelected.Contains(AspnetUserId))
                    {
                        lstAccountSelected.Add(AspnetUserId);
                    }
                }
                else
                {
                    if (lstAccountSelected.Contains(AspnetUserId))
                    {
                        lstAccountSelected.Remove(AspnetUserId);
                    }
                }
            }
            StateHasChanged();

        }
        protected async Task OnShowSetPWD(string userName, bool isShow)
        {
            if (isShow)
            {
                var selectedUser = await UserManager.FindByNameAsync(userName);
                if (selectedUser != null)
                {
                    setPwdModel.UserName = userName;
                    showSetPwdModal = isShow;
                    StateHasChanged();
                }
            }
            else
            {
                showSetPwdModal = isShow;
                StateHasChanged();
            }

        }
        protected async Task OnSetPwd()
        {
            var selectedUser = await UserManager.FindByNameAsync(setPwdModel.UserName);
            if (selectedUser != null)
            {
                string token = await UserManager.GeneratePasswordResetTokenAsync(selectedUser);
                var result = await UserManager.ResetPasswordAsync(selectedUser, token, setPwdModel.Password);
                if (result.Succeeded)
                {
                    toastService.ShowSuccess("Cập nhật mật khẩu thành công", "Thông báo");
                    showSetPwdModal = false;
                    StateHasChanged();
                }
                else
                {
                    toastService.ShowError("Có lỗi trong quá trình cập nhật mật khẩu", "Thông báo");
                }
            }
        }
        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitData();
        }

        #endregion

    }
}
