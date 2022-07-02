namespace CMS.Website.Areas.Shared
{
    public partial class ShopmanLayout
    {
        private HubConnection hubConnection;
        private GlobalModel globalModel { get; set; } = new();

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        ErrorBoundary errorBoundary;

        protected override async Task OnParametersSetAsync()
        {
            // On each page navigation, reset any error state
            errorBoundary?.Recover();


        }
        protected override void OnInitialized()
        {

        }

        protected override async Task OnInitializedAsync()
        {


            //get claim principal
            if (globalModel.user == null)
            {
                var authState = await authenticationStateTask;
                globalModel.user = authState.User;
                globalModel.userId = globalModel.user.FindFirstValue(ClaimTypes.NameIdentifier);
                var profiles = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(globalModel.userId);
                if (profiles != null)
                {
                    globalModel.avatar = profiles.AvatarUrl;
                    globalModel.productBrandId = profiles.ProductBrandId;
                    if (String.IsNullOrEmpty(profiles.FullName))
                    {
                        globalModel.fullName = globalModel.user.Identity.Name;
                    }
                    else
                    {
                        globalModel.fullName = profiles.FullName;
                    }
                    if (profiles.ProductBrandId != null && profiles.ProductBrandId != 0)
                    {
                        var prodBrand = await Repository.ProductBrand.ProductBrandById((int)profiles.ProductBrandId);
                        if (prodBrand != null)
                        {
                            globalModel.productBrandImage = prodBrand.Image;
                            globalModel.productBrandStatusId = prodBrand.ProductBrandStatusId;
                        }
                    }
                    globalModel.avatar = profiles.AvatarUrl ?? "noimages.png";
                }
                //check active produot brand
                if (globalModel.user.Identity.IsAuthenticated && globalModel.productBrandStatusId != 4)
                {
                    NavigationManager.NavigateTo("/cua-hang-cho-duyet", true);
                }

            }

            hubConnection = new HubConnectionBuilder()
              .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHubs"), options =>
              {
                  options.Headers.Add("USERID", globalModel.userId);
                  options.Headers.Add("USERNAME", globalModel.user.Identity.Name);
              })
              .WithAutomaticReconnect()
              .Build();

            hubConnection.On<string, string, string, string, string>("ReceiveMessage", (toUserId, subject, content, url, imageUrl) =>
            {
                //ToastMessage
                if (globalModel.userId == toUserId)
                {
                    toastService.ShowToast(ToastLevel.Info, $"{subject}", "Bạn có thông báo mới");
                    globalModel.lstUserNoti.Insert(0, new SpUserNotifySearchResult
                    {
                        Subject = subject,
                        Content = content,
                        URL = url,
                        CreateDate = DateTime.Now
                    });
                    globalModel.totalUnread += 1;
                    StateHasChanged();
                }
            });

            await hubConnection.StartAsync();
            await InitData();

        }

        private async Task InitData()
        {

            var result = await Repository.UserNoti.GetAllNoti(null, globalModel.userId, null, 3, 1);
            globalModel.lstUserNoti = result.Items;
            globalModel.totalUnread = result.TotalSize;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/assets/themes/default/js/custom.js");

                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/assets/cropperjs/_scripts.js");
            }
        }



        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}