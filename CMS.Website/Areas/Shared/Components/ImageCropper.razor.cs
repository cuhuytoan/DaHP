namespace CMS.Website.Areas.Shared.Components
{
    public partial class ImageCropper
    {
        public string response { get; set; }
        protected bool ShowConfirmation { get; set; }
        protected bool ShowAltText { get; set; }
        public string ImgData { get; set; }

        public async Task Show()
        {
            ShowConfirmation = true;
            await JSRuntime.InvokeVoidAsync("Crop.image", DotNetObjectReference.Create(this));

            StateHasChanged();
        }

        [Parameter]
        public EventCallback<bool> ConfirmationCropChanged { get; set; }

        protected async Task OnConfirmationChange(bool value)
        {
            ShowConfirmation = false;
            ImgData = response != null ? response.ToString() : "";
            await ConfirmationCropChanged.InvokeAsync(value);
        }

        [JSInvokable]
        public void ResponseMethod(string data)
        {
            response = data;
            StateHasChanged();
        }
    }
}