using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ItemResourceCostModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public List<string> ResourceTypeList = new List<string>();
        public List<string> PriceTypeList = new List<string>();

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
            ResourceTypeList.Clear();
            var resources = Enum.GetValues(typeof(ResourceType));
            foreach (var resource in resources)
            {
                ResourceTypeList.Add(resource.ToString());   
            }

            PriceTypeList.Clear();
            var priceTypes = Enum.GetValues(typeof(PerType));
            foreach (var priceType in priceTypes)
            {
                PriceTypeList.Add(priceType.ToString());
            }
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
