using System.Security.Claims;

namespace CMS.Services.Repositories
{
    public interface IPermissionRepository : IRepositoryBase<AspNetUsers>
    {
        #region Interface Article
        //Article
        bool CanViewArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage);

        bool CanAddNewArticle(ClaimsPrincipal user, string userId, ref string messsage);

        bool CanEditArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage);

        bool CanDeleteArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage);

        bool CanCommentArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage);

        bool CanCommentStaffArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage);

        bool CanEditCommentStaffArticle(ClaimsPrincipal user, string userId, int commentId, ref string messsage);

        bool CanEditCommentArticle(ClaimsPrincipal user, string userId, int commentId, ref string messsage);

        bool CanEditProductbrand(ClaimsPrincipal user, string userId, int articleId, ref string messsage);
        #endregion

        #region Interface Product
        //Product

        bool CanViewProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage);

        bool CanAddNewProduct(ClaimsPrincipal user, string userId, ref string messsage);

        bool CanEditProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage);

        bool CanDeleteProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage);

        bool CanCommentProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage);

        bool CanCommentStaffProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage);

        bool CanEditCommentStaffProduct(ClaimsPrincipal user, string userId, int commentId, ref string messsage);

        bool CanEditCommentProduct(ClaimsPrincipal user, string userId, int commentId, ref string messsage);

        bool CanDeleteProductCategory(ClaimsPrincipal user, string userId, int productCategoryId, ref string messsage);
        #endregion


        #region Employee
        bool CanAddNewEmployee(ClaimsPrincipal user, string userId, ref string messsage);

        bool CanEditEmployee(ClaimsPrincipal user, string userId, string employeeId, ref string messsage);
        #endregion

    }

    public class PermissionRepository : RepositoryBase<AspNetUsers>, IPermissionRepository
    {
        public PermissionRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }
        #region Article
        public bool CanAddNewArticle(ClaimsPrincipal user, string userId, ref string messsage)
        {
            return true;
        }

        public bool CanCommentArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage)
        {
            if(!String.IsNullOrEmpty(userId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CanCommentStaffArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CanDeleteArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage)
        {
            var articleItem = CmsContext.Article.Find(articleId);
            if (articleItem != null)
            {
                if (user.IsInRole("Quản trị hệ thống") )
                {
                    return true;
                }

                if ((user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng")))
                {
                    var profile = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId);
                    if (profile != null)
                    {
                        if (articleItem.ProductBrandId == profile.ProductBrandId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            else
            {
                messsage = "Không tìm thấy bài viết";
                return false;
            }
            messsage = "Không có quyền xóa bài viết";
            return false;
        }

        public bool CanEditArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage)
        {

            var articleItem = CmsContext.Article.Find(articleId);
            if (articleItem != null)
            {
                if (user.IsInRole("Quản trị hệ thống") || user.IsInRole("Lãnh đạo tòa soạn") || user.IsInRole("Lãnh đạo tòa soạn"))
                {
                    return true;
                }
                if (user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
                {
                    var profile = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId);
                    if (profile != null)
                    {
                        if (articleItem.ProductBrandId == profile.ProductBrandId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }          
            }
            else
            {
                messsage = "Không tìm thấy bài viết";
                return false;
            }
            messsage = "Không có quyền chỉnh sửa bài viết";
            return false;
        }

        public bool CanEditCommentArticle(ClaimsPrincipal user, string userId, int commentId, ref string messsage)
        {
            var item = CmsContext.ArticleComment.Where(x => x.Id == commentId && x.CreateBy == userId);
            if (item != null)
            {
                return true;
            }
            return false;
        }

        public bool CanEditCommentStaffArticle(ClaimsPrincipal user, string userId, int commentId, ref string messsage)
        {
            var item = CmsContext.ArticleComment.Where(x => x.Id == commentId && x.CreateBy == userId);
            if (item != null)
            {
                return true;
            }
            return false;
        }

        public bool CanViewArticle(ClaimsPrincipal user, string userId, int articleId, ref string messsage)
        {
            var articleItem = CmsContext.Article.Find(articleId);
            if (articleItem != null)
            {
                if (user.IsInRole("Quản trị hệ thống"))
                {
                    return true;
                }
                if (user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
                {
                    var profile = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId);
                    if (profile != null)
                    {
                        if (articleItem.ProductBrandId == profile.ProductBrandId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            else
            {
                messsage = "Không tìm thấy bài viết";
                return false;
            }
            messsage = "Không có quyền xem bài viết";
            return false;
        }
        #endregion

        #region Product

        public bool CanAddNewProduct(ClaimsPrincipal user, string userId, ref string messsage)
        {
            if (user.IsInRole("Quản trị hệ thống"))
            {
                return true;
            }
            if (user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
            {
                return true;
            }
            return false;
        }

        public bool CanCommentProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage)
        {
            return true;
        }
        public bool CanCommentStaffProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage)
        {
            var productItem = CmsContext.Product.Find(productId);
            if (productItem != null)
            {
                if (user.IsInRole("Quản trị hệ thống") )
                {
                    return true;
                }

                if (user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
                {
                    var profile = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId);
                    if (profile != null)
                    {
                        if (productItem.ProductBrandId == profile.ProductBrandId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            else
            {
                messsage = "Không tìm thấy sản phẩm";
                return false;
            }
            messsage = "Không có quyền bình luận sản phẩm";
            return false;
        }
        public bool CanDeleteProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage)
        {
            var productItem = CmsContext.Product.Find(productId);
            if (productItem != null)
            {
                if (user.IsInRole("Quản trị hệ thống"))
                {
                    return true;
                }

                if ((user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng") ))
                {
                    var profile = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId);
                    if (profile != null)
                    {
                        if (productItem.ProductBrandId == profile.ProductBrandId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                                
            }
            else
            {
                messsage = "Không tìm thấy bài viết";
                return false;
            }
            messsage = "Không có quyền xóa bài viết";
            return false;
        }

        public bool CanEditProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage)
        {
            var productItem = CmsContext.Product.Find(productId);
            if (productItem != null)
            {
                if (user.IsInRole("Quản trị hệ thống") || user.IsInRole("Lãnh đạo tòa soạn") || user.IsInRole("Lãnh đạo tòa soạn"))
                {
                    return true;
                }
                if (user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
                {
                    var profile = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId);
                    if (profile != null)
                    {
                        if (productItem.ProductBrandId == profile.ProductBrandId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                if ((user.IsInRole("Biên tập viên") || user.IsInRole("Cộng tác viên") || user.IsInRole("Khách")) && productItem.CreateBy == userId)
                {
                    if (productItem.ProductStatusId == 1 || productItem.ProductStatusId == 3)
                    {
                        return true;
                    }
                    else
                    {
                        messsage = "Không có quyền chỉnh sửa bài viết khi đã cập nhật trạng thái";
                        return false;
                    }

                }

                if (user.IsInRole("Phụ trách chuyên mục"))
                {
                    List<int> lstArtCategoryItem = CmsContext.ProductCategoryProduct.Where(x => x.ProductId == productId).Select(x => x.ProductCategoryId).ToList();
                    var productAssign = CmsContext.ProductCategoryAssign.Where(x => x.AspNetUsersId == userId && x.ProductCategoryId != null && lstArtCategoryItem.Contains(x.ProductCategoryId.Value)).ToList();
                    if (productAssign != null && productAssign.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        messsage = "Không có quyền chỉnh sửa bài viết không thuộc chuyên mục";
                        return false;
                    }
                }
            }
            else
            {
                messsage = "Không tìm thấy bài viết";
                return false;
            }
            messsage = "Không có quyền chỉnh sửa bài viết";
            return false;
        }

        public bool CanEditCommentProduct(ClaimsPrincipal user, string userId, int commentId, ref string messsage)
        {
            var item = CmsContext.ProductComment.Where(x => x.Id == commentId && x.CreateBy == userId);
            if (item != null)
            {
                return true;
            }
            return false;
        }

        public bool CanEditCommentStaffProduct(ClaimsPrincipal user, string userId, int commentId, ref string messsage)
        {
            var item = CmsContext.ProductComment.Where(x => x.Id == commentId && x.CreateBy == userId);
            if (item != null)
            {
                return true;
            }
            return false;
        }

        public  bool CanViewProduct(ClaimsPrincipal user, string userId, int productId, ref string messsage)
        {
            var productItem = CmsContext.Product.Find(productId);
            if (productItem != null)
            {
                if (user.IsInRole("Quản trị hệ thống"))
                {
                    return true;
                }
                if (user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
                {
                    var profile = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId);
                    if (profile != null)
                    {
                        if (productItem.ProductBrandId == profile.ProductBrandId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            else
            {
                messsage = "Không tìm thấy bài viết";
                return false;
            }
            messsage = "Không có quyền xem bài viết";
            return false;
        }

        public bool CanEditProductbrand(ClaimsPrincipal user, string userId, int productbrandId, ref string messsage)
        {
            var productbrandItem = CmsContext.ProductBrand.Find(productbrandId);
            if (productbrandItem != null)
            {
                if (user.IsInRole("Quản trị hệ thống"))
                {
                    return true;
                }
                if (user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
                {
                    var profile = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId && x.ProductBrandId == productbrandId);
                    if (profile != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                messsage = "Không tìm thấy cửa hàng chỉnh sửa";
                return false;
            }
            messsage = "Không có quyền chỉnh sửa cửa hàng";
            return false;
        }

        public bool CanAddNewEmployee(ClaimsPrincipal user, string userId, ref string messsage)
        {
            if (user.IsInRole("Quản trị hệ thống") || user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
            {
                return true;
            }
            else
            {
                messsage = "Không có quyền thêm mới ";
                return false;
            }

        }

        public bool CanEditEmployee(ClaimsPrincipal user, string userId, string employeeId, ref string messsage)
        {
            if (user.IsInRole("Quản trị hệ thống"))
            {
                return true;
            }
            else if (user.IsInRole("Quản trị cửa hàng") || user.IsInRole("Nhân viên cập nhật cửa hàng"))
            {
                var currentUser = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == userId);
                if (currentUser != null)
                {
                    var empUser = CmsContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId == employeeId);
                    if (empUser != null)
                    {
                        if (currentUser.ProductBrandId != empUser.ProductBrandId) return false;
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                messsage = "Không có quyền chỉnh sửa ";
                return false;
            }
        }

        public bool CanDeleteProductCategory(ClaimsPrincipal user, string userId, int productCategoryId, ref string messsage)
        {
            if (user.IsInRole("Quản trị hệ thống"))
            {
                //check can delete
                var item = CmsContext.ProductCategory.SingleOrDefault(x => x.Id == productCategoryId);
                if (item == null) return false;
                if (item.CanDelete == false) return false;
                var hasProduct = CmsContext.ProductCategoryProduct.Count(x => x.ProductCategoryId  == productCategoryId);
                if (hasProduct > 0) return false;
                return true;

            }
            else
            {
                messsage = "Không có quyền chỉnh sửa ";
                return false;
            }
        }

        #endregion

    }
}