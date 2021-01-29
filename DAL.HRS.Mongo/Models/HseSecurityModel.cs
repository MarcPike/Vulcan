using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class HseSecurityModel
    {
        public SecurityRoleModel Role { get; set; }

        public bool DirectReportsOnly { get; set; } = false;


        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();

        public List<LocationRef> MedicalLocations { get; set; } = new List<LocationRef>();


        public HseSecurityModel()
        {
        }

        public HseSecurityModel(HseUserSecurity sec)
        {
            var role = sec.GetRole();
            if (role == null) return;

            Role = new SecurityRoleModel(role);
            Locations = sec.Locations.OrderBy(x=>x.Office).ToList();
            MedicalLocations = sec.MedicalLocations;
        }
    }
}