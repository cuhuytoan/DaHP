using Telerik.Blazor.Components;

namespace CMS.Website.Areas.Admin.Pages.Product
{
    public partial class ProductPropertiesEdit
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
        public int productCategoryId { get; set; }

        [Parameter]
        public EventCallback<bool> OnCloseModal { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; }

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Thông tin";


        protected string productCategoryName { get; set; }

        protected bool enablePropertyType { get; set; }

        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }


        #endregion

        #region Model
        protected List<SpProductPropertyCategoryTreeResult> Data { get; set; } = new();

        protected SpProductPropertyCategoryTreeResult PropertyType { get; set; } = new();

        protected List<ProductPropertyType> DataPropertyType { get; set; } = new();

        protected ProductPropertyType ProdType { get; set; } = new();

        public TelerikTreeList<SpProductPropertyCategoryTreeResult> TreeListRef { get; set; } = new();

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
            DataPropertyType = await Repository.ProductProperties.ProductPropertyTypeGetLst();

            if (DataPropertyType != null)
            {
                DataPropertyType = DataPropertyType.Select(x => new ProductPropertyType { Id = x.Id, Name = x.Name }).ToList();
            }

        }

        protected async Task InitData()
        {
            var item = await Repository.ProductCategory.FindAsync(productCategoryId);
            if (item != null)
            {
                productCategoryName = item.Name;
            }
            Data = await Repository.ProductProperties.ProductPropertiesSpGetTreeLst(productCategoryId);

        }

        #endregion Init

        #region Event
        private async Task CreateItem(TreeListCommandEventArgs args)
        {
            var argsItem = args.Item as SpProductPropertyCategoryTreeResult;
            var itemParent = Data.FirstOrDefault(x => x.Id == argsItem.ParentId);

            //argsItem.ParentId = Data.FirstOrDefault(x => x.Id == argsItem.ParentId)?.ProductPropertyId;

            argsItem.ParentId = (args.ParentItem as SpProductPropertyCategoryTreeResult)?.ProductPropertyId;

            if (argsItem.ParentId != null)
            {
                //Insert ProductProperty
                ProductProperty itemProductProperty = new();
                itemProductProperty.ProductCategoryId = productCategoryId;
                itemProductProperty.ProductPropertyCategoryId = argsItem.ParentId;
                itemProductProperty.ProductPropertyTypeId = 1;// Default 1 argsItem.ProductPropertyTypeId;
                itemProductProperty.Name = argsItem.Name;
                itemProductProperty.UnitId = argsItem.UnitId;
                itemProductProperty.Sort = argsItem.Sort;
                itemProductProperty.CreateBy = globalModel.userId;
                itemProductProperty.LastEditBy = globalModel.userId;
                itemProductProperty.CreateDate = DateTime.Now;
                itemProductProperty.LastEditDate = DateTime.Now;
                var result = await Repository.ProductProperties.ProductPropertyInsertOrUpdate(itemProductProperty);
                if (!result)
                {
                    toastService.ShowError("Có lỗi trong quá trình thêm mới", "Thông báo");
                    Data.Remove(argsItem);
                }

            }
            else
            {
                //Insert ProductPropertyCategory
                ProductPropertyCategory itemProductProCate = new();
                itemProductProCate.ProductCategoryId = productCategoryId;
                itemProductProCate.Name = argsItem.Name;
                itemProductProCate.Sort = argsItem.Sort;
                itemProductProCate.CreateBy = globalModel.userId;
                itemProductProCate.LastEditBy = globalModel.userId;
                itemProductProCate.CreateDate = DateTime.Now;
                itemProductProCate.LastEditDate = DateTime.Now;
                var result = await Repository.ProductProperties.ProductPropertyCategoryInsertOrUpdate(itemProductProCate);
                if (!result)
                {
                    toastService.ShowError("Có lỗi trong quá trình thêm mới", "Thông báo");
                    Data.Remove(argsItem);
                }
            }


            await InitData();

        }

        private async Task UpdateItem(TreeListCommandEventArgs args)
        {
            var argsItem = args.Item as SpProductPropertyCategoryTreeResult;

            var itemParent = Data.FirstOrDefault(x => x.Id == argsItem.ParentId);

            argsItem.ParentId = Data.FirstOrDefault(x => x.Id == argsItem.ParentId)?.ProductPropertyId;

            if (argsItem.ParentId != null)
            {
                //Update ProductProperty
                ProductProperty itemProductProperty = new();
                itemProductProperty.Id = (int)argsItem.ProductPropertyId;
                itemProductProperty.ProductCategoryId = productCategoryId;
                itemProductProperty.ProductPropertyCategoryId = argsItem.ParentId;
                itemProductProperty.ProductPropertyTypeId = argsItem.ProductPropertyTypeId;
                itemProductProperty.Name = argsItem.Name;
                itemProductProperty.UnitId = argsItem.UnitId;
                itemProductProperty.Sort = argsItem.Sort;
                itemProductProperty.LastEditBy = globalModel.userId;
                itemProductProperty.LastEditDate = DateTime.Now;
                var result = await Repository.ProductProperties.ProductPropertyInsertOrUpdate(itemProductProperty);
                if (!result)
                {
                    toastService.ShowError("Có lỗi trong quá trình cập nhật", "Thông báo");

                }

            }
            else
            {
                //Insert ProductPropertyCategory
                ProductPropertyCategory itemProductProCate = new();
                itemProductProCate.Id = (int)argsItem.ProductPropertyId;
                itemProductProCate.ProductCategoryId = productCategoryId;
                itemProductProCate.Name = argsItem.Name;
                itemProductProCate.Sort = argsItem.Sort;
                itemProductProCate.CreateBy = globalModel.userId;
                itemProductProCate.LastEditBy = globalModel.userId;
                itemProductProCate.CreateDate = DateTime.Now;
                itemProductProCate.LastEditDate = DateTime.Now;
                var result = await Repository.ProductProperties.ProductPropertyCategoryInsertOrUpdate(itemProductProCate);
                if (!result)
                {
                    toastService.ShowError("Có lỗi trong quá trình cập nhật", "Thông báo");

                }
            }

            await InitData();

        }

        private async Task DeleteItem(TreeListCommandEventArgs args)
        {
            var argsItem = args.Item as SpProductPropertyCategoryTreeResult;

            try
            {

                if ((bool)argsItem.HaveChild)
                {
                    var childItems = Data.Where(x => argsItem.ProductPropertyId.Equals(x.ParentId)).ToList();
                    if (childItems.Count > 0)
                    {
                        for (int i = 0; i < childItems.Count; i++)
                        {
                            await Repository.ProductProperties.ProductPropertyDelete((int)childItems[i].ProductPropertyId);
                        }
                    }

                    await Repository.ProductProperties.ProductPropertyCategoryDelete((int)argsItem.ProductPropertyId);
                }
                else // Delete in ProductPropertyCategory
                {
                    await Repository.ProductProperties.ProductPropertyDelete((int)argsItem.ProductPropertyId);
                }
            }
            catch
            {
                toastService.ShowError("Có lỗi trong quá trình xóa danh mục", "Thông báo");
            }


            await InitData();

        }



        bool VisiblePropeties(SpProductPropertyCategoryTreeResult item)
        {
            if (item != null)
            {
                if (item.ParentId != null)
                {
                    return false;
                }
            }
            return true;
        }
        void CanAddChild(TreeListCommandEventArgs e)
        {
            SpProductPropertyCategoryTreeResult checkItem = e.Item as SpProductPropertyCategoryTreeResult;
            if (checkItem.ParentId != null)
            {
                e.IsCancelled = true;
                toastService.ShowError("Không thể tạo cấp con cho giá trị", "Thông báo");
            }
            else
            {
                var state = TreeListRef.GetState();
                state.InsertedItem = new SpProductPropertyCategoryTreeResult() { HaveChild = false };
                TreeListRef.SetState(state);
            }
        }
        async Task InsertItem()
        {
            var state = TreeListRef.GetState();
            state.InsertedItem = new SpProductPropertyCategoryTreeResult() { HaveChild = true };
            await TreeListRef.SetState(state);
        }
        async Task OnStateInitHandler(TreeListStateEventArgs<SpProductPropertyCategoryTreeResult> args)
        {
            var expandedState = new TreeListState<SpProductPropertyCategoryTreeResult>()
            {
                //collapse all items in the TreeList upon initialization of the state
                ExpandedItems = Data.Where(x => x.HaveChild == true).ToList()

            };

            args.TreeListState.ExpandedItems = Data;
        }
        #endregion
    }
}
