namespace CMS.Website.Areas.Admin.Pages.Employee
{
    public partial class Edit : IDisposable
    {
        #region Inject   
        [Inject]
        IMapper Mapper { get; set; }
        [Inject]
        ILoggerManager Logger { get; set; }
        [Inject]
        UserManager<IdentityUser> UserManager { get; set; }
        #endregion

        #region Parameter
        [Parameter]
        [SupplyParameterFromQuery]
        public string userId { get; set; }
        public AspNetUserInfoDTO userInfo { get; set; } = new();

        List<AspNetRoles> lstRole { get; set; }
        List<KeyValuePair<bool, string>> lstGender { get; set; }
        public DateTime MaxDate = new(2020, 12, 31);
        public DateTime MinDate = new(1950, 1, 1);
        private List<Location> lstLocation { get; set; } = new();
        private List<District> lstDistrict { get; set; } = new();
        private List<Ward> lstWard { get; set; } = new();
        private List<Department> lstDepartment { get; set; } = new();
        private List<Bank> lstBank { get; set; } = new();
        private List<CMS.Data.ModelEntity.ProductBrand> lstProductBrand { get; set; } = new();

        private bool enableAssignCategory { get; set; }

        private bool enableAssignProductCategory { get; set; }

        private bool IsEmail { get; set; } = false;

        private bool IsPhoneNumber { get; set; } = false;

        public List<int> SelectedCateValue { get; set; } = new List<int>();
        public List<string> SelectedCateName { get; set; } = new List<string>();

        public List<int> SelectedProductCateValue { get; set; } = new List<int>();
        public List<string> SelectedProductCateName { get; set; } = new List<string>();
        private List<ArticleCategory> lstArticleCategory { get; set; } = new();
        private List<ProductCategory> lstProductCategory { get; set; } = new();


        [CascadingParameter]
        private GlobalModel globalModel { get; set; }


        #endregion

        #region LifeCycle

        protected override async Task OnInitializedAsync()
        {

            await InitControl();
            await InitData();


        }

        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
        #endregion

        #region Init
        protected async Task InitControl()
        {
            //Binding lstRole
            //Binding Category
            var ListRoles = await Repository.AspNetUsers.AspNetRolesGetAll();
            if (ListRoles != null)
            {
                lstRole = ListRoles.Select(x => new AspNetRoles { Id = x.Id.ToString(), Name = x.Name }).ToList();
            }
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
            lstProductBrand = lstProductBrand.Select(x => new CMS.Data.ModelEntity.ProductBrand { Id = x.Id, Name = x.Name }).ToList();

            var lstArticleCate = await Repository.ArticleCategory.GetArticleCategoryById(null);
            if (lstArticleCate != null)
            {
                lstArticleCategory = lstArticleCate.Select(x => new ArticleCategory { Id = x.Id, Name = x.Name }).ToList();
            }
            var lstProductCate = await Repository.ProductCategory.GetProductCategoryById(null);
            if (lstProductCate != null)
            {
                lstProductCategory = lstProductCate.Select(x => new ProductCategory { Id = x.Id, Name = x.Name }).ToList();
            }


        }
        protected async Task InitData()
        {
            if (userId != null)
            {
                var result = await Repository.AspNetUsers.AspNetUsersGetById(userId);
                if (result != null)
                {
                    var profile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(userId);
                    var roles = await Repository.AspNetUsers.AspNetUserRolesGetByUserId(userId);
                    var lstcategoryAssign = await Repository.ArticleCategory.ArticleCategoryAssignsGetLstByUserId(userId);
                    userInfo.AspNetUsers = Mapper.Map<AspNetUsersDTO>(result);
                    userInfo.AspNetUserRoles = Mapper.Map<AspNetUserRolesDTO>(roles);
                    userInfo.AspNetUserProfiles = Mapper.Map<AspNetUserProfilesDTO>(profile);
                    userInfo.LstArtCatAssign = lstcategoryAssign;

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
                    if (userInfo.AspNetUserRoles.RoleId == "6df4162d-38a4-42e9-b3d3-a07a5c29215b")
                    {
                        //Get Lst ArticleCategory
                        var lstArtCateAssign = await Repository.ArticleCategory.ArticleCategoryAssignsGetLstByUserId(userId);
                        SelectedCateValue = lstArtCateAssign.Where(x => x.ArticleCategoryId != null).Select(x => x.ArticleCategoryId.Value).ToList();
                        enableAssignCategory = true;
                    }
                    if (userInfo.AspNetUserRoles.RoleId == "6FB4167E-7583-4437-A8E6-6A20BBF5DAEA")
                    {
                        //Get Lst ProductCategory
                        var lstProdCateAssign = await Repository.ProductCategory.ProductCategoryAssignsGetLstByUserId(userId);
                        SelectedProductCateValue = lstProdCateAssign.Where(x => x.ProductCategoryId != null).Select(x => x.ProductCategoryId.Value).ToList();
                        enableAssignProductCategory = true;
                    }
                }
                //Check account Email Or Phone
                if (CMS.Common.Utils.IsEmail(result.UserName))
                {
                    IsEmail = true;
                    IsPhoneNumber = false;
                }
                if (CMS.Common.Utils.IsPhoneNumber(result.UserName))
                {
                    IsEmail = false;
                    IsPhoneNumber = true;
                }

            }
        }
        #endregion

        #region Event

