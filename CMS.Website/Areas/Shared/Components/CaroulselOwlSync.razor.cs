namespace CMS.Website.Areas.Shared.Components
{
    public partial class CaroulselOwlSync
    {
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                return JSRuntime.InvokeVoidAsync("InitOwlSync").AsTask();
            }
            return Task.CompletedTask;
        }
    }
}
