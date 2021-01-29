using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.iMetal.Core.DbUtilities;

namespace DAL.iMetal.Core.Queries
{
    public class WarehouseQuery: BaseQuery<WarehouseQuery>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Status { get; set; }
        public string WarehouseType { get; set; }

        public WarehouseQuery()
        {
            
        }

        public new static async Task<List<WarehouseQuery>> ExecuteAsync(string coid,
            Dictionary<string, object> parameters = null)
        {
            var sql =
                $@"
                    SELECT
                      w.id AS Id,
                      w.name AS Name,
                      w.code AS Code,
                      w.short_name AS ShortName,
                      w.status AS Status,
                      wtc.description AS WarehouseType
                    FROM
                      warehouses w
                      INNER JOIN warehouse_type_codes wtc ON wtc.id = w.warehouse_type_id
                ";
            var sqlQuery = new SqlQuery<WarehouseQuery>();
            return await sqlQuery.ExecuteQueryAsync(coid, sql);
        }
    }
}
