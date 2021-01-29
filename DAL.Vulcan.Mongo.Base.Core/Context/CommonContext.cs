using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace DAL.Vulcan.Mongo.Base.Core.Context
{
    public class CommonContext : ContextBase
    {
        public CommonContext()
        {

            ConnectionString = @"mongodb://S-US-MDB02:27017/Common";

            //if (EnvironmentSettings.Database == MongoDatabase.VulcanBI)
            //{
            //    ConnectionString = @"mongodb://S-US-MDB03:27017/Common";

            //}

            DatabaseName = "Common";


            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);

            //ConnectionString = Settings.Default.ConnectionString;

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
