using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace DAL.Vulcan.Mongo.Base.Core.Context
{
    public class ContextBase
    {
        public IMongoDatabase Database;
        public MongoClient MongoClient;
        public string ConnectionString = @"mongodb://S-US-MDB01:27017/Vulcan";
        public string DatabaseName = "Vulcan";
        public GridFSBucket FileAttachmentBucket { get; set; }
    }
}