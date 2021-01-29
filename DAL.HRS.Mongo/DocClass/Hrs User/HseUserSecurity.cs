using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
{
    [BsonIgnoreExtraElements]
    public class HseUserSecurity : ISupportLocationNameChangesNested
    {
        public ObjectId RoleId { get; set; } = ObjectId.Empty;

        //public SecurityRole.SecurityRole Role { get; set; }

        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();

        public List<LocationRef> MedicalLocations { get; set; } = new List<LocationRef>();

        public SecurityRole.SecurityRole GetRole()
        {
            if (RoleId == ObjectId.Empty) return null;

            return SecurityRole.SecurityRole.Helper.FindById(RoleId);
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            foreach (var item in Locations)
            {
                modified = item.ChangeOfficeName(locationId, newName, modified);
            }

            foreach (var item in MedicalLocations)
            {
                modified = item.ChangeOfficeName(locationId, newName, modified);
            }

            return modified;
        }
    }
}