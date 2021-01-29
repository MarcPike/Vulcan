using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Context;

namespace Vulcan.IMetal.Queries.GeneralInfo
{
    public class SalesGroupQuery
    {
        public string Coid { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public static List<SalesGroupQuery> GetForCoid(string coid)
        {
            var result = new List<SalesGroupQuery>();
            var context = ContextFactory.GetGeneralInfoContextForCoid(coid);
            foreach (var salesGroup in context.SalesGroup.Where(x=>x.Status == "A"))
            {
                result.Add(new SalesGroupQuery()
                {
                    Coid = coid,
                    Code = salesGroup.Code,
                    Description = salesGroup.Description
                });
            }
            return result;
        }
    }
}
