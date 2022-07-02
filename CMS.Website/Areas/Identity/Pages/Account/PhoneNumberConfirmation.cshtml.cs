namespace CMS.Website.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class PhoneNumberConfirmation : PageModel
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RepositoryWrapper Repository;
        public PhoneNumberConfirmation(UserManager<IdentityUser> userManager, RepositoryWrapper Repository)
        {
            _userManager = userManager;
            this.Repository = Repository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            public string PhoneNumber { get; set; }

            public string Code { get; set; }

        }
        public IActionResult OnGet(string phoneNumber = null)
        {
            if (phoneNumber == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await Repository.AspNetUsers.AspNetUsersGetByPhoneNumber(Input.PhoneNumber);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"Số điện thoại : {Input.PhoneNumber} không tồn tại trên hệ thống");
                return Page();
            }
            var userExists = await _userManager.FindByIdAsync(user.Id);
            if (user == null || userExists == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(userExists, Input.Code, Input.PhoneNumber);
            if (result)
            {
                return RedirectToPage("./ResetPassword", new { type = "Phone", phoneNumber = Input.PhoneNumber });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "code không hợp lệ");
                return Page();
            }

        }
        public class PhoneNum
        {
            public string phoneNumber { get; set; }
        }

        public async Task<JsonResult> OnPostResentSMS(string phoneNumber)
        {
            var user = await Repository.AspNetUsers.AspNetUsersGetByPhoneNumber(phoneNumber);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"Số điện thoại không tồn tại trong hệ thống");
                return new JsonResult("error");
            }
            else
            {
                var userExists = await _userManager.FindByIdAsync(user.Id);
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(userExists, phoneNumber);
                //Sent code to phone number
                SMSVerify model = new();
                model.PhoneNumber = Input.PhoneNumber;
                model.Content = $"ma xac thuc cua ban la: {code}";
                var result = await Repository.AspNetUsers.SendSMSVerify(model);
                if (result)
                {
                    return new JsonResult("success");
                }
            }
            return new JsonResult("success");
        }
    }
}
