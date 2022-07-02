using CMS.Common;
using System.Net;

namespace CMS.Website.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly RepositoryWrapper Repository;
        private readonly IMapper Mapper;
        private readonly IWebHostEnvironment _env;
        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            RepositoryWrapper Repository,
            IMapper Mapper,
            IWebHostEnvironment _env)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            this.Repository = Repository;
            this.Mapper = Mapper;
            this._env = _env;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        public AspNetUserInfoDTO userInfo { get; set; } = new();
        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync(string provider, string returnUrl = null, string fr = null)
        {
            if (fr == "blazor") // Modify Call from Blazor
            {
                // Request a redirect to the external login provider.
                var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
                var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return new ChallengeResult(provider, properties);
            }
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                    var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };

                    var createUser = await _userManager.CreateAsync(user);
                    if (createUser.Succeeded)
                    {
                        createUser = await _userManager.AddLoginAsync(user, info);
                        if (createUser.Succeeded)
                        {
                            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                            //Insert new Profilers
                            userInfo.AspNetUserProfiles.UserId = user.Id;
                            await Repository.AspNetUsers.AspNetUserProfilesCreateNew(
                                Mapper.Map<AspNetUserProfiles>(userInfo.AspNetUserProfiles));
                            //Insert new Role                            
                            userInfo.AspNetUserRoles.UserId = user.Id;
                            userInfo.AspNetUserRoles.RoleId = "421d1e22-4d17-49d4-97d7-e6abc27ea497"; //Khách
                            await Repository.AspNetUsers.AspNetUserRolesCreateNew(
                                Mapper.Map<AspNetUserRoles>(userInfo.AspNetUserRoles));

                            var userId = await _userManager.GetUserIdAsync(user);
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            //Verify Code
                            await _userManager.ConfirmEmailAsync(user, code);
                            //Update avatar
                            var profileImgUrl = info.Principal.FindFirstValue("urn:google:picture");
                            if (profileImgUrl != null)
                            {
                                string fileName = "noimages.png";
                                var profileImg = $"Profile_{user.Id}";
                                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                                fileName = String.Format("{0}-{1}.{2}", profileImg, timestamp, ".jpg");
                                Uri uri = new Uri(profileImgUrl);
                                WebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                                var physicalPath = Path.Combine(_env.WebRootPath, "data/user/mainimages/original", fileName);
                                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                                {
                                    using (Stream stream = webResponse.GetResponseStream())
                                    {
                                        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                                        img.Save(physicalPath);
                                        img.Dispose();
                                    }
                                }
                                try
                                {
                                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/user/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/user/mainimages/small", fileName), 500, 500);
                                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/user/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/user/mainimages/thumb", fileName), 120, 120);
                                }
                                catch
                                {
                                }
                                var profileInfo = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(user.Id);
                                profileInfo.AvatarUrl = fileName;

                                await Repository.AspNetUsers.AspNetUserProfilesUpdate(
                                  Mapper.Map<AspNetUserProfiles>(profileInfo));
                            }

                            //var callbackUrl = Url.Page(
                            //    "/Account/ConfirmEmail",
                            //    pageHandler: null,
                            //    values: new { area = "Identity", userId = userId, code = code },
                            //    protocol: Request.Scheme);


                            //await Repository.Setting.SendMail("Xác nhận đăng ký tài khoản", Input.Email, Input.Email, "Xác nhận đăng ký tài khoản", $"Click vào đường dẫn để xác nhận đăng ký mới tài khoản <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Click</a>.");


                            // If account confirmation is required, we need to show the link if we don't have a real email sender
                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                            }

                            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in createUser.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return Page();
        }
    }


}

