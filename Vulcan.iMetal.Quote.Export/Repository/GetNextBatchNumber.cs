using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DAL.IntegrationDb;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    public static class GetNextBatchNumber
    {
        public static int Execute()
        {
            object _lock = new object();
            lock (_lock)
            {
                using (var ctx = new IntegrationDb())
                {
                    using (ctx.Database.Connection)
                    {
                        ctx.Database.Connection.Open();
                        var cmd = ctx.Database.Connection.CreateCommand();
                        cmd.CommandText = "GetNextBatchNumber";
                        cmd.CommandType = CommandType.StoredProcedure;
                        var nextBatchNumberParm = new SqlParameter("NextBatchNumber", 0) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(nextBatchNumberParm);

                        var result = (int)cmd.ExecuteScalar();

                        while (ctx.import_sales_headers.Any(x => x.import_batch_number == result))
                        {
                            result = (int)cmd.ExecuteScalar();
                        }

                        //Access output variable after reader is closed
                        return result;
                    }
                }
            }

        }
    }
}
