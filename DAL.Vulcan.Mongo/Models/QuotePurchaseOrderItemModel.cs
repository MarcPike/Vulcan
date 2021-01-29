using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;

namespace DAL.Vulcan.Mongo.Models
{
    public class QuotePurchaseOrderItemModel
    {
        public string Application { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Coid { get; set; } = string.Empty;
        public int ProductId { get; set; } = 0;
        public decimal CostPerPound { get; set; } = 0;

        public OrderQuantity OrderQuantity { get; set; } = new OrderQuantity(1,1,"in");
        public string DisplayCurrency { get; set; } = string.Empty;
        public string CompanyId { get; set; }

    }
}
