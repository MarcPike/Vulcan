using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Base.Core.Filters
{
    public class BaseFilter<TBaseDocument>
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;

        public virtual FilterDefinition<TBaseDocument> ToFilterDefinition()
        {
            return Builders<TBaseDocument>.Filter.Empty; // new BsonDocument()

        }
    }
}