namespace CMS.Website.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RepositoryWrapper Repository;
        public ResetPasswordModel(UserManager<IdentityUser> userManager, RepositoryWrapper Repository)
        {
            _userManager = userManager;
            this.Repository = Repository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Mật khẩu tối thiểu 6 kí tự", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
            public string EmailConfirm { get; set; }
            public string Type { get; set; }
            public string PhoneNumber { get; set; }
        }

        public IActionResult OnGet(string type = null, string code = null, string email = null, string phoneNumber = null)
        {
            if (type == "Email")
            {
                if (code == null)
                {
                    return BadRequest("A code must be supplied for password reset.");
                }
                else
                {
                    Input = new InputModel
                    {
                        Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                        EmailConfirm = email,
                        Type = type
                    };
                    return Page();
                }
            }
            else if (type == "Phone")
            {
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber,
                    Type = type
                };
                return Page();
            }
            else
            {
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Input.Type == "Email")
            {
                var user = await _userManager.FindByEmailAsync(Input.EmailConfirm);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToPage("./ResetPasswordConfirmation");
                }

                var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
                if (result.Succeeded)
                {
                    return RedirectToPage("./ResetPasswordConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            else if (Input.Type == "Phone")
            {
                var user = await Repository.AspNetUsers.AspNetUsersGetByPhoneNumber(Input.PhoneNumber);
                var userExists = await _userManager.FindByIdAsync(user.Id);
                if (userExists != null)
                {
                    userExists.PasswordHash = _userManager.PasswordHasher.HashPassword(userExists, Input.Password);
                    var res = await _userManager.UpdateAsync(userExists);
                    if (res.Succeeded)
                    {
                        return RedirectToPage("./ResetPasswordConfirmation");
                    }
                }
            }
            return Page();
        }
    }
}
