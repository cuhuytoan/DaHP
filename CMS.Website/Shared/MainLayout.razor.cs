namespace CMS.Website.Shared
{
    public partial class MainLayout
    {
        private HubConnection hubConnection;
        private GlobalModel globalModel { get; set; } = new GlobalModel();

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //get claim principal
            var authState = await authenticationStateTask;
            globalModel.user = authState.User;
            globalModel.userId = globalModel.user.FindFirstValue(ClaimTypes.NameIdentifier);

            hubConnection = new HubConnectionBuilder()
              .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHubs"), options =>
              {
                  options.Headers.Add("USERID", globalModel.userId);
                  options.Headers.Add("USERNAME", globalModel.user.Identity.Name);
              })
              .WithAutomaticReconnect()
              .Build();
            await hubConnection.StartAsync();
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
            //
            await InitData();
            globalModel.totalInCart = await GetTotalInCart();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/assets/themes/front/assets/js/script.js");
                await JSRuntime.InvokeVoidAsync("menuMobileFunction.init");
            }

        }
        private async Task InitData()
        {
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
            var result = await Repository.UserNoti.GetAllNoti(null, globalModel.userId, null, 3, 1);
            globalModel.lstUserNoti = result.Items;
            globalModel.totalUnread = result.TotalSize;
        }

        protected async Task<int> GetTotalInCart()
        {
            int total = 0;
            var result = await BrowserStorage.GetAsync<List<ProductCartDTO>>("cart");
            if (result.Success)
            {
                foreach (var p in result.Value)
                {
                    total += 1;
                }

            }
            return total;
        }

    }
}
