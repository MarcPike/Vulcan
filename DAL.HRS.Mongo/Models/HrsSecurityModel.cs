using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class HrsSecurityModel
    {
           
            public SecurityRoleModel Role { get; set; } = null;

            public bool DirectReportsOnly { get; set; } = true;

            public bool HasCompensation { get; set; } = false;

            public List<LocationRef> MedicalLocations { get; set; } = new List<LocationRef>();
            public List<LocationRef> Locations { get; set; } = new List<LocationRef>();

            public List<PayrollRegionRef> PayrollRegionsForCompensation { get; set; } = new List<PayrollRegionRef>();

            public HrsSecurityModel()
            {
            }

            public HrsSecurityModel(HrsUserSecurity sec)
            {
                var role = sec.GetRole();
                if (role == null) return;
                Role = new SecurityRoleModel(role);
                HasCompensation = sec.HasCompensation;
                Locations = sec.Locations.OrderBy(x=>x.Office).ToList();
                PayrollRegionsForCompensation = sec.PayrollRegionsForCompensation;
                MedicalLocations = sec.MedicalLocations.OrderBy(x => x.Office).ToList();
            }
    }
}
