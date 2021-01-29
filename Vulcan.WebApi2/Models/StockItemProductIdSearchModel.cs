using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.WebApi2.Models
{
    public class StockItemProductIdSearchModel
    {
        public string Coid { get; set; } = string.Empty;
        public List<int> ProductIdList { get; set; } = new List<int>();
    }
}
