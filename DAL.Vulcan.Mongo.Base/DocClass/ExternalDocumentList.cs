using System.Collections.Generic;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class ExternalDocumentList
    {
        public string Name { get; set; }
        public List<BsonDocument> Documents { get; set; } = new List<BsonDocument>();
    }
}