using System.Collections.Generic;

namespace Vulcan.SVC.WebApi.Core.Models
{
    public class StockItemProductIdSearchModel
    {
        public string Coid { get; set; } = string.Empty;
        public List<int> ProductIdList { get; set; } = new List<int>();
    }
}
