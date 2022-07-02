using System.Collections.Concurrent;

namespace CMS.Website.Services
{
    public class LoginInfo
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
    public class BlazorCookieLoginMiddleware
    {

        public GlobalModel globalModel { get; set; } = new();
        public static IDictionary<Guid, LoginInfo> Logins { get; private set; }
            = new ConcurrentDictionary<Guid, LoginInfo>();


        private readonly RequestDelegate _next;

        public BlazorCookieLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, SignInManager<IdentityUser> signInMgr)
        {
            if (context.Request.Path == "/lg" && context.Request.Query.ContainsKey("key"))
            {
                var returnUrl = "/";
                var key = Guid.Parse(context.Request.Query["key"]);
                var info = Logins[key];
                if (context.Request.Query.ContainsKey("returnUrl"))
                {
                    returnUrl = String.IsNullOrEmpty(context.Request.Query["returnUrl"]) ? "/" : context.Request.Query["returnUrl"];
                }
                var result = await signInMgr.PasswordSignInAsync(info.Email, info.Password, false, lockoutOnFailure: true);
                info.Password = null;
                if (result.Succeeded)
                {
                    Logins.Remove(key);
                    context.Response.Redirect(returnUrl);
                    return;
                }
                else if (result.RequiresTwoFactor)
                {
                    //TODO: redirect to 2FA razor component
                    context.Response.Redirect("/loginwith2fa/" + key);
                    return;
                }
                else
                {
                    //TODO: Proper error handling
                    context.Response.Redirect("/loginfailed");
                    return;
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
