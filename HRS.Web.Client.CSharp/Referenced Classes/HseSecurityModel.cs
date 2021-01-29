using System.Collections.Generic;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class HseSecurityModel
    {
        public SecurityRoleModel Role { get; set; }

        public bool DirectReportsOnly { get; set; } = false;


        public List<LocationRef> Locations { get; set; } 

        public List<LocationRef> MedicalLocations { get; set; } 


    }
}