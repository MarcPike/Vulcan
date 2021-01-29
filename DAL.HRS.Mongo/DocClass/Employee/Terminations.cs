using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class Termination: ISupportLocationNameChangesNested
    {
        public PropertyValueRef TerminationCode { get; set; }
        public string TerminationExplanation { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]

        public DateTime TerminationDate { get; set; }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = TerminationCode.ChangeOfficeName(locationId, newName, modified);
            return modified;
        }
    }
}
