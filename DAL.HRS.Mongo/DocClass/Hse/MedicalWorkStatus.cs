using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class MedicalWorkStatus : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef Type { get; set; }
        public byte[] Date { get; set; } 
        public byte[] Comments { get; set; }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = Type.ChangeOfficeName(locationId, newName, modified);
            return modified;
        }

        public override string ToString()
        {
            var enc = Encryption.NewEncryption;
            var missingCode = "(missing code)";
            var missingDate = "(missing date)";
            var date = enc.Decrypt<DateTime?>(Date);
            var dateValue = missingDate;
            if (date != null) dateValue = date.ToString();

            return $"Type: {Type?.Code ?? missingCode} on Date: {dateValue}";
        }

    }
}