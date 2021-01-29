using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PayrollRegionRef = DAL.Common.DocClass.PayrollRegionRef;

namespace DAL.HRS.Mongo.DocClass.Hrs_User
{
    [BsonIgnoreExtraElements]
    public class HrsUserSecurity : ISupportLocationNameChangesNested
    {
        public ObjectId RoleId { get; set; } = ObjectId.Empty;
        //public SecurityRole.SecurityRole Role { get; set; }

        public bool HasCompensation => CheckHasCompensation();

        public List<LocationRef> MedicalLocations { get; set; } = new List<LocationRef>();

        private bool CheckHasCompensation()
        {
            var role = GetRole();

            if (role == null) return false;

            return role.Modules.Any(x => x.ModuleType.Name == "Compensation");
        }


        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();

        public List<PayrollRegionRef> PayrollRegionsForCompensation { get; set; } = new List<PayrollRegionRef>();

        public SecurityRole.SecurityRole GetRole()
        {
            if (RoleId == ObjectId.Empty) return null;

            return SecurityRole.SecurityRole.Helper.FindById(RoleId);
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            foreach (var item in MedicalLocations)
            {
                modified = item.ChangeOfficeName(locationId, newName, modified);
            }

            foreach (var item in Locations)
            {
                modified = item.ChangeOfficeName(locationId, newName, modified);
            }

            return modified;

        }
    }
}
