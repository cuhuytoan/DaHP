using CMS.Website.Shared;

namespace CMS.Website.Pages.Products
{
    public partial class ProductDetail : IDisposable
    {
        #region Inject

        [Inject]
        private IMapper Mapper { get; set; }

        [Inject]
        private ILoggerManager Logger { get; set; }

        [Inject]
        private UserManager<IdentityUser> UserManager { get; set; }

        #endregion Inject

        #region Parameter

        [Parameter]
        public string url { get; set; }

        #endregion Parameter

        #region Model

        private ProductDTO productDetail { get; set; } = new();

        private List<ProductPicture> lstProductPicture { get; set; } = new();
        private ProductBrandDTO productBrandDetail { get; set; } = new();        
        private int pointStar { get; set; } = 5;

        private string productReviewContent { get; set; }
        private List<SpProductReviewSearchResult> lstProductReview { get; set; }
        
        private bool ProductLike { get; set; } = false;
        private bool ProductBrandFollow { get; set; } = false;
        private int QtyToCart { get; set; } = 1;
        private ProductPropertiesDTO productPropertyModel { get; set; } = new();
        private CMS.Data.ModelEntity.Setting setting { get; set; } = new();
        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }


        #endregion Model

        #region LifeCycle



        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
            NavigationManager.LocationChanged += HandleLocationChanged;
        }
        protected override async Task OnParametersSetAsync()
        {

        }
        protected override async Task OnInitializedAsync()
        {
            await InitData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "https://cdnjs.cloudflare.com/ajax/libs/Swiper/3.1.2/js/swiper.min.js");

                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/assets/themes/front/assets/js/script.js");

                await JSRuntime.InvokeVoidAsync("swiperFunctions.init");


            }



        }

        #endregion LifeCycle

        #region Init

        protected async Task InitControl()
        {

            
        }

        protected async Task InitData()
        {
            setting = await Repository.Setting.GetSetting();

            var productDetailItem = await Repository.Product.ProductGetByUrl(url);
            if (productDetailItem == null)
            {
                NavigationManager.NavigateTo("/Error404");
                return;
            }
            productDetail = Mapper.Map<ProductDTO>(productDetailItem);
            if (productDetail.UnitId != null)
            {
                var item = await Repository.MasterData.UnitGetById((int)productDetail.UnitId);
                if (item != null)
                {
                    productDetail.UnitName = item.Name;
                }
            }
            if (productDetail.CountryId != null)
            {
                var item = await Repository.MasterData.CountryGetById((int)productDetail.CountryId);
                if (item != null)
                {
                    productDetail.CountryName = item.Name;
                }
            }
            if (productDetail.ProductCategoryIds != null)
            {
                var prodCategory = await Repository.ProductCategory.ProductCategoryGetByProductId((int)productDetail.Id);
                if (prodCategory != null)
                {
                    productDetail.ProductCategoryIdsName = prodCategory.Name;
                }
            }

            //GetLstProduct Picture
            lstProductPicture = await Repository.ProductPicture.ProductPictureGetLstByProductId((int)productDetail.Id);
            var productBrandItem = await Repository.ProductBrand.ProductBrandById((int)productDetail.ProductBrandId);
            if (productBrandItem == null)
            {
                NavigationManager.NavigateTo("/Error404");
                return;
            }
            productBrandDetail = Mapper.Map<ProductBrandDTO>(productBrandItem);

            if (productBrandDetail.LocationId != null)
            {
                var item = await Repository.MasterData.LocationGetById((int)productBrandDetail.LocationId);
                if (item != null)
                {
                    productBrandDetail.LocationName = item.Name;
                }
            }
            if(productBrandDetail.ProductBrandLevelId !=null)
            {
                var item = await Repository.MasterData.ProductBrandLevelsGetById((int)productBrandDetail.ProductBrandLevelId);
                if (item != null)
                {
                    productBrandDetail.ProductBrandLevelName = item.Name;
                }
            }    
            productBrandDetail.ProductCount = await Repository.Product.ProductCountByBrandId(productBrandDetail.Id);
            //Get ProductReview
            ProductReviewSearchFilter prodReviewFilter = new();
            prodReviewFilter.ProductId = (int)productDetail.Id;
            prodReviewFilter.CurrentPage = 1;
            prodReviewFilter.Active = true;
            prodReviewFilter.PageSize = 9999;
            prodReviewFilter.FromDate = DateTime.Now.AddYears(-10);
            prodReviewFilter.ToDate = DateTime.Now;
            var lstProdReview = await Repository.ProductReview.ProductReviewSearchWithPaging(prodReviewFilter);
            if (lstProdReview != null)
            {
                lstProductReview = lstProdReview.Items;
            }
            var pointProductReview = await Repository.ProductReview.ProductReviewGetAverage((int)productDetail.Id);

            if (pointProductReview != null)
            {
                productDetail.TotalProductReview = pointProductReview.TotalProductReview;
                productDetail.AveragePointReview = pointProductReview.AveragePointReview;
            }
            // Get Product Like
            ProductLike = await Repository.Product.ProductGetLikeByUserId((int)productDetail.Id, globalModel.userId);
            //Get ProductBrand Follow
            ProductBrandFollow = await Repository.ProductBrand.ProductBrandGetFollowByUserId((int)productDetail.ProductBrandId, globalModel.userId);
            //Increas Counter
            await Repository.Product.ProductIncreaseCount((int)productDetail.Id);
            await Repository.ProductBrand.ProductBrandIncreaseViewPageCount((int)productDetail.ProductBrandId);
            //Get ProductProperties
            var lstProductCategory = await Repository.ProductCategory.GetLstProductCatebyProductId((int)productDetail.Id);
            if (lstProductCategory != null)
            {
                int productCategoryId = lstProductCategory.Select(x => x.ProductCategoryId).FirstOrDefault();
                var lstProdProCate = await Repository.ProductProperties.ProductPropertyCategoryGetLstByCategoryId(productCategoryId);

                productPropertyModel.lstProductPropertyCate = Mapper.Map<List<ProductPropertiesCategoryDTO>>(lstProdProCate);
                foreach (var p in productPropertyModel.lstProductPropertyCate)
                {
                    p.lstProductProperties = await Repository.ProductProperties.ProductPropertiesSpGetLst((int)productDetail.Id, p.Id);
                }
            }
            StateHasChanged();
        }
        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }
        #endregion Init

        #region Event
        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitControl();
            await InitData();
        }


        protected async Task SaveToCart()
        {
            var countProduct = globalModel.totalInCart ?? 0;
            List<ProductCartDTO> lstProductInCart = new();
            var result = await BrowserStorage.GetAsync<List<ProductCartDTO>>("cart");
            if (result.Success)
            {
                lstProductInCart = result.Value;
                foreach (var p in result.Value)
                {
                    var checkExists = p.lstProduct.Find(x => x.Id == productDetail.Id);
                    if (checkExists != null)
                    {
                        toastService.ShowWarning("Sản phẩm đã tồn tại trong giỏ hàng", "Thông báo");
                        return;
                    }
                }
                //Insert
                ProductCartDTO item = new();
                productDetail.QtyInCart = QtyToCart;
                item.lstProduct.Add(productDetail);
                item.productBrand = productBrandDetail;
                lstProductInCart.Add(item);

                countProduct += 1;
            }
            else
            {
                //Insert
                ProductCartDTO item = new();
                productDetail.QtyInCart = QtyToCart;
                item.lstProduct.Add(productDetail);
                item.productBrand = productBrandDetail;
                lstProductInCart.Add(item);
                countProduct += 1;
            }

            await BrowserStorage.DeleteAsync("cart");

            await BrowserStorage.SetAsync("cart", lstProductInCart);

            globalModel.SetTotalInCart(countProduct);
            toastService.ShowSuccess("Sản phẩm đã được thêm vào giỏ hàng", "Thông báo");

        }
        protected async Task OnPostProductReview()
        {
            if (String.IsNullOrEmpty(productReviewContent))
            {
                toastService.ShowWarning("Bạn hãy nhập đánh giá về sản phẩm", "Thông báo");
            }
            else
            {
                //Check already evaluation
                var lstReview = await Repository.ProductReview.ProductReviewGetByUserId((int)productDetail.Id, globalModel.userId);
                if (lstReview != null & lstReview.Count > 0)
                {
                    toastService.ShowWarning("Bạn đã có đánh giá về sản phẩm này", "Thông báo");
                    return;
                }
                ProductReview model = new();
                model.ProductId = productDetail.Id;
                model.ProductBrandId = productDetail.ProductBrandId;
                model.Content = productReviewContent;
                model.Star = pointStar;
                model.CustomerName = globalModel.fullName;

                var result = await Repository.ProductReview.ProductReviewInsert(model, globalModel.userId);
                if (result > 0)
                {
                    toastService.ShowSuccess("Cảm ơn bạn đã đánh giá về sản phẩm, chúng tôi sẽ xét duyệt đánh giá của bạn", "Thông báo");
                    productReviewContent = "";
                }
            }
        }
        protected async Task OnChangeStar(int value)
        {
            pointStar = value;
            StateHasChanged();
        }
        protected async Task OnStarMouseOver(int value)
        {
            pointStar = value;
            StateHasChanged();
        }
        protected async Task OnChangeProductLike(bool productLike)
        {
            if (!globalModel.user.Identity.IsAuthenticated)
            {
                toastService.ShowError("Bạn cần đăng nhập để thực hiện chức năng này", "Thông báo");
                return;
            }
            if (productLike) // Delete
            {
                var result = await Repository.Product.ProductLikeDelete((int)productDetail.Id, globalModel.userId);
                if (result)
                {
                    productDetail.LikeCount -= 1;
                    ProductLike = !ProductLike;
                }
            }
            else
            {
                var result = await Repository.Product.ProductLikeInsert((int)productDetail.Id, (int)productDetail.ProductBrandId, globalModel.userId);
                if (result)
                {
                    productDetail.LikeCount += 1;
                    ProductLike = !ProductLike;
                }
            }
            StateHasChanged();
        }
        protected async Task OnChangeProductBrandFollow(bool productBrandFollow)
        {
            if (!globalModel.user.Identity.IsAuthenticated)
            {
                toastService.ShowError("Bạn cần đăng nhập để thực hiện chức năng này", "Thông báo");
                return;
            }
            if (productBrandFollow) // Delete
            {
                var result = await Repository.ProductBrand.ProductBrandFollowDelete((int)productDetail.ProductBrandId, globalModel.userId);
                if (result)
                {
                    productBrandDetail.FollowCount -= 1;
                    ProductBrandFollow = !ProductBrandFollow;
                }
            }
            else
            {
                var result = await Repository.ProductBrand.ProductBrandFollowInsert((int)productDetail.ProductBrandId, globalModel.userId);
                if (result)
                {
                    productBrandDetail.FollowCount += 1;
                    ProductBrandFollow = !ProductBrandFollow;
                }
            }
            StateHasChanged();
        }
        protected async Task SocialShare(string typeSocial)
        {
            if (typeSocial == "fb")
            {
                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Products/ProductDetail.razor.js");
                await JSRuntime.InvokeVoidAsync("ShareFunction.fbDialog");
            }
            else if (typeSocial == "copy")
            {
                await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Products/ProductDetail.razor.js");
                await JSRuntime.InvokeVoidAsync("ShareFunction.copyToClipBooard");
            }
        }
        protected async Task BuyNow()
        {

            NavigationManager.NavigateTo($"/dat-hang?productUrl={productDetail.Url}&Qty={QtyToCart}");
        }
        //For bug close swiper js
        [JSInvokable]
        public async Task RefeshData()
        {
            await InitData();
        }
        protected async Task OnQtyChange(int type)
        {
            if(type == 1) // plus
            {
                QtyToCart += 1;
            }
            else if (type == 2) // plus
            {
                if (QtyToCart <= 1) return;
                QtyToCart -= 1;
            }
           
            StateHasChanged();

        }
        #endregion
    }
}
