using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.iMetal.Core.Context;
using Npgsql;

namespace DAL.iMetal.Core.DbUtilities
{
    public class SqlQuery<T> where T : class, new()
    {
        public async Task<List<T>> ExecuteQueryAsync(string coid, string sql) 
        {
            List<T> result = new List<T>();
            await using var conn = ConnectionFactory.GetConnection(coid);
            try
            {
                conn.Open();
                await using var cmd = new NpgsqlCommand(sql, conn);
                var rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    result.Add(rdr.ConvertToObject<T>());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    await conn.CloseAsync();
                }
            }

            return result;
        }
    }

    public class SqlQueryStruct<T> where T : struct
    {
        public async Task<List<T>> ExecuteQueryAsync(string coid, string sql)
        {
            List<T> result = new List<T>();
            await using var conn = ConnectionFactory.GetConnection(coid);
            try
            {
                conn.Open();
                await using var cmd = new NpgsqlCommand(sql, conn);
                var rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    result.Add(rdr.ConvertToStruct<T>());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    await conn.CloseAsync();
                }
            }

            return result;
        }
    }

}