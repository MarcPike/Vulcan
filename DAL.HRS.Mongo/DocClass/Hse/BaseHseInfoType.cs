using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BaseHseInfoType : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PropertyValueRef Type { get; set; }
        public string Comments { get; set; }

        public BaseHseInfoType()
        {
        }

        public override string ToString()
        {
            return Type + ":" + Comments;
        }

        public override int GetHashCode()
        {
            var hashValue = Type?.Code ?? "" + Comments;
            return hashValue.GetHashCode();
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = Type?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            return modified;
        }
    }
}