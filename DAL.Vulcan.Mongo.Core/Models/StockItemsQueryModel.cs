using System;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class StockItemsQueryModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Coid { get; set; } = String.Empty;
        public int ProductId { get; set; } = 0;
        public List<string> SearchAllValues { get; set; } = new List<string>();
        public bool RefreshCache { get; set; } = false;
        public string ProductBucketId { get; set; } = string.Empty;
        public string DisplayCurrency { get; set; } = string.Empty;

        public StockItemsQueryModel()
        {
        }

        public StockItemsQueryModel(string application, string userId)
        {
            Application = application;
            UserId = userId;
        }

    }
}