using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Context;

namespace Vulcan.IMetal.Queries.GeneralInfo
{
    public class SalesGroup
    {
        public string Coid { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public static List<SalesGroup> GetForCoid(string coid)
        {
            var result = new List<SalesGroup>();
            var context = ContextFactory.GetGeneralInfoContextForCoid(coid);
            foreach (var salesGroup in context.SalesGroup)
            {
                result.Add(new SalesGroup()
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
