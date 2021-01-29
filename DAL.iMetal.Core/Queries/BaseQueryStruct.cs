using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.iMetal.Core.DbUtilities;

namespace DAL.iMetal.Core.Queries
{
    public class BaseQueryStruct<T> where T : struct
    {
        public string Sql { get; set; }

        public virtual async Task<List<T>> ExecuteAsync(string coid, Dictionary<string, object> parameters = null)
        {
            var sqlQuery = new SqlQueryStruct<T>();
            return await sqlQuery.ExecuteQueryAsync(coid, Sql);
        }

        public virtual async Task<List<dynamic>> ExecuteDynamicAsync(string coid, Dictionary<string, object> parameters = null)
        {
            var sqlQueryDynamic = new SqlQueryDynamic(coid, conn: null);
            return await sqlQueryDynamic.ExecuteQueryAsync(Sql);
        }

    }
}