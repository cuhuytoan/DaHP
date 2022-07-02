namespace CMS.Website.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly RepositoryWrapper Repository;
        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, RepositoryWrapper Repository)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            this.Repository = Repository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool EnableInputCode { get; set; } = false;

        public class InputModel
        {
            [Required(ErrorMessage = "Nhập địa chỉ email")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (CMS.Common.Utils.IsEmail(Input.Email))
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToPage("./ForgotPasswordConfirmation");
                    }
                    // For more information on how to enable account confirmation and password reset please 
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code, email = Input.Email, type = "Email" },
                        protocol: Request.Scheme); ;
                    // gửi email
                    await Repository.Setting.SendMail("Hệ thống CMS", Input.Email, user.UserName, "Reset mật khẩu",
                        $"Click vào link để reset mật khẩu <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>reset mật khẩu</a>.");


                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                else if (CMS.Common.Utils.IsPhoneNumber(Input.Email))
                {
                    var user = await Repository.AspNetUsers.AspNetUsersGetByPhoneNumber(Input.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, $"Số điện thoại không tồn tại trong hệ thống");

                    }
                    else
                    {
                        var userExists = await _userManager.FindByIdAsync(user.Id);
                        var code = await _userManager.GenerateChangePhoneNumberTokenAsync(userExists, Input.Email);
                        //Sent code to phone number
                        SMSVerify model = new();
                        model.PhoneNumber = Input.Email;
                        model.Content = $"ma xac thuc la: {code}";
                        var result = await Repository.AspNetUsers.SendSMSVerify(model);
                        if (result)
                        {
                            return RedirectToPage("./PhoneNumberConfirmation", new { phoneNumber = Input.Email });
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Tài khoản là số điện thoại hoặc email");

                }

            }

            return Page();
        }
    }
}
