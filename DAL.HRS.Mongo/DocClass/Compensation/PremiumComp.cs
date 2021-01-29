using DAL.HRS.Mongo.DocClass.Properties;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    public class PremiumComp : ISupportLocationNameChangesNested
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef PremiumCompensationType { get; set; }
        public PropertyValueRef ValueType { get; set; }
        public string Branch { get; set; }
        public decimal OvertimeRateFactor { get; set; }
        public decimal DoubleOvertimeRateFactor { get; set; }
        public decimal? Value { get; set; }
        public byte[] Comment { get; set; }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = PremiumCompensationType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = ValueType?.ChangeOfficeName(locationId, newName, modified) ?? modified;

            return modified;

        }
    }
}