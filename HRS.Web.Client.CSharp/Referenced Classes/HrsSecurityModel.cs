using System.Collections.Generic;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class HrsSecurityModel
    {
           
            public SecurityRoleModel Role { get; set; } = null;

            public bool DirectReportsOnly { get; set; } = true;

            public bool HasCompensation { get; set; } = false;

            public List<LocationRef> MedicalLocations { get; set; } 
            public List<LocationRef> Locations { get; set; } 

            public List<PayrollRegionRef> PayrollRegionsForCompensation { get; set; } 


    }
}
