using System.Data.SqlClient;
using DAL.Vulcan.Mongo.Base.Context;

namespace Vulcan.iMetal.Quote.Export.Ado
{
    public class ConnectionFactory
    {
        public string ConnectionString { get; }
        public SqlConnection Connection { get; }
        public string DatabaseNamePrefix { get; }

        public ConnectionFactory()
        {
            if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
            {
                ConnectionString =
                    @"data source=S-US-SQL2\IMPORT;initial catalog=IntegrationDB_Test;integrated security=True;MultipleActiveResultSets=True;";
                DatabaseNamePrefix = "IntegrationDB_Test.dbo.";
            }
            else
            {
                ConnectionString =
                    @"data source=S-US-SQL2\IMPORT;initial catalog=IntegrationDB;integrated security=True;MultipleActiveResultSets=True;";
                DatabaseNamePrefix = "IntegrationDB.dbo.";

            }
            Connection = new SqlConnection(ConnectionString);
        }
    }
}