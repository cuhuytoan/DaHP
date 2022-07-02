namespace CMS.Website.Areas.Admin.Pages.Article
{
    public partial class Index : IDisposable
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
        public string keyword { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? articleCategoryId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? articleStatusId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int? p { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public int currentPage { get; set; }

        #endregion Parameter

        #region Model

        private List<SpArticleSearchResult> lstArticle;

        public int totalCount { get; set; }
        public int pageSize { get; set; } = 30;
        public int totalPages => (int)Math.Ceiling(decimal.Divide(totalCount, pageSize));
        public ArticleSearchFilter modelFilter { get; set; }

        public int? setArticleStatusSelected { get; set; }
        private List<ArticleCategory> lstArticleCategory { get; set; }
        private List<ArticleStatus> lstArticleStatus { get; set; }
        private string subTitle { get; set; } = "bài viết đã cập nhật";
        public string outMessage = "";

        [CascadingParameter]
        protected GlobalModel globalModel { get; set; }

        protected ConfirmBase DeleteConfirmation { get; set; }

        protected ModalDynamicComponent PropertiesDynamicEdit { get; set; }

        private List<int> listArticleSelected { get; set; } = new List<int>();

        private bool isCheck { get; set; }

        #endregion Model

        #region LifeCycle

        protected override void OnInitialized()
        {
            //Add for change location and seach not reload page
            NavigationManager.LocationChanged += HandleLocationChanged;
        }

        protected override async Task OnInitializedAsync()
        {
            await InitControl();
            await InitData();
        }



        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
            GC.SuppressFinalize(this);
        }

        #endregion LifeCycle

        #region Init

        protected async Task InitControl()
        {
            //Binding Category
            if (globalModel.user.IsInRole("Phụ trách chuyên mục"))
            {
                var lstArticleCate = await Repository.ArticleCategory.GetArticleCategoryByUserId(globalModel.userId);
                if (lstArticleCate != null)
                {
                    lstArticleCategory = lstArticleCate.Select(x => new ArticleCategory { Id = x.Id, Name = x.Name }).ToList();
                }
                //Binding Status
                var lstStatus = await Repository.Article.GetLstArticleStatusByUserId(globalModel.userId);
                if (lstStatus != null)
                {
                    lstArticleStatus = lstStatus.Select(x => new ArticleStatus { Id = x.Id, Name = x.Name }).ToList();
                }
            }

            else
            {
                var lstArticleCate = await Repository.ArticleCategory.GetArticleCategoryById(null);
                if (lstArticleCate != null)
                {
                    lstArticleCategory = lstArticleCate.Select(x => new ArticleCategory { Id = x.Id, Name = x.Name }).ToList();
                }
                //Binding Status
                var lstStatus = await Repository.Article.GetLstArticleStatus();
                if (lstStatus != null)
                {
                    lstArticleStatus = lstStatus.Select(x => new ArticleStatus { Id = x.Id, Name = x.Name }).ToList();
                }
            }


        }

        protected async Task InitData()
        {
            Logger.LogDebug("Init");
            ArticleSearchFilter modelFilter = new()
            {
                Keyword = keyword,
                ArticleCategoryId = articleCategoryId,
                CurrentPage = p ?? 1,
                PageSize = 30,
                ArticleStatusId = articleStatusId,
                FromDate = DateTime.Now.AddYears(-10),
                ToDate = DateTime.Now
            };
            if (globalModel.user.IsInRole("Cộng tác viên"))
            {
                modelFilter.CreateBy = globalModel.userId;
            }
            if (globalModel.user.IsInRole("Phụ trách chuyên mục"))
            {
                modelFilter.AssignBy = globalModel.userId;
            }
            var result = await Repository.Article.ArticleSearchWithPaging(modelFilter);

            lstArticle = result.Items;
            totalCount = result.TotalSize;

            //Init Selected
            listArticleSelected.Clear();
            StateHasChanged();
        }

        #endregion Init

        #region Event

        protected async Task OnPostDemand(int postType)
        {
            if (listArticleSelected.Count == 0)
            {
                toastService.ShowToast(ToastLevel.Warning, "Chưa chọn bài viết thực thi", "Thông báo");
                return;
            }
            else
            {
                try
                {
                    foreach (var item in listArticleSelected)
                    {
                        await Repository.Article.ArticleUpdateStatusType(globalModel.userId, item, postType);
                    }

                    toastService.ShowToast(ToastLevel.Success, "Cập nhật thành công", "Thành công!");

                }
                catch (Exception ex)
                {
                    toastService.ShowToast(ToastLevel.Warning, $"Có lỗi trong quá trình thực thi {ex}", "Lỗi!");
                }

                StateHasChanged();
                await InitData();
            }
        }

        protected void DeleteArticle(int? articleId)
        {

            if (articleId == null) // Delete Demand
            {
                if (listArticleSelected.Count == 0)
                {
                    toastService.ShowToast(ToastLevel.Warning, "Chưa chọn bài viết để xóa", "Thông báo");
                    return;
                }
            }
            else
            {
                listArticleSelected.Clear();
                listArticleSelected.Add((int)articleId);
                if (!Repository.Permission.CanDeleteArticle(globalModel.user, globalModel.userId, (int)articleId, ref outMessage))
                {
                    toastService.ShowError(outMessage, "Thông báo");
                    return;
                }

            }

            DeleteConfirmation.Show();
        }

        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                try
                {
                    foreach (var item in listArticleSelected)
                    {
                        await Repository.Article.ArticleDelete(item);
                    }
                    toastService.ShowToast(ToastLevel.Success, "Xóa bài viết thành công", "Thành công");
                }
                catch
                {
                    toastService.ShowToast(ToastLevel.Warning, "Có lỗi trong quá trình thực thi", "Lỗi!");
                }
                StateHasChanged();
                await InitData();
            }
        }
        protected void ArticleRelationShow(int articleId)
        {
            ComponentMetadata componentMeta = new();
            componentMeta.ComponentType = typeof(ArticleRelation);
            componentMeta.ComponentParameters = new Dictionary<string, object>()
            {
                {"ArticleId",articleId }
            };
            PropertiesDynamicEdit.ConfirmationTitle = "Chỉnh sửa bài viết liên quan";
            PropertiesDynamicEdit.Component = componentMeta;

            PropertiesDynamicEdit.Show();
        }
        protected void ArticleCommentShow(int articleId)
        {
            ComponentMetadata componentMeta = new();
            componentMeta.ComponentType = typeof(ArticleCommentMng);
            componentMeta.ComponentParameters = new Dictionary<string, object>()
            {
                {"articleId",articleId }
            };
            PropertiesDynamicEdit.ConfirmationTitle = "Duyệt bình luận";
            PropertiesDynamicEdit.Component = componentMeta;

            PropertiesDynamicEdit.Show();
        }
        protected void OnCheckBoxChange(bool headerChecked, int ArticleId, object isChecked)
        {
            if (headerChecked)
            {
                if ((bool)isChecked)
                {
                    listArticleSelected.AddRange(lstArticle.Select(x => x.Id));
                    isCheck = true;
                }
                else
                {
                    isCheck = false;
                    listArticleSelected.Clear();
                }
            }
            else
            {
                if ((bool)isChecked)
                {
                    if (!listArticleSelected.Contains(ArticleId))
                    {
                        listArticleSelected.Add(ArticleId);
                    }
                }
                else
                {
                    if (listArticleSelected.Contains(ArticleId))
                    {
                        listArticleSelected.Remove(ArticleId);
                    }
                }
            }
            StateHasChanged();
        }

        protected async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await InitData();
        }

        private void OnChangeArticleStatus(int artStatusId)
        {
            subTitle = lstArticleStatus.Where(x => x.Id == artStatusId).First()?.Name;
            StateHasChanged();
        }
        #endregion Event
    }
}