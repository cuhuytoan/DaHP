using CMS.Common;
using System.Reflection;
using Newtonsoft.Json;

namespace CMS.Services.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<int?> ProductInsert(Product model, string userId, int productStatusId, List<int> productCategoryProduct);

        Task ProductLogEditInsert(ProductLogEdit model, string userId);
        Task ProductUpdate(Product model, string UserId, int ProductStatusId, List<int> productCategoryProduct);

        Task ProductDelete(int ProductId);

        Task<Product> ProductGetById(int ProductId);

        Task<Product> ProductGetByUrl(string url);

        Task<VirtualizeResponse<SpProductSearchResult>> ProductSearchWithPaging(ProductSearchFilter model);

        void ProductTopCategorySave(int ProductId);

        void ProductTopParentCategorySave(int ProductId);

        Task ProductUpdateStatusType(string userId, int ProductId, int StatusTypeId);

        Task<string> CreateProductURL(int ProductId);

        Task<List<ProductStatus>> GetLstProductStatus();

        Task<List<ProductStatus>> GetLstProductStatusByUserId(string userId);

        Task<List<ProductAttachFile>> ProductAttachGetLstByProductId(int productId);

        Task<bool> ProductAttachDelete(int productAttachFileId);

        Task<bool> ProductAttachInsert(List<ProductAttachFile> model);

        Task<List<ProductType>> ProductTypeGetLst();

        Task<List<ProductManufacture>> ProductManufacturesGetLst();

        Task<bool> ProductRelationInsert(List<ProductRelationProduct> model);

        Task<bool> ProductRelationDeleteById(int productId, int productRelationId);

        Task<List<SpProductSearchResult>> ProductRelationGetlstByProductId(int productId);

        Task<List<SpProductBreadcrumbResult>> ProductBreadCrumbGetlst(int productCategoryId);

        Task<int> ProductCountByBrandId(int productBrandId);

        Task<bool> ProductGetLikeByUserId(int productId, string userId);

        Task<bool> ProductLikeInsertOrDelete(bool isLike, ProductLike model);

        Task<bool> ProductLikeInsert(int productId, int productBrandId, string userId);
        Task<bool> ProductLikeDelete(int productId, string userId);

        Task ProductIncreaseCount(int productId);
        Task<int> ProductCountByDateQry(DateTime FromDate, DateTime ToDate);
    }
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {

        public ProductRepository(CmsContext CmsDBContext) : base(CmsDBContext)
        {
        }
        public async Task<int> ProductCountByDateQry(DateTime FromDate, DateTime ToDate)
        {
            return await CmsContext.Product.Where(x => x.CreateDate >= FromDate && x.CreateDate <= ToDate).CountAsync();
        }
        public async Task<int?> ProductInsert(Product model, string userId, int productStatusId, List<int> productCategoryProduct)
        {
            // Add một lần          
            model.ProductBrandId = model.ProductBrandId;
            model.ProductStatusId = productStatusId;
            model.QrcodePublic = Utils.GetRandomText(6);
            model.CreateBy = userId;
            model.CreateDate = DateTime.Now;
            model.LastEditDate = DateTime.Now;
            model.LastEditBy = userId;
            model.CanCopy = true;
            model.CanComment = true;
            model.CanDelete = true;
            model.Active = true;
            model.Counter = 0;
            model.SellCount = 0;
            model.LikeCount = 0;
            model.Approved = -1;
            model.Checked = -1;
            CmsContext.Product.Add(model);

            await CmsContext.SaveChangesAsync();
            //Insert productCategoryProduct
            await ProductSetProductCategory(model.Id, productCategoryProduct);
            return model.Id;
        }

        public async Task ProductUpdate(Product model, string userId, int productStatusId, List<int> productCategoryProduct)
        {
            try
            {
                var items = CmsContext.Product.FirstOrDefault(p => p.Id == model.Id);
                if (items != null)
                {
                    model.ProductStatusId = 1; // Khi update trạng thái luôn = 1
                    model.LastEditBy = userId;
                    model.LastEditDate = DateTime.Now;
                    model.Sku = $"{model.ProductBrandId}{CMS.Common.Utils.SKUGetByYear(DateTime.Now.Year)}{model.ProductCategoryIds}" +
                        $"-{model.Id}{(!String.IsNullOrEmpty(model.ManufactureSku) ? $"/{model.ManufactureSku}" : "")}";
                    if (string.IsNullOrEmpty(items.Url))
                    {
                        model.Url = await CreateProductURL(items.Id);
                    }
                    CmsContext.Entry(items).State = EntityState.Detached;
                    CmsContext.Entry(model).State = EntityState.Modified;
                    await CmsContext.SaveChangesAsync();
                    //Insert productCategoryProduct
                    await ProductSetProductCategory(model.Id, productCategoryProduct);
                    //Insert ProductLogEdit
                    await ProductLogEditInsert(JsonConvert.DeserializeObject<ProductLogEdit>(JsonConvert.SerializeObject(items)), userId);

                }
               
            }
            catch
            {
            }
        }

        public async Task ProductDelete(int ProductId)
        {
            try
            {
                var items = await CmsContext.Product.FirstOrDefaultAsync(p => p.Id == ProductId);
                if (items != null)
                {
                    CmsContext.Product.Remove(items);
                    await CmsContext.SaveChangesAsync();
                    //Delete In ArticleBlockArticle
                    var lstProductBlockProduct = await CmsContext.ProductBlockProduct.Where(x => x.ProductId == ProductId).ToListAsync();
                    if (lstProductBlockProduct != null && lstProductBlockProduct.Count > 0)
                    {
                        CmsContext.ProductBlockProduct.RemoveRange(lstProductBlockProduct);
                        await CmsContext.SaveChangesAsync();
                    }
                    //Delete In ProductRelationProduct
                    var lstProductRelationProduct = await CmsContext.ProductRelationProduct.Where(x => x.ProductId == ProductId).ToListAsync();
                    if (lstProductRelationProduct != null && lstProductRelationProduct.Count > 0)
                    {
                        CmsContext.ProductRelationProduct.RemoveRange(lstProductRelationProduct);
                        await CmsContext.SaveChangesAsync();
                    }
                }
            }
            catch
            {
            }
        }

        public async Task<Product> ProductGetById(int ProductId)
        {
            return await CmsContext.Product.FirstOrDefaultAsync(p => p.Id == ProductId);
        }


        public async Task<VirtualizeResponse<SpProductSearchResult>> ProductSearchWithPaging(ProductSearchFilter model)
        {
            var output = new VirtualizeResponse<SpProductSearchResult>();
            var itemCounts = new OutputParameter<int?>();
            var returnValues = new OutputParameter<int>();
            try
            {
                var result = await CmsContext.GetProcedures().SpProductSearchAsync(
                model.Keyword,
                model.ProductCategoryId,
                model.ProductManufactureId,
                model.ProductStatusId,
                model.CountryId,
                model.LocationId,
                model.DepartmentManId,
                model.ProductBrandId,
                model.ProductTypeId,
                model.ExceptionId,
                model.ExceptionProductTop,
                model.FromPrice,
                model.ToPrice,
                model.FromDate,
                model.ToDate,
                model.Efficiency,
                model.Active,
                model.AssignBy,
                model.CreateBy,
                model.OrderBy,
                model.PageSize,
                model.CurrentPage, itemCounts, returnValues
                );
                output.Items = result.ToList();
                output.TotalSize = (int)itemCounts.Value;
            }
            catch
            {

            }
            return output;
        }

        public async Task ProductSetProductCategory(int ProductId, List<int> productCategoryProduct)
        {
            var listItem = new List<ProductCategoryProduct>();
            var item = await CmsContext.ProductCategoryProduct.Where(p => p.ProductId == ProductId).ToListAsync();
            if (item != null) // Update
            {
                CmsContext.ProductCategoryProduct.RemoveRange(item);
                await CmsContext.SaveChangesAsync();
            }
            //Add
            foreach (var p in productCategoryProduct)
            {
                var itemArtCate = new ProductCategoryProduct
                {
                    ProductId = ProductId,
                    ProductCategoryId = p
                };
                listItem.Add(itemArtCate);
            }
            await CmsContext.ProductCategoryProduct.AddRangeAsync(listItem);
            await CmsContext.SaveChangesAsync();
        }

        public async Task<string> CreateProductURL(int ProductId)
        {
            try
            {
                var currentProduct = await CmsContext.Product.FirstOrDefaultAsync(p => p.Id == ProductId);
                return FormatURL(currentProduct?.Name) + "-" + ProductId.ToString();
            }
            catch
            {
            }
            return "nourl";
        }

        public string ProductGetStatusString(int? ProductStatusId)
        {
            string Result = "";
            var currentProductStatus = CmsContext.ProductStatus.FirstOrDefault(p => p.Id == ProductStatusId);

            if (currentProductStatus.Id == 0)
            {
                Result = "<label class='badge badge-info'>Đã lưu</label>";
            }
            if (currentProductStatus.Id == 1)
            {
                Result = "<label class='badge badge-warning'>Chờ duyệt</label>";
            }
            if (currentProductStatus.Id == 2)
            {
                Result = "<label class='badge badge-success'>Đã đăng</label>";
            }

            return Result;
        }

        public void ProductTopCategorySave(int ProductId)
        {
            var ProductCategoryProduct_Item = CmsContext.ProductCategoryProduct.FirstOrDefault(p => p.ProductId == ProductId);
            int ProductCategoryId = ProductCategoryProduct_Item.ProductCategoryId;

            //var ProductTop_Items = CmsContext.ProductTop.Where(p => p.ProductCategoryId == ProductCategoryId);
            //CmsContext.ProductTop.RemoveRange(ProductTop_Items);

            ProductTop ProductTop_Item = new()
            {
                ProductCategoryId = ProductCategoryId,
                ProductId = ProductId
            };
            CmsContext.ProductTop.Add(ProductTop_Item);

            CmsContext.SaveChanges();
        }

        public void ProductTopParentCategorySave(int ProductId)
        {
            var ProductCategoryProduct_Item = CmsContext.ProductCategoryProduct.FirstOrDefault(p => p.ProductId == ProductId);
            int ProductCategoryId = ProductCategoryProduct_Item.ProductCategoryId;

            var ProductCategory_Item = CmsContext.ProductCategory.FirstOrDefault(p => p.Id == ProductCategoryId);

            if (ProductCategory_Item.ParentId != null)
            {
                int ProductCategoryParentId = ProductCategory_Item.ParentId.Value;
                //var ProductTop_Items = CmsContext.ProductTop.Where(p => p.ProductCategoryId == ProductCategoryParentId);
                //CmsContext.ProductTop.RemoveRange(ProductTop_Items);

                ProductTop ProductTop_Item = new()
                {
                    ProductCategoryId = ProductCategoryParentId,
                    ProductId = ProductId
                };
                CmsContext.ProductTop.Add(ProductTop_Item);

                CmsContext.SaveChanges();
            }
        }

        public async Task ProductUpdateStatusType(string userId, int ProductId, int StatusTypeId)
        {
            var product = await CmsContext.Product.FirstOrDefaultAsync(p => p.Id == ProductId);
            if (product != null)
            {
                if (StatusTypeId == 2 || StatusTypeId == 3) // Kiểm tra
                {
                    product.Checked = 1;
                    product.CheckBy = userId;
                    product.CheckDate = DateTime.Now;
                }
                else if (StatusTypeId == 4) //Duyệt
                {
                    product.Approved = 1;
                    product.ApproveBy = userId;
                    product.ApproveDate = DateTime.Now;
                }
                else if (StatusTypeId == 0) //Từ chối đăng
                {
                    product.Approved = 0;
                    product.ApproveBy = userId;
                    product.ApproveDate = DateTime.Now;
                }
                product.ProductStatusId = StatusTypeId;
                await CmsContext.SaveChangesAsync();
            }
        }

        public async Task<List<ProductStatus>> GetLstProductStatus()
        {
            return await CmsContext.ProductStatus.ToListAsync();
        }

        public async Task<List<ProductAttachFile>> ProductAttachGetLstByProductId(int productId)
        {
            return await CmsContext.ProductAttachFile.Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<bool> ProductAttachDelete(int productAttachFileId)
        {
            var item = await CmsContext.ProductAttachFile.FindAsync(productAttachFileId);
            if (item != null)
            {
                CmsContext.ProductAttachFile.Remove(item);
                await CmsContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ProductAttachInsert(List<ProductAttachFile> model)
        {
            try
            {
                foreach (var p in model)
                {
                    CmsContext.Entry(p).State = EntityState.Added;
                    await CmsContext.SaveChangesAsync();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }



        public async Task<List<ProductStatus>> GetLstProductStatusByUserId(string userId)
        {
            var lstRole = await CmsContext.AspNetUserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();
            if (lstRole != null && lstRole.Count > 0)
            {
                if (lstRole.Contains("6df4162d-38a4-42e9-b3d3-a07a5c29215b")) // Phụ trách chuyên mục
                {
                    List<int> lstStatusId = new() { 1, 2, 3 }; //Trạng thái mới gửi và đã kiểm tra
                    return await CmsContext.ProductStatus.Where(x => lstStatusId.Contains(x.Id)).ToListAsync();
                }
                else
                {
                    return await CmsContext.ProductStatus.ToListAsync();
                }
            }
            else
            {
                return await CmsContext.ProductStatus.ToListAsync();
            }


        }

        public async Task<List<ProductType>> ProductTypeGetLst()
        {
            return await CmsContext.ProductType.AsNoTracking().ToListAsync();
        }

        public async Task<List<ProductManufacture>> ProductManufacturesGetLst()
        {
            return await CmsContext.ProductManufacture.AsNoTracking().ToListAsync();
        }

        public async Task<bool> ProductRelationInsert(List<ProductRelationProduct> model)
        {
            try
            {
                foreach (var p in model)
                {
                    var item = await CmsContext.ProductRelationProduct.FirstOrDefaultAsync(x => x.ProductId == p.ProductId && x.ProductRelationId == p.ProductRelationId);
                    if (item != null) continue;
                    else
                    {
                        CmsContext.ProductRelationProduct.Add(p);
                        await CmsContext.SaveChangesAsync();
                    }
                }

            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ProductRelationDeleteById(int productId, int productRelationId)
        {
            try
            {
                var item = await CmsContext.ProductRelationProduct.FirstOrDefaultAsync(x => x.ProductId == productId && x.ProductRelationId == productRelationId);
                if (item != null)
                {
                    CmsContext.ProductRelationProduct.Remove(item);
                    await CmsContext.SaveChangesAsync();

                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<List<SpProductSearchResult>> ProductRelationGetlstByProductId(int productId)
        {
            List<SpProductSearchResult> lstOutput = new();
            var lstProduct = await CmsContext.ProductRelationProduct.Where(x => x.ProductId == productId).AsNoTracking().ToListAsync();
            if (lstProduct != null)
            {
                foreach (var p in lstProduct)
                {
                    SpProductSearchResult item = new();
                    var product = await CmsContext.Product.FirstOrDefaultAsync(x => x.Id == p.ProductRelationId);
                    if (product != null)
                    {
                        item.Id = product.Id;
                        item.Image = product.Image;
                        item.Name = product.Name;
                        item.CreateDate = product.CreateDate;
                        if (Int32.TryParse(item.ProductCategoryIds, out int _cate))
                        {
                            var cate = await CmsContext.ProductCategory.FirstOrDefaultAsync(x => x.Id == _cate);
                            item.ProductCategoryName = cate.Name;
                        }
                    }
                    lstOutput.Add(item);
                }

            }
            return lstOutput;
        }

        public async Task<List<SpProductBreadcrumbResult>> ProductBreadCrumbGetlst(int productCategoryId)
        {
            var output = new List<SpProductBreadcrumbResult>();
            try
            {
                output = await CmsContext.GetProcedures().SpProductBreadcrumbAsync(productCategoryId);
            }
            catch
            {

            }
            return output;
        }

        public async Task<Product> ProductGetByUrl(string url)
        {
            return await CmsContext.Product.FirstOrDefaultAsync(p => p.Url == url);
        }

        public async Task<int> ProductCountByBrandId(int productBrandId)
        {
            return await CmsContext.Product.CountAsync(x => x.ProductBrandId == productBrandId && x.ProductStatusId == 4);
        }

        public async Task<bool> ProductGetLikeByUserId(int productId, string userId)
        {
            var returnResult = false;
            var item = await CmsContext.ProductLike.CountAsync(x => x.ProductId == productId && x.CreateBy == userId);
            if (item > 0)
            {
                returnResult = true;
            }
            return returnResult;
        }

        public async Task<bool> ProductLikeInsertOrDelete(bool isLike, ProductLike model)
        {
            bool returnResult = false;


            if (isLike)// Delete
            {
                var item = await CmsContext.ProductLike.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (item != null)
                {
                    CmsContext.Entry(item).State = EntityState.Deleted;
                    await CmsContext.SaveChangesAsync();

                    var productItem = await CmsContext.Product.FirstOrDefaultAsync(x => x.Id == model.ProductId);
                    if (productItem != null)
                    {
                        if (productItem.LikeCount > 0)
                        {
                            productItem.LikeCount -= 1;
                            await CmsContext.SaveChangesAsync();
                        }
                    }
                    returnResult = true;
                }

            }
            else // Insert
            {
                CmsContext.Entry(model).State = EntityState.Added;
                await CmsContext.SaveChangesAsync();
                var productItem = await CmsContext.Product.FirstOrDefaultAsync(x => x.Id == model.ProductId);
                if (productItem != null)
                {
                    productItem.LikeCount += 1;
                    await CmsContext.SaveChangesAsync();
                }
                returnResult = true;
            }

            return returnResult;
        }

        public async Task<bool> ProductLikeInsert(int productId, int productBrandId, string userId)
        {
            bool returnResult = false;
            try
            {
                ProductLike model = new();
                model.ProductId = productId;
                model.ProductBrandId = productBrandId;
                model.CreateBy = userId;
                model.CreateDate = DateTime.Now;
                CmsContext.ProductLike.Add(model);
                await CmsContext.SaveChangesAsync();

                var productItem = await CmsContext.Product.FirstOrDefaultAsync(x => x.Id == productId);
                if (productItem != null)
                {
                    productItem.LikeCount += 1;
                    await CmsContext.SaveChangesAsync();
                }
                returnResult = true;
            }
            catch
            {
                return returnResult;
            }
            return returnResult;
        }

        public async Task<bool> ProductLikeDelete(int productId, string userId)
        {
            bool returnResult = false;
            var item = await CmsContext.ProductLike.FirstOrDefaultAsync(x => x.ProductId == productId && x.CreateBy == userId);
            if (item != null)
            {
                CmsContext.ProductLike.Remove(item);
                await CmsContext.SaveChangesAsync();

                var productItem = await CmsContext.Product.FirstOrDefaultAsync(x => x.Id == productId);
                if (productItem != null)
                {
                    if (productItem.LikeCount > 0)
                    {
                        productItem.LikeCount -= 1;
                        await CmsContext.SaveChangesAsync();
                    }
                }
                returnResult = true;
            }
            return returnResult;
        }

        public async Task ProductIncreaseCount(int productId)
        {
            var prod = await CmsContext.Product.FirstOrDefaultAsync(x => x.Id == productId);
            if (prod != null)
            {
                prod.Counter += 1;
                await CmsContext.SaveChangesAsync();
            }

        }

        public async Task ProductLogEditInsert(ProductLogEdit model, string userId)
        {
            model.CreateBy = userId;
            model.CreateDate = DateTime.Now;
            CmsContext.Entry(model).State = EntityState.Added;
            await CmsContext.SaveChangesAsync();
        }
    }
}
