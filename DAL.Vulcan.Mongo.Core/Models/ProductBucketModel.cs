using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ProductBucketModel
    {
        public string Coid { get; set; } = string.Empty;
        public string Application { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public List<string> ProductCategories { get; set; }
        public string CategoryConditionAndOrValue { get; set; }
        public List<string> ProductConditions { get; set; }
        public List<string> ShowOnlyWarehouseCodes { get; set; }
        public List<string> IgnoreWarehouseCodes { get; set; }
        public string Name { get; set; }

        public ProductBucketModel()
        {
        }

        public ProductBucketModel(string application, string userId, ProductBucket bucket)
        {
            Coid = bucket.Coid;
            Application = application;
            UserId = userId;
            Id = bucket.Id.ToString();
            ProductCategories = bucket.ProductCategories;
            ProductConditions = bucket.ProductConditions;
            IgnoreWarehouseCodes = bucket.IgnoreWarehouseCodes;
            ShowOnlyWarehouseCodes = bucket.ShowOnlyWarehouseCodes;
            CategoryConditionAndOrValue = bucket.CategoryConditionAndOrValue.ToString();
            Name = bucket.Name;
        }

    }
}
