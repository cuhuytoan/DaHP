using CMS.Common;
using Microsoft.AspNetCore.Components.Forms;
using System.Drawing;

namespace CMS.Website.Areas.Member.Pages.Account
{
    public partial class EditProfile : IDisposable
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

        public AspNetUserInfoDTO userInfo { get; set; } = new();
        private List<KeyValuePair<bool, string>> lstGender { get; set; }

        private List<string> imageDataUrls = new();
        private IReadOnlyList<IBrowserFile> MainImages;
        public DateTime MaxDate = new(2021, 12, 31);
        public DateTime MinDate = new(1950, 1, 1);

        private List<Location> lstLocation { get; set; } = new();
        private List<District> lstDistrict { get; set; } = new();
        private List<Ward> lstWard { get; set; } = new();
        private List<Department> lstDepartment { get; set; } = new();
        private List<Bank> lstBank { get; set; } = new();

        private List<ProductBrand> lstProductBrand { get; set; } = new();
        private bool IsEmail { get; set; } = false;

        private bool IsPhoneNumber { get; set; } = false;

        [CascadingParameter]
        private GlobalModel globalModel { get; set; }

        //Modal Crop Image
        protected ImageCropper imageCropperModal { get; set; }

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
            //Binding lstGender
            List<KeyValuePair<bool, string>> lstGenderAdd = new();
            lstGenderAdd.Add(new KeyValuePair<bool, string>(true, "Nam"));
            lstGenderAdd.Add(new KeyValuePair<bool, string>(false, "Nữ"));
            lstGender = lstGenderAdd.ToList();

            lstLocation = await Repository.MasterData.LocationGetLstByCountryId(1);
            lstLocation = lstLocation.Select(x => new Location { Id = x.Id, Name = x.Name }).ToList();

            lstDepartment = await Repository.MasterData.DepartmentsGetLst();
            lstDepartment = lstDepartment.Select(x => new Department { Id = x.Id, Name = x.Name }).ToList();

            lstBank = await Repository.MasterData.BankGetLst();
            lstBank = lstBank.Select(x => new Bank { Id = x.Id, Name = x.Name }).ToList();

