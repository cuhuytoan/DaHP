using CMS.Common;
using Microsoft.AspNetCore.Components.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Editor;
using FontFamily = Telerik.Blazor.Components.Editor.FontFamily;

namespace CMS.Website.Areas.Admin.Pages.Product
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

        public int? productId { get; set; }
        public ProductDTO product { get; set; } = new ProductDTO();
        public int ProductStatusId { get; set; } = 0;
        //private List<ProductCategory> lstProductCategory { get; set; } =new();
        private List<SpProductCategoryTreeResult> lstProductCategory { get; set; } = new();
        private List<ProductType> lstProductType { get; set; } = new();
        private List<ProductManufacture> lstProductManufactures { get; set; } = new();
        private List<Country> lstCountry { get; set; } = new();
        private List<Unit> lstUnit { get; set; } = new();
        public string PreviewImage { get; set; }
        public int SelectedCateValue { get; set; } = -1;
        public List<string> SelectedCateName { get; set; } = new List<string>();
        private long maxFileSize = 1024 * 1024 * 15;
        private List<string> lstMainImages = new();
        private List<string> lstSubImageData = new();
        private IReadOnlyList<IBrowserFile> lstSubImagesUpload;
        private List<ProductPicture> lstProductPicture = new();
        public int postType { get; set; }
        public bool chkTopProductCategory { get; set; } = false;
        public bool chkTopProductCategoryParent { get; set; } = false;
        public IReadOnlyList<IBrowserFile> MainImages { get; set; }
        string outMessage = "";
        private bool isCropMainImage { get; set; }

        // setup upload endpoints
        public string SaveUrl => ToAbsoluteUrl("api/upload/save");

        public string RemoveUrl => ToAbsoluteUrl("api/upload/remove");

        ////List FileAttach Add new
        private List<ProductAttachFile> lstAttachFile { get; set; } = new List<ProductAttachFile>();

        ////List FileAttach binding
        private List<ProductAttachFile> lstAttachFileBinding { get; set; } = new List<ProductAttachFile>();

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
            if (!Repository.Permission.CanAddNewProduct(globalModel.user, globalModel.userId, ref outMessage))
            {
                toastService.ShowError(outMessage, "Thông báo");
                return;
            }
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
            //ProductCategory
            //var lstProductCate = await Repository.ProductCategory.GetProductCategoryById(null);
            var lstProductCate = await Repository.ProductCategory.ProductCategoryTreeGetLst();
            if (lstProductCate != null)
            {
                //lstProductCategory = lstProductCate.Select(x => new ProductCategory { Id = x.Id, Name = x.Name }).ToList();
                lstProductCategory = lstProductCate.Select(x => new SpProductCategoryTreeResult { Id = x.Id, Name = x.Name, HaveChild = x.HaveChild, ParentId = x.ParentId }).ToList();
            }
            //ProductType
            lstProductType = await Repository.Product.ProductTypeGetLst();
            if (lstProductType != null)
            {
                lstProductType = lstProductType.Select(x => new ProductType { Id = x.Id, Name = x.Name }).ToList();
            }
            //Product Manyfactures
            lstProductManufactures = await Repository.Product.ProductManufacturesGetLst();
            if (lstProductManufactures != null)
            {
                lstProductManufactures = lstProductManufactures.Select(x => new ProductManufacture { Id = x.Id, Name = x.Name }).ToList();
            }
            //Country
            lstCountry = await Repository.MasterData.CountriesGetLst();
            if (lstCountry != null)
            {
                lstCountry = lstCountry.Select(x => new Country { Id = x.Id, Name = x.Name }).ToList();
            }
            //Lst Unit
            lstUnit = await Repository.MasterData.UnitGetLst();
            if (lstUnit != null)
            {
                lstUnit = lstUnit.Select(x => new Unit { Id = x.Id, Name = x.Name }).ToList();
            }
        }

        protected async Task InitData()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
            if (queryStrings.TryGetValue("productId", out var _productId))
            {
                this.productId = Convert.ToInt32(_productId);
            }

            if (productId != null)
            {
                var result = await Repository.Product.ProductGetById((int)productId);
                if (result != null)
                {
                    product = Mapper.Map<ProductDTO>(result);
                    //Get Lst ProductCategory
                    var lstProdCate = await Repository.ProductCategory.GetLstProductCatebyProductId((int)product.Id);
                    SelectedCateValue = lstProdCate.Select(x => x.ProductCategoryId).FirstOrDefault();
                    lstProductPicture = await Repository.ProductPicture.ProductPictureGetLstByProductId((int)product.Id);
                }
                //L
                lstAttachFileBinding = await Repository.Product.ProductAttachGetLstByProductId((int)productId);
            }
        }

        #endregion Init

        #region Event
        private void OnProductSelected()
        {
            //SelectedCateName = lstProductCategory.Where(p => SelectedCateValue.Contains(p.Id)).Select(p => p.Name).ToList();
            //product.ProductCategoryIds = String.Join(",", SelectedCateValue.ToArray());
            product.ProductCategoryIds = SelectedCateValue.ToString();
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            product.Image = null;
            var imageFiles = e.GetMultipleFiles();
            MainImages = imageFiles;
            var format = "image/png";
            foreach (var item in imageFiles)
            {
                var resizedImageFile = await item.RequestImageFileAsync(format, 150, 150);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                //Clear Old Image
                lstMainImages.Clear();
                lstMainImages.Add(imageDataUrl);
            }
        }

        private async Task PostProduct()
        {
            if (postType == 1)
            {
                ProductStatusId = 1;
            }
            //            
            SelectedCateValue = Int32.Parse(product.ProductCategoryIds);
            List<int> prodCateId = new();
            prodCateId.Add(SelectedCateValue);
            //Create new
            if (product.Id == null || product.Id == 0)
            {
                //Check permission
                if (!Repository.Permission.CanAddNewProduct(globalModel.user, globalModel.userId, ref outMessage))
                {
                    if (!Repository.Permission.CanAddNewProduct(globalModel.user, globalModel.userId, ref outMessage))
                    {
                        toastService.ShowError(outMessage, "Thông báo");
                        return;
                    }
                }
                product.EndDate = product.StartDate?.AddYears(100);
                product.ProductBrandId = globalModel.productBrandId;
                int ProdCateID = Convert.ToInt32(product.ProductCategoryIds);

                product.Id = await Repository.Product.ProductInsert(Mapper.Map<CMS.Data.ModelEntity.Product>(product), globalModel.userId, ProductStatusId, prodCateId);
                //get product for values update
                product = Mapper.Map<ProductDTO>(await Repository.Product.ProductGetById((int)product.Id));
            }
     

            //Update
            if (product.Id != null && product.Id > 0)
            {
                //Check permission
                if (!Repository.Permission.CanEditProduct(globalModel.user, globalModel.userId, (int)product.Id, ref outMessage))
                {
                    toastService.ShowError(outMessage, "Thông báo");
                    return;
                }
                try
                {
                    //get product for values update
                    //product = Mapper.Map<ProductDTO>(await Repository.Product.ProductGetById((int)product.Id));
                    //Save Main Image
                    if (lstMainImages != null && lstMainImages.Count > 0)
                    {
                        product.Image = await SaveMainImage((int)product.Id, lstMainImages);
                    }
                    //Save Sub Image
                    if (lstSubImageData != null && lstSubImageData.Count > 0)
                    {
                        List<ProductPicture> lstProPic = await SaveSubImage((int)product.Id, globalModel.userId);
                        var result = await Repository.ProductPicture.ProductPictureInsert(lstProPic, globalModel.userId, (int)product.Id);
                        if (!result)
                        {
                            toastService.ShowError("Có lỗi trong quá trình cập nhật ảnh phụ", "Thông báo");
                        }
                    }
                    //change Content
                    if (product.Content != null && Utils.CheckContentHasBase64(product.Content))
                    {
                        product.Content = Utils.UploadImgBase64Content(product.Url, _env.WebRootPath, $"data/product/upload/{globalModel.userId}/{DateTime.Now:yyyy-MM-dd}", product.Content);
                    }
                    //Save Upload File
                    if (lstAttachFile.Count > 0)
                    {
                        lstAttachFile.ForEach(x =>
                        {
                            x.ProductId = product.Id;
                            x.CreateDate = DateTime.Now;
                            x.LastEditDate = DateTime.Now;
                            x.CreateBy = globalModel.userId;
                            x.LastEditBy = globalModel.userId;
                        });
                        var uploadResult = await Repository.Product.ProductAttachInsert(lstAttachFile);
                        if (!uploadResult)
                        {
                            Logger.LogError("Upload File Error");
                        }
                    }

                    await Repository.Product.ProductUpdate(Mapper.Map<CMS.Data.ModelEntity.Product>(product), globalModel.userId, ProductStatusId, prodCateId);


                    //ToastMessage
                    toastService.ShowToast(ToastLevel.Success, "Cập nhật sản phẩm thành công", "Thành công");
                    //Noti for globalModel.user
                    await hubConnection.SendAsync("SendNotification", globalModel.userId, "Gửi Sản phẩm thành công", $"Sản phẩm <b>{product.Name}</b> đang chờ xét duyệt", $"/Admin/Product/Preview?productId={product.Id}", product.Image);
                    //Noti for sectary
                    AccountSearchFilter modelfilter = new()
                    {
                        RoleId = Guid.Parse("6df4162d-38a4-42e9-b3d3-a07a5c29215b"), // phụ trách chuyên mục
                        PageSize = 100,
                        CurrentPage = 1,
                        Active = true
                    };
                    var lstProfielSec = await Repository.AspNetUsers.GetLstUsersPaging(modelfilter);
                    if (lstProfielSec != null && lstProfielSec.Items.Count > 0)
                    {
                        foreach (var p in lstProfielSec.Items)
                        {
                            if (p.Id == null || product == null || globalModel.user == null) continue;
                            await hubConnection.SendAsync("SendNotification", p.Id, "Sản phẩm mới gửi", $"Sản phẩm {product.Name} đã được {globalModel.user.Identity.Name} gửi tới tòa soạn chờ sơ duyệt", $"/Admin/Product/Preview?productId={product.Id}", product.Image);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ToastMessage
                    toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình cập nhật {ex}", "Lỗi");
                }
            }
            NavigationManager.NavigateTo("/Admin/Product");
        }

        //Config Editor
        public List<IEditorTool> Tools { get; set; } = new List<IEditorTool>()
       {
            new EditorButtonGroup(new Bold(), new Italic(), new Underline()),
            new EditorButtonGroup(new AlignLeft(), new AlignCenter(), new AlignRight()),
            new UnorderedList(),
            new EditorButtonGroup(new CreateLink(), new Unlink(), new InsertImage()),
            new InsertTable(),
            new EditorButtonGroup(new AddRowBefore(), new AddRowAfter(), new MergeCells(), new SplitCell()),
            new Format(),
            new FontSize(),
            new FontFamily(),
            new CustomTool("ImportImage")
       };

        private async Task InsertSubImage(InputFileChangeEventArgs e)
        {
            if (e.FileCount > 10)
            {
                toastService.ShowError("Chỉ có thể tối đa 10 ảnh", "Thông báo");
                return;
            }
            var imageFiles = e.GetMultipleFiles();
            if (imageFiles.Count + lstProductPicture.Count + lstSubImageData.Count > 10)
            {
                toastService.ShowError("Chỉ có thể tối đa 10 ảnh", "Thông báo");
                return;
            }
            lstSubImagesUpload = imageFiles;
            foreach (var item in imageFiles)
            {
                var format = item.ContentType;
                var resizedImageFile = await item.RequestImageFileAsync(format, 150, 150);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                lstSubImageData.Add($"data:{format};base64,{Convert.ToBase64String(buffer)}");

            }
        }

        //Save MainImage
        protected async Task<string> SaveMainImage(int ProductId, List<string> lstMainImages)
        {
            string fileName = "noimages.png";
            string fileNameWithText = "noimages.png";
            foreach (var file in lstMainImages)
            {
                var imageDataByteArray = Convert.FromBase64String(CMS.Common.Utils.GetBase64Image(file));

                var urlProduct = await Repository.Product.CreateProductURL(ProductId);
                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                fileName = String.Format("{0}-{1}.{2}", urlProduct, timestamp, "webp");
                fileNameWithText = String.Format("{0}-{1}-wt.{2}", urlProduct, timestamp, "webp");
                var physicalPath = Path.Combine(_env.WebRootPath, "data/product/mainimages/original");
                ImageCodecInfo jpgEncoder = CMS.Common.Utils.GetEncoder(ImageFormat.Jpeg);

                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                EncoderParameters myEncoderParameters = new(1);

                EncoderParameter myEncoderParameter = new(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                using (MemoryStream ms = new(imageDataByteArray))
                {
                  
                    using Bitmap bm2 = new(ms);
                    bm2.Save(Path.Combine(physicalPath, fileName), jpgEncoder, myEncoderParameters);                    
                }
                string imageFilePath = Path.Combine(_env.WebRootPath, "data/product/mainimages/original", fileName);
                Bitmap bitmap = (Bitmap)Image.FromFile(imageFilePath);
                StringFormat stringFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                PointF firstLocation = new PointF(10f, 10f);
                PointF secondLocation = new PointF(10f, 50f);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font("Sans-Serif", 30))
                    {
                        graphics.DrawString("0987.019.192", arialFont, Brushes.White, firstLocation);
                        graphics.DrawString("Đá mỹ nghệ Hòa Phát", arialFont, Brushes.Red, secondLocation);
                    }
                }
                bitmap.Save(Path.Combine(_env.WebRootPath, "data/product/mainimages/original", fileNameWithText));                
                try
                {
                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/product/mainimages/original", fileNameWithText), Path.Combine(_env.WebRootPath, "data/product/mainimages/small", fileNameWithText), 500, 500);
                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/product/mainimages/original", fileNameWithText), Path.Combine(_env.WebRootPath, "data/product/mainimages/thumb", fileNameWithText), 120, 120);
                }
                catch
                {
                }
            }
            return fileNameWithText;
        }
        //Save Sub
        protected async Task<List<ProductPicture>> SaveSubImage(int ProductId, string userId)
        {
            List<ProductPicture> lstProPic = new();

            int endSub = 1;
            foreach (var file in lstSubImagesUpload)
            {
                ProductPicture item = new();
                var urlProduct = await Repository.Product.CreateProductURL(ProductId);
                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                string fileName = String.Format("{0}-{1}-{2}.{3}", urlProduct, endSub, timestamp, "webp");
                var physicalPath = Path.Combine(_env.WebRootPath, "data/productpicture/mainimages/original", fileName);
                FileStream fs = new(physicalPath, FileMode.Create);
                await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                await fs.DisposeAsync(); await fs.DisposeAsync(); await fs.DisposeAsync();
                try
                {
                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/productpicture/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/productpicture/mainimages/small", fileName), 500, 500);
                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/productpicture/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/productpicture/mainimages/thumb", fileName), 120, 120);
                }
                catch
                {
                }
                item.Image = fileName;
                item.ProductId = productId;
                item.Sort = endSub;
                item.CreateBy = userId;
                item.CreateDate = DateTime.Now;
                item.LastEditBy = userId;
                item.LastEditDate = DateTime.Now;
                lstProPic.Add(item);
                //Increase 
                endSub++;

            }
            return lstProPic;
        }
        public string ToAbsoluteUrl(string url)
        {
            return $"{NavigationManager.BaseUri}{url}";
        }

        public void OnSuccess(UploadSuccessEventArgs args)
        {
            foreach (var file in args.Files)
            {
                ProductAttachFile item = new()
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

        private async Task DeleteAttachFile(int productAttachFileId)
        {
            await Repository.Product.ProductAttachDelete(productAttachFileId);
            StateHasChanged();
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
                        product.Image = null;
                        lstMainImages.Clear();
                        lstMainImages.Add(imageCropperModal.ImgData);
                    }
                    else
                    {
                        product.Content = product.Content + Environment.NewLine + $"<img src=\"{imageCropperModal.ImgData}\"/>" + Environment.NewLine;
                    }

                    StateHasChanged();
                }
            }
        }

        private async Task OnDeleteSubImage(bool isExistsDB, string imgSub, ProductPicture item)
        {
            if (!isExistsDB) // file base 64
            {
                lstSubImageData.Remove(imgSub);
                StateHasChanged();
            }
            else
            {
                var result = await Repository.ProductPicture.ProductPictureDeleteById(item.Id);
                if (result)
                {
                    lstProductPicture.Remove(item);
                    StateHasChanged();
                }
            }
        }
        #endregion Event
    }
}
