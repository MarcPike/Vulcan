using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Bindings;
using MongoDB.Driver.Core.Operations;
using MongoDB.Driver.GridFS;
using Environment = DAL.Vulcan.Mongo.Base.Core.Context.Environment;

namespace DAL.Vulcan.Mongo.Base.Core.Context
{
    public class BiContext : ContextBase
    {
        public async Task<BsonValue> EvaluateJavascriptAsync(string javascript)
        {

            var function = new BsonJavaScript(javascript);
            var op = new EvalOperation(Database.DatabaseNamespace, function, null);

            using (var writeBinding = new WritableServerBinding(MongoClient.Cluster, new CoreSessionHandle(new NoCoreSession())))
            {
                return await op.ExecuteAsync(writeBinding, CancellationToken.None);
            }
        }

        public BsonValue EvaluateJavascript(string javascript)
        {

            var function = new BsonJavaScript(javascript);
            var op = new EvalOperation(Database.DatabaseNamespace, function, null);

            using (var writeBinding = new WritableServerBinding(MongoClient.Cluster, new CoreSessionHandle(new NoCoreSession())))
            {
                return op.Execute(writeBinding, CancellationToken.None);
            }
        }

        public BiContext()
        {
            ConnectionString = @"mongodb://S-US-MDB03:27017/BusinessIntelligence";
            DatabaseName = "BI";


            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);

            //ConnectionString = Settings.Default.ConnectionString;

            var databaseSuffix = string.Empty;
            if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
                databaseSuffix = "Dev";
            if (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl)
                databaseSuffix = "QC";
            if (EnvironmentSettings.CurrentEnvironment == Environment.Production)
                databaseSuffix = "";

            ConnectionString += databaseSuffix;

            DatabaseName = DatabaseName + databaseSuffix;

            try
            {
                var credentials = DatabaseCredentialsFactory.GetCredentialsFor(DatabaseName,
                    EnvironmentSettings.SecurityType);


                var settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                //settings.ClusterConfigurator = builder => builder.Subscribe(new Log4NetMongoEvents());

                //settings.Credential = credentials;

                MongoClient = new MongoClient(settings);

                Database = MongoClient.GetDatabase(DatabaseName);

                FileAttachmentBucket = new GridFSBucket(Database, new GridFSBucketOptions()
                {
                    BucketName = "fileAttachments"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

    }
}