            lstProductBrand = await Repository.ProductBrand.ProductBrandGetLst();
            lstProductBrand = lstProductBrand.Select(x => new ProductBrand { Id = x.Id, Name = x.Name }).ToList();

        }

        protected async Task InitData()
        {
            var resultUser = await Repository.AspNetUsers.AspNetUsersGetById(globalModel.userId);
            if (resultUser != null)
            {
                userInfo.AspNetUsers = Mapper.Map<AspNetUsersDTO>(resultUser);
            }
            var result = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(globalModel.userId);
            if (result != null)
            {
                userInfo.AspNetUserProfiles = Mapper.Map<AspNetUserProfilesDTO>(result);
            }
            if (userInfo.AspNetUserProfiles.LocationId > 0)
            {
                lstDistrict = userInfo.AspNetUserProfiles.LocationId == null ? new() : await Repository.MasterData.DistrictsGetLstByLocationId((int)userInfo.AspNetUserProfiles.LocationId);
                lstDistrict = lstDistrict.Select(x => new District { Id = x.Id, Name = x.Name }).ToList();
            }
            if (userInfo.AspNetUserProfiles.DistrictId > 0)
            {
                lstWard = userInfo.AspNetUserProfiles.DistrictId == null ? new() : await Repository.MasterData.WardsGetLstByDistrictId((int)userInfo.AspNetUserProfiles.DistrictId);
                lstWard = lstWard.Select(x => new Ward { Id = x.Id, Name = x.Name }).ToList();
            }
            //Check account Email Or Phone
            if (CMS.Common.Utils.IsEmail(resultUser.UserName))
            {
                IsEmail = true;
                IsPhoneNumber = false;
            }
            if (CMS.Common.Utils.IsPhoneNumber(resultUser.UserName))
            {
                IsEmail = false;
                IsPhoneNumber = true;
            }
        }

        #endregion Init

        #region Event


        private async Task PostUserInfo()
        {
            var profileExists = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(globalModel.userId);
            if (profileExists != null)
            {
                try
                {
                    var userExists = await Repository.AspNetUsers.AspNetUsersGetById(globalModel.userId);
                    var user = await UserManager.FindByIdAsync(userExists.Id);
                    if (user != null)
                    {
                        if (!IsEmail)
                        {
                            if (!CMS.Common.Utils.IsEmail(userInfo.AspNetUsers.Email))
                            {
                                toastService.ShowError("Email không hợp lệ", "Thông báo");
                                return;
                            }
                            //Check Email & Phone Number
                            var emailExists = await UserManager.FindByEmailAsync(userInfo.AspNetUsers.Email);
                            if (emailExists != null && userExists.Email != userInfo.AspNetUsers.Email)
                            {
                                toastService.ShowToast(ToastLevel.Error, $"Email : {userInfo.AspNetUsers.Email} đã tồn tại trên hệ thống", "Lỗi");
                                return;
                            }
                            else
                            {
                                //Update AspnetUser                                
                                user.Email = userInfo.AspNetUsers.Email;
                                user.EmailConfirmed = true;
                                await UserManager.UpdateAsync(user);
                                //Update Profiles
                                profileExists.Email = userInfo.AspNetUsers.Email;
                                await Repository.AspNetUsers.AspNetUserProfilesUpdate(profileExists);
                                //Verify
                                //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                                //await UserManager.ConfirmEmailAsync(user, code);
                            }



                        }
                        if (!IsPhoneNumber)
                        {
                            if (!CMS.Common.Utils.IsPhoneNumber(userInfo.AspNetUsers.PhoneNumber))
                            {
                                toastService.ShowError("Số điện thoại không hợp lệ", "Thông báo");
                                return;
                            }
                            var phoneNumberExists = await Repository.AspNetUsers.AspNetUsersGetByPhoneNumber(userInfo.AspNetUsers.PhoneNumber);
                            if (phoneNumberExists != null && userExists.PhoneNumber != userInfo.AspNetUsers.PhoneNumber)
                            {
                                toastService.ShowToast(ToastLevel.Error, $"Số điện thoại : {userInfo.AspNetUsers.PhoneNumber} đã tồn tại trên hệ thống", "Lỗi");
                                return;
                            }
                            else
                            {
                                //Update AspnetUser
                                user.PhoneNumber = userInfo.AspNetUsers.PhoneNumber;
                                user.PhoneNumberConfirmed = true;
                                await UserManager.UpdateAsync(user);
                                //Update Profiles
                                profileExists.Phone = userInfo.AspNetUsers.PhoneNumber;
                                await Repository.AspNetUsers.AspNetUserProfilesUpdate(profileExists);

                                //var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user, userInfo.AspNetUsers.PhoneNumber);
                                //await UserManager.ChangePhoneNumberAsync(user, userInfo.AspNetUsers.PhoneNumber, code);
                            }


                        }
                    }
                    //Save Main Image
                    if (imageDataUrls != null && imageDataUrls.Count > 0)
                    {
                        userInfo.AspNetUserProfiles.AvatarUrl = await SaveMainImage((int)profileExists.Id, imageDataUrls);
                    }

                    await Repository.AspNetUsers.AspNetUserProfilesUpdate(
                      Mapper.Map<AspNetUserProfiles>(userInfo.AspNetUserProfiles));

                    //ToastMessage
                    toastService.ShowToast(ToastLevel.Success, "Cập nhật user thành công", "Thành công");
                }
                catch
                {
                    //ToastMessage
                    toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình cập nhật", "Lỗi");
                }
            }
            else
            {
                //ToastMessage
                toastService.ShowToast(ToastLevel.Error, $"Không tồn tại tài khoản cập nhật", "Lỗi");
            }
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            userInfo.AspNetUserProfiles.AvatarUrl = null;
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
                imageDataUrls.Clear();
                imageDataUrls.Add(imageDataUrl);
            }
        }

        //Save MainImage
        protected async Task<string> SaveMainImage(int UserProfileId, List<string> imageDataUrls)
        {
            string fileName = "noimages.png";
            foreach (var file in imageDataUrls)
            {
                var imageDataByteArray = Convert.FromBase64String(CMS.Common.Utils.GetBase64Image(file));

                var profileImg = $"Profile_{UserProfileId}";
                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                fileName = String.Format("{0}-{1}.{2}", profileImg, timestamp, CMS.Common.Utils.GetBase64ImageMime(file));
                var physicalPath = Path.Combine(_env.WebRootPath, "data/user/mainimages/original");

                using (MemoryStream ms = new(imageDataByteArray))
                {
                    using Bitmap bm2 = new(ms);
                    bm2.Save(Path.Combine(physicalPath, fileName));
                }
                try
                {
                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/user/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/user/mainimages/small", fileName), 500, 500);
                    Utils.EditSize(Path.Combine(_env.WebRootPath, "data/user/mainimages/original", fileName), Path.Combine(_env.WebRootPath, "data/user/mainimages/thumb", fileName), 120, 120);
                }
                catch
                {
                }
            }
            return fileName;
        }

        private void OnCropImage()

        {
            imageCropperModal.Show();
        }

        protected void ConfirmImageCropper(bool isDone)
        {
            if (isDone)
            {
                if (imageCropperModal.ImgData != null)
                {
                    userInfo.AspNetUserProfiles.AvatarUrl = null;
                    imageDataUrls.Clear();
                    imageDataUrls.Add(imageCropperModal.ImgData);
                    StateHasChanged();
                }
            }
        }

        private async void LocationSelected()
        {
            lstDistrict = userInfo.AspNetUserProfiles.LocationId == null ? new() : await Repository.MasterData.DistrictsGetLstByLocationId((int)userInfo.AspNetUserProfiles.LocationId);
            lstDistrict = lstDistrict.Select(x => new District { Id = x.Id, Name = x.Name }).ToList();
            StateHasChanged();
        }

        private async void DistrictSelected()
        {
            lstWard = userInfo.AspNetUserProfiles.DistrictId == null ? new() : await Repository.MasterData.WardsGetLstByDistrictId((int)userInfo.AspNetUserProfiles.DistrictId);
            lstWard = lstWard.Select(x => new Ward { Id = x.Id, Name = x.Name }).ToList();
            StateHasChanged();
        }

        #endregion Event
    }
}