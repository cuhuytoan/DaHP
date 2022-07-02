namespace CMS.Website.Areas.Identity.Pages.Account
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Parameter]
        public string returnUrl { get; set; }
        protected override void OnInitialized()
        {
            if (returnUrl is not null)
            {
                NavigationManager.NavigateTo($"/Login?returnUrl={returnUrl}", true);
            }
            else
            {
                NavigationManager.NavigateTo($"/Login", true);
            }

        }
    }
}
