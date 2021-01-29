using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class EquipmentInvolved : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef EquipmentCode { get; set; }
        public PropertyValueRef EquipmentType { get; set; }
        public string Comments { get; set; }

        public override string ToString()
        {
            return $"Code: ({EquipmentCode?.Code}) Type: ({EquipmentType.Code} Comments: {Comments})";
        }

        public override int GetHashCode()
        {
            var hashValue = EquipmentCode?.Code ?? "" + EquipmentType?.Code ?? "" + Comments;
            return hashValue.GetHashCode();
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = EquipmentCode.ChangeOfficeName(locationId, newName, modified);
            modified = EquipmentType.ChangeOfficeName(locationId, newName, modified);

            return modified;
        }
    }
}