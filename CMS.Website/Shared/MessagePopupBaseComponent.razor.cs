namespace CMS.Website.Shared;
public partial class MessagePopupBaseComponent
{
    protected bool ShowConfirmation { get; set; }
    protected bool ShowBackdrop { get; set; }
    [Parameter]
    public string ConfirmationTitle { get; set; } = "Xác nhận xóa";

    [Parameter]
    public string ConfirmationMessage { get; set; } = "Bạn có chắc chắn muốn xóa ?";

    public void Show()
    {
        ShowConfirmation = true;
        ShowBackdrop = true;
        StateHasChanged();

    }

    [Parameter]
    public EventCallback<bool> ConfirmationChanged { get; set; }

    protected async Task OnConfirmationChange(bool value)
    {
        ShowConfirmation = false;
        ShowBackdrop = false;
        await ConfirmationChanged.InvokeAsync(value);
    }
}
