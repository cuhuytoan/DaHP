using CMS.Common;
using System.Drawing;
using System.Drawing.Imaging;
using Telerik.Blazor.Components;

namespace CMS.Website.Areas.Admin.Pages.ProductBrand
{
    public partial class Edit : IDisposable
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
        [SupplyParameterFromQuery]
        public int? productbrandId { get; set; }


        //Noti Hub
        [CascadingParameter]
        protected HubConnection hubConnection { get; set; }

        [CascadingParameter]
        private GlobalModel globalModel { get; set; }

        #endregion Parameter

        #region Model
        private List<Location> lstLocation { get; set; } = new();
        private List<District> lstDistrict { get; set; } = new();
        private List<Ward> lstWard { get; set; } = new();
        public ProductBrandEditDTO productbrand { get; set; } = new ProductBrandEditDTO();
        private List<KeyValuePair<string, string>> lstPersonSurName { get; set; }
        public int ProductbrandStatusId { get; set; } = 0;

        private List<string> imageDataUrls = new();
        public int postType { get; set; }

        string outMessage = "";
        private bool isCropMainImage { get; set; }
        //Modal Crop Image
        protected ImageCropper imageCropperModal { get; set; }

        public List<Bank> lstBank { get; set; } = new();

        public List<ProductBrandLevel> lstProductBrandLevel { get; set; } = new();

        //Setup upload end point
        public string SaveUrl => ToAbsoluteUrl("api/upload/SaveProductBrand");

        public string RemoveUrl => ToAbsoluteUrl("api/upload/RemoveProductBrand");
        ////List FileAttach Add new
        private List<ProductBrandAttachFile> lstAttachFile { get; set; } = new List<ProductBrandAttachFile>();

        ////List FileAttach binding
        private List<ProductBrandAttachFile> lstAttachFileBinding { get; set; } = new List<ProductBrandAttachFile>();
        #endregion

        #region LifeCycle

