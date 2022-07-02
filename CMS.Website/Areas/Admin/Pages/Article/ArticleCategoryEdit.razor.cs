using Telerik.Blazor.Components;

namespace CMS.Website.Areas.Admin.Pages.Article
{
    public partial class ArticleCategoryEdit
    {


        #region Parameter

        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }



        #endregion Parameter

        #region Model
        protected List<SpArticleCategoryTreeResult> Data { get; set; } = new();

        protected ConfirmBase DeleteConfirmation { get; set; }
        #endregion

        #region LifeCycle

        protected override async Task OnInitializedAsync()
        {
            //await InitControl();
            await InitData();
            StateHasChanged();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion LifeCycle

        #region Init



        protected async Task InitData()
        {
            Data = await Repository.ArticleCategory.ArticleCategoryTreeGetLst();
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
            var argsItem = args.Item as SpArticleCategoryTreeResult;
            //var lastId = Data.Count > 0 ? Data.Max(x => x.Id) + 1 : 1;

            //argsItem.Id = lastId;
            argsItem.ParentId = (args.ParentItem as SpArticleCategoryTreeResult)?.Id;

            //Data.Insert(0, argsItem);
            //Insert DB
            ArticleCategory item = new();
            item.ParentId = argsItem.ParentId;
            item.Name = argsItem.Name;
            item.Sort = argsItem.Sort;
            item.Active = true;
            item.CanDelete = false;
            item.CreateBy = globalModel.userId;
            item.CreateDate = DateTime.Now;
            item.LastEditedBy = globalModel.userId;
            item.LastEditedDate = DateTime.Now;
            var result = await Repository.ArticleCategory.ArticleCategoryInsertOrUpdate(item);
            if (!result)
            {
                toastService.ShowError("Có lỗi trong quá trình thêm mới", "Thông báo");
                Data.Remove(argsItem);
            }

            await InitData();

        }

        private async Task UpdateItem(TreeListCommandEventArgs args)
        {
            var argsItem = args.Item as SpArticleCategoryTreeResult;

            //Insert DB
            var item = await Repository.ArticleCategory.FindAsync(argsItem.Id);
            if (item != null)
            {
                //var index = Data.FindIndex(i => i.Id == argsItem.Id);

                //if (index != -1)
                //{
                //    Data[index].Name = argsItem.Name;
                //    Data[index].Sort = argsItem.Sort;
                //}
                item.Id = (int)argsItem.Id;
                item.Name = argsItem.Name;
                item.Sort = argsItem.Sort;
                item.LastEditedBy = globalModel.userId;
                item.LastEditedDate = DateTime.Now;
                var result = await Repository.ArticleCategory.ArticleCategoryInsertOrUpdate(item);
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
            var argsItem = args.Item as SpArticleCategoryTreeResult;

            try
            {
                DeleteChildItems(argsItem);
                await Repository.ArticleCategory.ArticleCategoryDelete((int)argsItem.Id);
            }
            catch
            {
                toastService.ShowError("Có lỗi trong quá trình xóa danh mục", "Thông báo");
            }


            await InitData();

        }

        private async Task DeleteChildItems(SpArticleCategoryTreeResult parentItem)
        {
            var childItems = Data.Where(x => parentItem.Id.Equals(x.ParentId)).ToList();

            for (int i = 0; i < childItems.Count; i++)
            {

                await Repository.ArticleCategory.ArticleCategoryDelete((int)childItems[i].Id);
            }
        }
        #endregion
    }
}
