using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.iMetal.Core.DbUtilities;
using DAL.iMetal.Core.Models;

namespace DAL.iMetal.Core.Queries
{
    public class BaseQuery<T> where T : class, new() 
    {
        protected static string Sql { get; set; }

        public virtual async Task<List<T>> ExecuteAsync(string coid, Dictionary<string, object> parameters = null)
        {
            var sqlQuery = new SqlQuery<T>();
            return await sqlQuery.ExecuteQueryAsync(coid, Sql);
        }

        public static async Task<List<dynamic>> ExecuteDynamicAsync(string coid, Dictionary<string, object> parameters = null)
        {
            var sqlQueryDynamic = new SqlQueryDynamic(coid,conn:null);
            return await sqlQueryDynamic.ExecuteQueryAsync(Sql);
        }

    }
}
