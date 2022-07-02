using CMS.Data.ModelEntity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CMS.Data.ModelDTO
{
    public class GlobalModel
    {
        public int? totalUnread { get; set; }

        public int? totalInCart { get; set; }
        public List<SpUserNotifySearchResult> lstUserNoti { get; set; } = new List<SpUserNotifySearchResult>();
        public string avatar { get; set; } = "noimages.png";
        public ClaimsPrincipal user { get; set; }
        public string userId { get; set; }
        public string fullName { get; set; }
        public string token { get; set; }

        public int? productBrandId { get; set; }
        public int? productBrandStatusId { get; set; }
        public string productBrandImage { get; set; } = "noimages.png";

        public event Action OnChange;

        public void SetTotalInCart(int number)
        {
            totalInCart = number;
            NotifyStateChanged();
        }
        public void SetProductBrand(int id)
        {
            if (productBrandId == null)
            {
                productBrandId = id;
                NotifyStateChanged();
            }
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}