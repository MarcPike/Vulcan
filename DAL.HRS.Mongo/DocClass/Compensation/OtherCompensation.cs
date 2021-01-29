using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    public class OtherCompensation : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef CompensationType { get; set; }
        public byte[] EffectiveDate { get; set; }
        public byte[] EndDate { get; set; }
        public byte[] Comment { get; set; }
        public byte[] Amount { get; set; }
        public byte[] Annualized { get; set; } = Encryption.NewEncryption.Encrypt<decimal>(0);

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = CompensationType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            return modified;
        }
    }
}