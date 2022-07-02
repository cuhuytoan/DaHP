using Telerik.Blazor.Components;

namespace CMS.Website.Areas.Admin.Pages.Product
{
    public partial class ProductCategoryEdit
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

        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }



        #endregion Parameter

        #region Model
        protected List<SpProductCategoryTreeResult> Data { get; set; } = new();

        protected SpProductPropertyCategoryTreeResult PropertyCate { get; set; }

        protected ConfirmBase DeleteConfirmation { get; set; }

        protected ModalDynamicComponent PropertiesDynamicEdit { get; set; }

        protected SpProductCategoryTreeResult deleteItem { get; set; } = new();
        string message = "";
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


        }

        protected async Task InitData()
        {
            Data = await Repository.ProductCategory.ProductCategoryTreeGetLst();
            Data = Data.Where(x => x.Id > 0).ToList();
            foreach (var p in Data)
            {
                p.Name = p.Name.Replace("--", string.Empty);
            }
            StateHasChanged();
        }

        #endregion Init

        #region Event
        private async Task CreateItem(TreeListCommandEventArgs args)
        {
            var argsItem = args.Item as SpProductCategoryTreeResult;
            if(argsItem.Name is null)
            {
                toastService.ShowWarning("Vui lòng nhập thông tin tên danh mục", "Thông báo");
                return;
            }
            if (argsItem.Sort is null)
            {
                toastService.ShowWarning("Vui lòng nhập thông tin sắp xếp", "Thông báo");
                return;
            }
            //if (argsItem.URL is null)
            //{
            //    toastService.ShowWarning("Vui lòng nhập thông tin đường dẫn", "Thông báo");
            //    return;
            //}
            argsItem.ParentId = (args.ParentItem as SpProductCategoryTreeResult)?.Id;
            //Insert DB
            ProductCategory item = new();
            item.ParentId = argsItem.ParentId;
            item.Name = argsItem.Name;
            item.Sort = argsItem.Sort;
            item.Url = argsItem.URL;
            item.Active = true;
            item.CanDelete = false;
            item.CreateBy = globalModel.userId;
            item.CreateDate = DateTime.Now;
            item.LastEditedBy = globalModel.userId;
            item.LastEditedDate = DateTime.Now;
            var result = await Repository.ProductCategory.ProductCategoryInsertOrUpdate(item);
            if (!result)
            {
                toastService.ShowError("Có lỗi trong quá trình thêm mới", "Thông báo");
                Data.Remove(argsItem);
            }

            await InitData();

        }

        private async Task UpdateItem(TreeListCommandEventArgs args)
        {
            var argsItem = args.Item as SpProductCategoryTreeResult;
            if (argsItem.Name is null)
            {
                toastService.ShowWarning("Vui lòng nhập thông tin tên danh mục", "Thông báo");
                return;
            }
            if (argsItem.Sort is null)
            {
                toastService.ShowWarning("Vui lòng nhập thông tin sắp xếp", "Thông báo");
                return;
            }
            if (argsItem.URL is null)
            {
                toastService.ShowWarning("Vui lòng nhập thông tin đường dẫn", "Thông báo");
                return;
            }
            //Insert DB
            var item = await Repository.ProductCategory.FindAsync(argsItem.Id);
            if (item != null)
            {
                
                item.Id = (int)argsItem.Id;
                item.Name = argsItem.Name;
                item.Sort = argsItem.Sort;
                item.Url = argsItem.URL;
                item.Active = (bool)argsItem.Active;
                item.DisplayMenu = (bool)argsItem.DisplayMenu;
                item.DisplayMenuHorizontal = (bool)argsItem.DisplayMenuHorizontal;
                item.LastEditedBy = globalModel.userId;
                item.LastEditedDate = DateTime.Now;
                var result = await Repository.ProductCategory.ProductCategoryInsertOrUpdate(item);
                if (!result)
                {
                    toastService.ShowError("Có lỗi trong quá trình cập nhật", "Thông báo");
                }
            }
            else
            {
                toastService.ShowError("Không tồn tại dữ liệu cập nhật", "Thông báo");

            }
            await InitData();

        }

        private async Task DeleteItem(TreeListCommandEventArgs args)
        {
            deleteItem = new();
            deleteItem = args.Item as SpProductCategoryTreeResult;
            
            DeleteConfirmation.Show();
           

        }
        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                try
                {
                    if(!Repository.Permission.CanDeleteProductCategory(globalModel.user, globalModel.userId, (int)deleteItem.Id, ref message));
                    {
                        toastService.ShowError("Danh mục không được quyền xóa", "Thông báo");
                        return;
                    }
                    await DeleteChildItems(deleteItem);
                    await Repository.ProductCategory.ProductCategoryDelete((int)deleteItem.Id);
                }
                catch
                {
                    toastService.ShowError("Có lỗi trong quá trình xóa danh mục", "Thông báo");
                }


                await InitData();
            }
        }
        private async Task DeleteChildItems(SpProductCategoryTreeResult parentItem)
        {
            var childItems = Data.Where(x => parentItem.Id.Equals(x.ParentId)).ToList();

            for (int i = 0; i < childItems.Count; i++)
            {

                await Repository.ProductCategory.ProductCategoryDelete((int)childItems[i].Id);
            }
        }
        void UpdateProductProperties(SpProductCategoryTreeResult item)
        {
            if (item != null)
            {
              
                ComponentMetadata componentMeta = new();
                componentMeta.ComponentType = typeof(ProductPropertiesEdit);
                componentMeta.ComponentParameters = new Dictionary<string, object>()
            {
                {"productCategoryId",item.Id }
            };

                PropertiesDynamicEdit.ConfirmationTitle = "Chỉnh sửa thông số kỹ thuật";

                PropertiesDynamicEdit.Component = componentMeta;

                PropertiesDynamicEdit.Show();
            }
        }



        #endregion
    }
}
