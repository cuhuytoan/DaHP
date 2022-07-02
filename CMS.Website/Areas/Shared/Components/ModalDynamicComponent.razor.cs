namespace CMS.Website.Areas.Shared.Components
{
    public partial class ModalDynamicComponent
    {
        protected bool ShowForm { get; set; }

        [Parameter]
        public ComponentMetadata Component { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; }

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Thông tin";

        public void Show()
        {
            ShowForm = true;
            StateHasChanged();
        }

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }

        protected async Task OnConfirmationChange(bool value)
        {
            ShowForm = false;
            await ConfirmationChanged.InvokeAsync(value);
        }
        public void CloseModel()
        {
            ShowForm = false;
            StateHasChanged();
        }
    }
}
