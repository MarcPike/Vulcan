using System.Collections.Generic;
using Vulcan.IMetal.Models;

namespace Vulcan.WebApi2.Models
{
    public class SalesPersonListModel
    {
        public string SalesPerson { get; set; }
        public ItemMarginModel Margin { get; set; }
        //public List<OrderListModel> Orders { get; set; } = new List<OrderListModel>();

        public SalesPersonListModel(string salesPerson, ItemMarginModel margin)
        {
            SalesPerson = salesPerson;
            Margin = margin;
        }
    }
}