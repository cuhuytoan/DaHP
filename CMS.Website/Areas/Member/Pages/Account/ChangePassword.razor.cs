namespace CMS.Website.Areas.Member.Pages.Account
{
    public partial class ChangePassword : IDisposable
    {
        #region Inject   
        [Inject]
        IMapper Mapper { get; set; }
        [Inject]
        ILoggerManager Logger { get; set; }
        [Inject]
        UserManager<IdentityUser> UserManager { get; set; }
        [Inject]
        SignInManager<IdentityUser> SignInManager { get; set; }
        #endregion



        #region Parameter

        public ChangePwdModel changePwdModel { get; set; } = new();

        protected bool isFirstSetPass { get; set; } = false;

        [CascadingParameter]
        private GlobalModel globalModel { get; set; }



        #endregion

        #region LifeCycle


        protected override async Task OnInitializedAsync()
        {
            var currentUser = await UserManager.FindByIdAsync(globalModel.userId);
            if (currentUser.PasswordHash == null)
            {
                isFirstSetPass = true;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Init

        #endregion

        #region Event

        async Task PostChangePassword()
        {
            var currentUser = await UserManager.FindByIdAsync(globalModel.userId);
            if (currentUser != null)
            {
                if (currentUser.PasswordHash == null) //Set Password
                {
                    string token = await UserManager.GeneratePasswordResetTokenAsync(currentUser);
                    var result = await UserManager.ResetPasswordAsync(currentUser, token, changePwdModel.Password);
                    if (result.Succeeded)
                    {
                        toastService.ShowSuccess("Cập nhật mật khẩu thành công", "Thông báo");

                        StateHasChanged();
                        return;
                    }
                }
                try
                {
                    if (String.IsNullOrEmpty(changePwdModel.CurrentPassword))
                    {
                        toastService.ShowError("Nhập mật khẩu hiện tại", "Thông báo");
                        StateHasChanged();
                    }
                    var result = await UserManager.ChangePasswordAsync(currentUser, changePwdModel.CurrentPassword, changePwdModel.Password);
                    await UserManager.UpdateAsync(currentUser);
                    if (result.Succeeded)
                    {
                        toastService.ShowToast(ToastLevel.Success, "Cập nhật mật khẩu thành công", "Thành công");
                    }
                    else
                    {
                        toastService.ShowToast(ToastLevel.Error, $"Mật khẩu cũ không chính xác", "Lỗi");
                    }

                }
                catch (Exception ex)
                {
                    toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình cập nhật {ex.ToString()}", "Lỗi");
                }

            }
            else
            {
                toastService.ShowToast(ToastLevel.Error, $"Không tồn tại tài khoản cập nhật", "Lỗi");
            }

        }

        #endregion
    }
}