        async Task PostUserInfo()
        {
            if (userId == null) // Add new
            {
                if (!CMS.Common.Utils.IsPhoneNumber(userInfo.AspNetUsers.UserName) && !CMS.Common.Utils.IsEmail(userInfo.AspNetUsers.UserName))
                {
                    toastService.ShowError($"Số điện thoại hoặc email  {userInfo.AspNetUsers.UserName} không hợp lệ", "Thông báo");
                    return;
                }
                else
                {
                    var userExists = Repository.AspNetUsers.FirstOrDefault(p => p.UserName == userInfo.AspNetUsers.Email || p.UserName == userInfo.AspNetUsers.PhoneNumber || p.UserName == userInfo.AspNetUsers.UserName);
                    if (userExists != null)
                    {
                        toastService.ShowError($"Tài khoản {userInfo.AspNetUsers.UserName} đã tồn tại", "Thông báo");
                        return;
                    }
                    var user = new IdentityUser { UserName = userInfo.AspNetUsers.UserName };
                    try
                    {
                        var result = await UserManager.CreateAsync(user, userInfo.AspNetUsers.Password);
                        if (result.Succeeded)
                        {
                            //Insert new Profilers
                            userInfo.AspNetUserProfiles.UserId = user.Id;
                            userInfo.AspNetUserProfiles.RegType = IsEmail ? "Email" : "Phone";
                            userInfo.AspNetUserProfiles.ProductBrandId = globalModel.productBrandId;
                            await Repository.AspNetUsers.AspNetUserProfilesCreateNew(
                                Mapper.Map<AspNetUserProfiles>(userInfo.AspNetUserProfiles));
                            //Insert new Role
                            //userInfo.AspNetUserRoles.RoleId = RoleId;
                            userInfo.AspNetUserRoles.UserId = user.Id;
                            await Repository.AspNetUsers.AspNetUserRolesCreateNew(
                                Mapper.Map<AspNetUserRoles>(userInfo.AspNetUserRoles));

                            if (userInfo.AspNetUserRoles.RoleId == "6df4162d-38a4-42e9-b3d3-a07a5c29215b")
                            {
                                await Repository.ArticleCategory.ArticleCategoryAssignsUpdate(userExists.Id, SelectedCateValue);
                            }

                            //ToastMessage
                            toastService.ShowToast(ToastLevel.Success, "Cập nhật user thành công", "Thành công");
                            NavigationManager.NavigateTo("/Shopman/Employee/Index");
                        }
                    }
                    catch
                    {
                        //ToastMessage
                        toastService.ShowToast(ToastLevel.Error, $"Có lỗi trong quá trình cập nhật", "Lỗi");
                    }
                }
            }
            else // Update
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user == null)
                {
                    toastService.ShowError($"Tài khoản không hợp lệ", "Thông báo");
                    return;
                }
                else
                {
                    if (user != null)
                    {
                        if (!IsEmail)
                        {
                            //Update AspnetUser                                
                            user.Email = userInfo.AspNetUsers.Email;
                            user.EmailConfirmed = true;
                            await UserManager.UpdateAsync(user);

                        }
                        if (!IsPhoneNumber)
                        {
                            //Update AspnetUser                                
                            user.PhoneNumber = userInfo.AspNetUsers.PhoneNumber;
                            user.PhoneNumberConfirmed = true;
                            await UserManager.UpdateAsync(user);
                        }

                    }


                    await Repository.AspNetUsers.AspNetUserProfilesUpdate(
                      Mapper.Map<AspNetUserProfiles>(userInfo.AspNetUserProfiles));

                    await Repository.AspNetUsers.AspNetUserRolesUpdate(
                        Mapper.Map<AspNetUserRoles>(userInfo.AspNetUserRoles));

                    if (userInfo.AspNetUserRoles.RoleId == "6df4162d-38a4-42e9-b3d3-a07a5c29215b")
                    {
                        await Repository.ArticleCategory.ArticleCategoryAssignsUpdate(userId, SelectedCateValue);
                    }

                    //ToastMessage
                    toastService.ShowToast(ToastLevel.Success, "Cập nhật tài khoản thành công", "Thành công");
                    NavigationManager.NavigateTo("/Shopman/Employee/Index");
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

        private void OnChangedRole()
        {

            if (userInfo.AspNetUserRoles.RoleId == "6df4162d-38a4-42e9-b3d3-a07a5c29215b")
            {
                enableAssignCategory = true;
                enableAssignProductCategory = false;
            }
            else if (userInfo.AspNetUserRoles.RoleId == "6FB4167E-7583-4437-A8E6-6A20BBF5DAEA")
            {
                enableAssignCategory = false;
                enableAssignProductCategory = true;
            }
            else
            {
                enableAssignCategory = false;
                enableAssignProductCategory = false;
            }
            StateHasChanged();
        }

        private void OnArticleSelected()
        {
            SelectedCateName = lstArticleCategory.Where(p => SelectedCateValue.Contains(p.Id)).Select(p => p.Name).ToList();
        }
        private void OnProductSelected()
        {
            SelectedProductCateName = lstArticleCategory.Where(p => SelectedCateValue.Contains(p.Id)).Select(p => p.Name).ToList();
        }
        protected Task OnBlurAccount()
        {

            //Check account Email Or Phone
            if (CMS.Common.Utils.IsEmail(userInfo.AspNetUsers.UserName))
            {
                IsEmail = true;
                IsPhoneNumber = false;
            }
            if (CMS.Common.Utils.IsPhoneNumber(userInfo.AspNetUsers.UserName))
            {
                IsEmail = false;
                IsPhoneNumber = true;
            }
            return Task.CompletedTask;
        }

        #endregion
    }
}
