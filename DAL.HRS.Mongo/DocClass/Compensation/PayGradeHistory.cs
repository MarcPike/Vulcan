using DAL.HRS.Mongo.DocClass.Properties;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    public class PayGradeHistory: ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef PayGradeType { get; set; }
        public byte[] Minimum { get; set; }
        public byte[] Maximum { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public byte[] CreateDateTime { get; set; }
        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = PayGradeType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            return modified;
        }
    }
}