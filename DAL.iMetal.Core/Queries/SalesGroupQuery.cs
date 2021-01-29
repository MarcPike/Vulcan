using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.iMetal.Core.DbUtilities;

namespace DAL.iMetal.Core.Queries
{
    public class SalesGroupQuery : BaseQuery<SalesGroupQuery>
    {
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public bool IsActive
        {
            get { return Status == "A"; }
        }

        public new static async Task<List<SalesGroupQuery>> ExecuteAsync(string coid,
            Dictionary<string, object> parameters = null)
        {
            var sqlQuery = new SqlQuery<SalesGroupQuery>();
            return await sqlQuery.ExecuteQueryAsync(coid, 
            $@"
            SELECT
                '{coid}' as Coid,
                sg.code AS Code,
                sg.description AS Description,
                sg.status AS Status
            FROM
                public.sales_groups sg
            ");
        }

    }
}
