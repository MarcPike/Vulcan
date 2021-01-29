using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Context;

namespace Vulcan.IMetal.Queries.Warehouses
{
    public class WarehousesAdvancedQuery
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public static List<WarehousesAdvancedQuery> GetForCoid(string coid)
        {
            var context = ContextFactory.GetStockItemsContextForCoid(coid);
            return context.Warehouse.Select(x =>
                new WarehousesAdvancedQuery() { Code = x.Code, Name = x.Name, ShortName = x.ShortName }).OrderBy(x => x.Code).ToList();


        }
    }
}
