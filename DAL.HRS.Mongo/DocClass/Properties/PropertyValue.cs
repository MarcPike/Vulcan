using System;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Properties
{
    [BsonIgnoreExtraElements]
    public class PropertyValue : BaseDocument
    {
        public string Type { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public EntityRef Entity { get; set; } 
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();

        public static MongoRawQueryHelper<PropertyValue> Helper = new MongoRawQueryHelper<PropertyValue>();

        public int CodeAsInteger => Int32.Parse(Regex.Replace(Code, "[^0-9.]", ""));

        public PropertyValue()
        {
        }

        public PropertyValue(PropertyType propertyType, string code, string description, string entityId, List<LocationRef> locations)
        {
            Type = propertyType.Type;
            Code = code;
            Description = description;
            Entity = new RepositoryBase<Entity>().Find(entityId).AsEntityRef();
            Locations = locations;
        }

        public PropertyValueRef AsPropertyValueRef()
        {
            return new PropertyValueRef(this);
        }

    }
}
