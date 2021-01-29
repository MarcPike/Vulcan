using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class ProductBucket: BaseDocument
    {
        public string Coid { get; set; } = string.Empty;
        public List<string> ProductCategories { get; set; } = new List<string>();

        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)]
        public AndOrValue CategoryConditionAndOrValue { get; set; } = AndOrValue.And;

        public List<string> ProductConditions { get; set; } = new List<string>();
        public List<string> IgnoreWarehouseCodes { get; set; } = new List<string>();
        public List<string> ShowOnlyWarehouseCodes { get; set; } = new List<string>();

        public string Name { get; set; } = string.Empty;
    }
}
