namespace CMS.Website.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public partial class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly RepositoryWrapper Repository;
        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager, RepositoryWrapper Repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            this.Repository = Repository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
            //[EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Mật khẩu không hợp lệ")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("/Shopman/Dashboard");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                if (CMS.Common.Utils.IsEmail(Input.Email))
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user != null)
                    {
                        Input.Email = user.UserName;
                    }
                }
                else if (CMS.Common.Utils.IsPhoneNumber(Input.Email))
                {
                    var user = await Repository.AspNetUsers.AspNetUsersGetByPhoneNumber(Input.Email);
                    if (user != null)
                    {
                        Input.Email = user.UserName;
                    }
                }
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //Update LastLoginDate
                    var user = await _userManager.FindByNameAsync(Input.Email);
                    if (user != null)
                    {
                        var profiles = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(user.Id);
                        if (profiles != null)
                        {
                            profiles.LastActivityDate = DateTime.Now;
                            profiles.LastLoginDate = DateTime.Now;
                            await Repository.AspNetUsers.AspNetUserProfilesUpdate(profiles);
                        }
                    }
                    // Update sercurity Stamp Force Logout

                    //await _userManager.UpdateSecurityStampAsync(user);
                    return LocalRedirect(returnUrl ?? "/");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Thông tin đăng nhập không chính xác");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}