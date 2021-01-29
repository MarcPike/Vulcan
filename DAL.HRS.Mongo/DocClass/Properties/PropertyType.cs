using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Properties
{
    public class PropertyType : BaseDocument
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [BsonIgnore]
        public List<PropertyValue> PropertyValues
        {
            get { return new RepositoryBase<PropertyValue>().AsQueryable()
                .Where(x => x.Type == Type).ToList();
            }
        }

        public static MongoRawQueryHelper<PropertyType> Helper = new MongoRawQueryHelper<PropertyType>();
    }
}