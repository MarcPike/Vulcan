using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class RequiredActivityRef : ReferenceObject<RequiredActivity>
    {
        public EmployeeRef Employee { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo        public RequiredActivityType Type { get; set; }
        public RequiredActivityStatus Status { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo        public RequiredActivityType Type { get; set; }
        public RequiredActivityType Type { get; }

        public RequiredActivityRef()
        {
        }

        public RequiredActivityRef(RequiredActivity r)
        {
            Id = r.Id.ToString();
            Employee = r.Employee;
            Status = r.Status;
            Type = r.Type;
        }

        public RequiredActivity AsRequiredActivity()
        {
            return ToBaseDocument();
        }

    }
}