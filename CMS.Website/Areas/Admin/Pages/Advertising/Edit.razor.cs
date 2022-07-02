using Microsoft.AspNetCore.Components.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace CMS.Website.Areas.Admin.Pages.Advertising
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
        public int? advertisingId { get; set; }
        [Parameter]
        [SupplyParameterFromQuery]
        public int? advertisingBlockId { get; set; }
        public CMS.Data.ModelEntity.Advertising advertising { get; set; } = new();

        private List<string> imageDataUrls = new List<string>();

        string outMessage = "";
        private bool isCropMainImage { get; set; }

        //Modal Crop Image
        protected ImageCropper imageCropperModal { get; set; }

        //Noti Hub
        [CascadingParameter]
        protected HubConnection hubConnection { get; set; }

        [CascadingParameter]
        private GlobalModel globalModel { get; set; }

        #endregion Parameter

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

        }

        protected async Task InitData()
        {
            if (advertisingId != null)
            {
                advertising = await Repository.Advertising.AdvertisingGetById((int)advertisingId);
            }
            else
            {
                advertising.AdvertisingBlockId = advertisingBlockId;
            }
        }

        #endregion Init

        #region Event

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            advertising.Image = null;
            var imageFiles = e.GetMultipleFiles();
            var format = "image/png";
            foreach (var item in imageFiles)
            {
                var resizedImageFile = await item.RequestImageFileAsync(format, 150, 150);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                //Clear Old Image
                imageDataUrls.Clear();
                imageDataUrls.Add(imageDataUrl);
            }
        }

        private async Task PostAdvertising()
        {
            var result = await Repository.Advertising.AdvertisingInsertOrUpdate(advertising);
            if (result)
            {
                //Save Main Image
                if (imageDataUrls != null & imageDataUrls.Count > 0)
                {
                    advertising.Image = await SaveMainImage((int)advertising.Id, imageDataUrls);
                }
                var resultUpdateImage = await Repository.Advertising.AdvertisingInsertOrUpdate(advertising);

                toastService.ShowSuccess("Cập nhật thành công", "Thông báo");
                NavigationManager.NavigateTo($"/Admin/Advertising?advertisingBlockId={advertising.AdvertisingBlockId}");

            }
            else
            {
                toastService.ShowSuccess("Có lỗi trong quá trình cập nhật", "Thông báo");
            }

        }




        //Save MainImage
        protected async Task<string> SaveMainImage(int AdvertisingId, List<string> imageDataUrls)
        {
            string fileName = "noimages.png";
            foreach (var file in imageDataUrls)
            {
                var imageDataByteArray = Convert.FromBase64String(CMS.Common.Utils.GetBase64Image(file));

                var urlAdvertising = Repository.Advertising.FormatURL(advertising.Name);
                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                fileName = String.Format("{0}-{1}.{2}", urlAdvertising, timestamp, "webp");
                var physicalPath = Path.Combine(_env.WebRootPath, "data/advertising/mainimages/original");
                ImageCodecInfo jpgEncoder = CMS.Common.Utils.GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID  
                // for the Quality parameter category.  
                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.  
                // An EncoderParameters object has an array of EncoderParameter  
                // objects. In this case, there is only one  
                // EncoderParameter object in the array.  
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                using (MemoryStream ms = new(imageDataByteArray))
                {
                    using Bitmap bm2 = new(ms);
                    bm2.Save(Path.Combine(physicalPath, fileName), jpgEncoder, myEncoderParameters);
                }

            }
            return fileName;
        }

        public string ToAbsoluteUrl(string url)
        {
            return $"{NavigationManager.BaseUri}{url}";
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

                    advertising.Image = null;
                    imageDataUrls.Clear();
                    imageDataUrls.Add(imageCropperModal.ImgData);

                    StateHasChanged();
                }
            }
        }





        #endregion Event

    }
}
