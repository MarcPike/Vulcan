using System;
using System.Collections.Generic;

namespace Vulcan.WebApi2.Models
{
    public class PurchaseOrderItemsQueryModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Coid { get; set; } = String.Empty;
        public int ProductId { get; set; } = 0;
        public List<string> SearchAllValues { get; set; } = new List<string>();
        public bool RefreshCache { get; set; } = false;
        public string ProductBucketId { get; set; } = string.Empty;
        public string DisplayCurrency { get; set; } = string.Empty;

        public PurchaseOrderItemsQueryModel()
        {
        }

        public PurchaseOrderItemsQueryModel(string application, string userId)
        {
            Application = application;
            UserId = userId;
        }

    }
}