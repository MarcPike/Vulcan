using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Models
{
    public class ItemResourceCostModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Dictionary<int,string> ResourceTypeList = new Dictionary<int, string>();
        public Dictionary<int, string> PriceTypeList = new Dictionary<int, string>();

        public int ResourceTypeId { get; set; } = 0;
        public int PriceTypeId { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public decimal Cost { get; set; } = 0;
        public bool IsPriceBlended { get; set; } = true;

        public ItemResourceCostModel(ItemResourceCost cost)
        {
            InitializeEnumList();
            Id = cost.Id.ToString();
            ResourceTypeId = (int) cost.ResourceType;
            PriceTypeId = (int) cost.PerType;
            Price = cost.Price;
            Cost = cost.Cost;
            IsPriceBlended = cost.IsPriceBlended;
        }

        public ItemResourceCostModel()
        {
            InitializeEnumList();
        }

        private void InitializeEnumList()
        {
            ResourceTypeList = Enum.GetValues(typeof(ResourceType))
                .Cast<ResourceType>()
                .ToDictionary(t => (int) t, t => t.ToString());
            PriceTypeList = Enum.GetValues(typeof(PerType))
                .Cast<PerType>()
                .ToDictionary(t => (int)t, t => t.ToString());
        }

        public ItemResourceCost AsItemResourceCost()
        {

            var resourceType = (ResourceType)Enum.ToObject(typeof(ResourceType), ResourceTypeId);
            var priceType = (PerType) Enum.ToObject(typeof(PerType), PriceTypeId);

            return new ItemResourceCost()
            {
                Id = Guid.Parse(this.Id),
                ResourceType = resourceType,
                PerType = priceType,
                Price = this.Price,
                Cost = this.Cost,
                IsPriceBlended = this.IsPriceBlended
            };
        }
    }
}
