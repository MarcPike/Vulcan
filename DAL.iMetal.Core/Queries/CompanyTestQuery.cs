using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.iMetal.Core.DbUtilities;

namespace DAL.iMetal.Core.Queries
{
    public class CompanyTestQuery : BaseQuery<CompanyTestQuery>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public CompanyTestQuery()
        {
            Sql = $"SELECT id, code, name FROM companies";
        }

        public new static async Task<List<CompanyTestQuery>> ExecuteAsync(string coid,
            Dictionary<string, object> parameters = null)
        {
            var sqlQuery = new SqlQuery<CompanyTestQuery>();
            return await sqlQuery.ExecuteQueryAsync(coid, Sql);
        }
    }
}

    