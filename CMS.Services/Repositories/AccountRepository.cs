namespace CMS.Services.Repositories
{
    public interface IAccountRepository : IRepositoryBase<AspNetUsers>
    {
        Task<IEnumerable<AspNetRoles>> AspNetRolesGetAll();

        Task<List<AspNetRoles>> AspNetRolesGetByGroup(string groupName);

        Task AspNetUsersDelete(string UserId);

        Task<AspNetUsers> AspNetUsersGetById(string UserId);

        Task<AspNetUsers> AspNetUsersGetByPhoneNumber(string PhoneNumber);

        Task AspNetUserRolesCreateNew(AspNetUserRoles model);

        Task AspNetUserRolesUpdate(AspNetUserRoles model);

        Task AspNetUserRolesDelete(string UserId);

        Task<AspNetUserRoles> AspNetUserRolesGetByUserId(string UserId);

        Task AspNetUserProfilesCreateNew(AspNetUserProfiles model);

        Task AspNetUserProfilesUpdate(AspNetUserProfiles model);

        Task AspNetUserProfilesDeleteByUserId(string UserId);

        Task<AspNetUserProfiles> AspNetUserProfilesGetByUserId(string UserId);

        Task<AspNetUserInfo> GetAccountInfoByUserId(string UserId);

        Task<VirtualizeResponse<SpAccountSearchResult>> GetLstUsersPaging(AccountSearchFilter model);

        Task<bool> SendSMSVerify(SMSVerify model);


    }

    public class AccountRepository : RepositoryBase<AspNetUsers>, IAccountRepository
    {
        public AccountRepository(CmsContext CmsContext) : base(CmsContext)
        {
        }

        // Quản lý nhóm quyền
        public async Task<IEnumerable<AspNetRoles>> AspNetRolesGetAll()
        {
            return await CmsContext.AspNetRoles.ToListAsync();
        }

        // Quản lý người dùng
        public async Task<AspNetUsers> AspNetUsersGetById(string UserId)
        {
            return await CmsContext.AspNetUsers.FirstOrDefaultAsync(p => p.Id == UserId);
        }

        public async Task AspNetUsersDelete(string UserId)
        {
            var item = await CmsContext.AspNetUsers.FindAsync(UserId);
            if (item != null)
            {
                CmsContext.AspNetUsers.Remove(item);
                await CmsContext.SaveChangesAsync();
            }
        }

        // QUnr lý người dùng thuộc nhóm quyền nào
        public async Task AspNetUserRolesCreateNew(AspNetUserRoles model)
        {
            try
            {
                CmsContext.AspNetUserRoles.Add(model);
                await CmsContext.SaveChangesAsync();
            }
            catch
            {
            }
        }

        public async Task AspNetUserRolesUpdate(AspNetUserRoles model)
        {
            await AspNetUserRolesDelete(model.UserId);

            CmsContext.AspNetUserRoles.Add(model);

            await CmsContext.SaveChangesAsync();
        }

        public async Task AspNetUserRolesDelete(string UserId)
        {
            var item = await CmsContext.AspNetUserRoles.Where(p => p.UserId == UserId).ToListAsync();
            if (item != null)
            {
                CmsContext.AspNetUserRoles.RemoveRange(item);
                await CmsContext.SaveChangesAsync();
            }
        }

        public async Task<AspNetUserRoles> AspNetUserRolesGetByUserId(string UserId)
        {
            return await CmsContext.AspNetUserRoles.FirstOrDefaultAsync(p => p.UserId == UserId);
        }

        // Quản lý thông tin người dùng
        public async Task AspNetUserProfilesCreateNew(AspNetUserProfiles model)
        {
            try
            {
                var userProfile = new AspNetUserProfiles()
                {

                    UserId = model.UserId,
                    FullName = model.FullName,
                    BirthDate = model.BirthDate,
                    Gender = model.Gender,
                    Address = model.Address,
                    ProductBrandId = model.ProductBrandId,
                    RegType = model.RegType,
                    RegisterDate = DateTime.Now,
                    VerifiedDate = DateTime.Now,
                    AllowNotifyApp = true
                };
                CmsContext.AspNetUserProfiles.Add(userProfile);
                await CmsContext.SaveChangesAsync();
            }
            catch
            {
            }
        }

        public async Task AspNetUserProfilesUpdate(AspNetUserProfiles model)
        {
            try
            {
                var existsProfilers = await CmsContext.AspNetUserProfiles.FindAsync(model.Id);
                if (existsProfilers != null)
                {
                    existsProfilers.FullName = model.FullName;
                    existsProfilers.BirthDate = model.BirthDate;
                    existsProfilers.Gender = model.Gender;
                    existsProfilers.Address = model.Address;
                    existsProfilers.AvatarUrl = model.AvatarUrl;
                    existsProfilers.BankAcc = model.BankAcc;
                    existsProfilers.Company = model.Company;
                    existsProfilers.CountryId = 1;
                    existsProfilers.LocationId = model.LocationId;
                    existsProfilers.DistrictId = model.DistrictId;
                    existsProfilers.WardId = model.WardId;
                    existsProfilers.BankId = model.BankId;
                    existsProfilers.DepartmentId = model.DepartmentId;
                    existsProfilers.ProductBrandId = model.ProductBrandId;
                    existsProfilers.Phone = model.Phone;
                    existsProfilers.AllowNotifyApp = model.AllowNotifyApp ?? false;
                    existsProfilers.AllowNotifyEmail = model.AllowNotifyEmail ?? false;
                    existsProfilers.AllowNotifySms = model.AllowNotifySms ?? false;
                    await CmsContext.SaveChangesAsync();
                }
            }
            catch
            {
            }
        }

        public async Task AspNetUserProfilesDeleteByUserId(string UserId)
        {
            var item = CmsContext.AspNetUserProfiles.FirstOrDefault(p => p.UserId == UserId);
            if (item != null)
            {
                CmsContext.AspNetUserProfiles.Remove(item);
                await CmsContext.SaveChangesAsync();
            }
        }

        public async Task<AspNetUserProfiles> AspNetUserProfilesGetByUserId(string UserId)
        {
            var profile = await CmsContext.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == UserId);
            return profile;
        }

        public async Task<AspNetUserInfo> GetAccountInfoByUserId(string UserId)
        {
            var output = new AspNetUserInfo();
            output.AspNetUsers = await AspNetUsersGetById(UserId);
            output.AspNetUserProfiles = await AspNetUserProfilesGetByUserId(UserId);
            output.AspNetUserRoles = await AspNetUserRolesGetByUserId(UserId);
            return output;
        }

        public async Task<VirtualizeResponse<SpAccountSearchResult>> GetLstUsersPaging(AccountSearchFilter model)
        {
            var output = new VirtualizeResponse<SpAccountSearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();
            var result = await CmsContext.GetProcedures().SpAccountSearchAsync(
            model.Keyword,
            model.RoleId,
            model.ProductBrandId,
            model.Active,
            model.PageSize,
            model.CurrentPage, itemCounts, returnValues
            );
            output.Items = result.ToList();
            output.TotalSize = (int)itemCounts.Value;

            return output;
        }

        public async Task<AspNetUsers> AspNetUsersGetByPhoneNumber(string PhoneNumber)
        {
            return await CmsContext.AspNetUsers.FirstOrDefaultAsync(x => x.PhoneNumber == PhoneNumber || x.UserName == PhoneNumber);
        }

        public async Task<bool> SendSMSVerify(SMSVerify model)
        {
            HttpClient client = new();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://canhnn01.ddns.net:88/apiv1/SMS/Send", stringContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<AspNetRoles>> AspNetRolesGetByGroup(string groupName)
        {
            return await CmsContext.AspNetRoles.Where(x => x.AspNetRoleGroup == groupName).ToListAsync();
        }
    }
}