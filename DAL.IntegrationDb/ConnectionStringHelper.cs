using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IntegrationDb
{
    public class ConnectionStringHelper
    {
        public static string GetConnectionString()
        {
            string connectionString = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder
            {
                //Metadata = "res://*/IntegrationDb.csdl|res://*/IntegrationDb.ssdl|res://*/IntegrationDb.msl;",
                Metadata = @"res://*/",
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = new SqlConnectionStringBuilder
                {
                    InitialCatalog = "IntegrationDB_Test",
                    DataSource = @"S-US-SQL2\IMPORT",
                    IntegratedSecurity = true,
                    MultipleActiveResultSets = true,
                }.ConnectionString
            }.ConnectionString;

            return connectionString;
        }

    }
}