        protected override async Task OnInitializedAsync()
        {
            await InitControl();
            await InitData();
            StateHasChanged();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitControl()
        {
            //Binding lstGender
            List<KeyValuePair<string, string>> lstGenderAdd = new();
            lstGenderAdd.Add(new KeyValuePair<string, string>("Ông", "Ông"));
            lstGenderAdd.Add(new KeyValuePair<string, string>("Bà", "Bà"));
            lstPersonSurName = lstGenderAdd.ToList();
            //Binding lstLocation
            lstLocation = await Repository.MasterData.LocationGetLstByCountryId(1);
            lstLocation = lstLocation.Select(x => new Location { Id = x.Id, Name = x.Name }).ToList();
            //binding lstBank
            lstBank = await Repository.MasterData.BankGetLst();
            lstBank = lstBank.Select(x => new Bank { Id = x.Id, Name = x.Name }).ToList();
            //bindding lstProductBrand Level
            lstProductBrandLevel = await Repository.MasterData.ProductBrandLevelsGetlst();
            lstProductBrandLevel = lstProductBrandLevel.Select(x => new ProductBrandLevel { Id = x.Id, Name = x.Name }).ToList();
        }

        protected async Task InitData()
        {
            if (productbrandId is null)
            {
                NavigationManager.NavigateTo("/Error404");
                return;
            }
            else
            {
                var prodBrand = await Repository.ProductBrand.ProductBrandById((int)productbrandId);
                if (prodBrand is null)
                {
                    NavigationManager.NavigateTo("/Error404");
                    return;
                }
                productbrand = Mapper.Map<ProductBrandEditDTO>(prodBrand);
                //Binding attach file
                lstAttachFileBinding = await Repository.ProductBrand.ProductBrandAttachGetLstByProductBrandId((int)productbrand.Id);
            }
        }

        #endregion Init

        #region Event
        private async Task PostProductbrand()
        {
            //Update
            if (productbrand.Id > 0)
            {
                //Check permission
                if (!Repository.Permission.CanEditProductbrand(globalModel.user, globalModel.userId, (int)productbrand.Id, ref outMessage))
                {
                    toastService.ShowError(outMessage, "Thông báo");
                    return;
                }
                try
                {

                    //Save Main Image
                    if (imageDataUrls != null && imageDataUrls.Count > 0)
                    {
                        productbrand.Image = await SaveMainImage((int)productbrand.Id, imageDataUrls);
                    }
                    else
                    {
                        productbrand.Image = "noimages.jpg";
                    }
                    //Save Upload File
                    if (lstAttachFile.Count > 0)
                    {
                        lstAttachFile.ForEach(x =>
                        {
                            x.ProductBrandId = productbrand.Id;
                            x.CreateDate = DateTime.Now;
                            x.LastEditDate = DateTime.Now;
                            x.CreateBy = globalModel.userId;
                            x.LastEditBy = globalModel.userId;
                        });
                        var uploadResult = await Repository.ProductBrand.ProductBrandAttachInsert(lstAttachFile);
                        if (!uploadResult)
                        {
                            Logger.LogError("Upload File Error");
                        }
                    }

                    await Repository.ProductBrand.ProductBrandUpdate(Mapper.Map<CMS.Data.ModelEntity.ProductBrand>(productbrand), globalModel.userId);


                    //ToastMessage
                    toastService.ShowToast(ToastLevel.Success, "Cập nhật hàng thành công", "Thành công");

                }
                catch (Exception ex)
                {
                    //ToastMessage
                    toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình cập nhật {ex}", "Lỗi");
                }
            }
            NavigationManager.NavigateTo("/Admin/Productbrand/Index");
        }



        //Save MainImage
        protected async Task<string> SaveMainImage(int ProductbrandId, List<string> imageDataUrls)
        {
            string fileName = "noimages.png";
            foreach (var file in imageDataUrls)
            {
                var imageDataByteArray = Convert.FromBase64String(CMS.Common.Utils.GetBase64Image(file));

                var urlProductbrand = await Repository.ProductBrand.CreateProductBrandURL(ProductbrandId);
                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                fileName = String.Format("{0}-{1}.{2}", urlProductbrand, timestamp, "webp");
                var physicalPath = Path.Combine(_env.WebRootPath, "data/productbrand/mainimages/original");
                ImageCodecInfo jpgEncoder = CMS.Common.Utils.GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID  
                // for the Quality parameter category.  
                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.  
                // An EncoderParameters object has an array of EncoderParameter  
                // objects. In this case, there is only one  
                // EncoderParameter object in the array.  
                EncoderParameters myEncoderParameters = new(1);

                EncoderParameter myEncoderParameter = new(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                using (MemoryStream ms = new(imageDataByteArray))
                {
                    using Bitmap bm2 = new(ms);
                    bm2.Save(Path.Combine(physicalPath, fileName), jpgEncoder, myEncoderParameters);
                }
                try
                {
                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/productbrand/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/productbrand/mainimages/small", fileName), 500, 500);
                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/productbrand/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/productbrand/mainimages/thumb", fileName), 120, 120);
                }
                catch
                {
                }
            }
            return fileName;
        }

        private void OnCropImage(bool isMainImages)
        {
            isCropMainImage = isMainImages;

            imageCropperModal.Show();
        }

        protected void ConfirmImageCropper(bool isDone)
        {
            if (isDone)
            {
                if (imageCropperModal.ImgData != null)
                {
                    if (isCropMainImage)
                    {
                        productbrand.Image = null;
                        imageDataUrls.Clear();
                        imageDataUrls.Add(imageCropperModal.ImgData);
                    }
                    StateHasChanged();
                }
            }
        }

        private async void LocationSelected()
        {
            lstDistrict = productbrand.LocationId == null ? new() : await Repository.MasterData.DistrictsGetLstByLocationId((int)productbrand.LocationId);
            lstDistrict = lstDistrict.Select(x => new District { Id = x.Id, Name = x.Name }).ToList();
            StateHasChanged();
        }

        private async void DistrictSelected()
        {
            lstWard = productbrand.DistrictId == null ? new() : await Repository.MasterData.WardsGetLstByDistrictId((int)productbrand.DistrictId);
            lstWard = lstWard.Select(x => new Ward { Id = x.Id, Name = x.Name }).ToList();
            StateHasChanged();
        }
        public void OnSuccess(UploadSuccessEventArgs args)
        {
            lstAttachFile.Clear();
            foreach (var file in args.Files)
            {
                ProductBrandAttachFile item = new()
                {
                    AttachFileName = file.Name,
                    FileType = file.Extension,
                    FileSize = file.Size
                };
                lstAttachFile.Add(item);
            }
        }

        public void OnRemove(UploadEventArgs args)
        {
            foreach (var file in args.Files)
            {
                var itemDel = lstAttachFile.FirstOrDefault(p => p.AttachFileName == file.Name);
                if (itemDel != null)
                {
                    lstAttachFile.Remove(itemDel);
                }
            }
        }
        async Task OnSelectHandler(UploadSelectEventArgs e)
        {
            foreach (var item in e.Files)
            {
                // this will change the file name displayed in the TelerikUpload component
                // delays will result in a delay in rendering the file and starting the upload
                // NOTE: the file name in the XHR request to the server will be the original file name
                // and it is up to the server to handle it with the same logic for naming
                item.Name = await AskServerForFinalFileName(item.Name, productbrand.Name);
            }
        }
        async Task<string> AskServerForFinalFileName(string fileName, string productBrandName)
        {


            // in a real case this can be the controller that will save the files on the server
            // make sure that the same name generation logic will be used when actually saving the file
            string finalName = $"{productBrandName}-{fileName}";

            return await Task.FromResult(finalName);
        }
        private async Task DeleteAttachFile(int productBrandAttachFileId)
        {
            await Repository.ProductBrand.ProductBrandAttachDelete(productBrandAttachFileId);
            StateHasChanged();
        }
        public string ToAbsoluteUrl(string url)
        {
            return $"{NavigationManager.BaseUri}{url}";
        }
        #endregion Event
    }
}