using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Companies
{
    public class CompanyUpdateHistory
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public string UpdateReason { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public CompanyUpdateHistory(string fieldName, string oldValue, string newValue)
        {
            UpdateReason = $"{fieldName} Original Value [{oldValue}] was changed to [{newValue}]";
            FieldName = fieldName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}