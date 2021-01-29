using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.iMetal.Core.Context;
using Npgsql;

namespace DAL.iMetal.Core.DbUtilities
{
    public class SqlQueryDynamic : IDisposable
    {
        private NpgsqlConnection Connection;

        public SqlQueryDynamic()
        {

        }

        public SqlQueryDynamic(string coid, NpgsqlConnection conn = null)
        {
            Connection ??= ConnectionFactory.GetConnection(coid);
        }

        public async Task<List<dynamic>> ExecuteQueryAsync(string sql)
        {
            List<dynamic> result = new List<dynamic>();
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    await Connection.OpenAsync();
                }

                await using var cmd = new NpgsqlCommand(sql, Connection);
                var rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    result.Add(rdr.ConvertToDynamic());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return result;
        }

        public void Dispose()
        {
            if ((Connection != null) && (Connection.State == ConnectionState.Open))
            {
                Connection.Close();
                Connection.Dispose();
            }
        }
    }
}