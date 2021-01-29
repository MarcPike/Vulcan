using PayrollRegionRef = HRS.Web.Client.CSharp.Referenced_Classes.PayrollRegionRef;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class EmployeeRef
    {
        public string Id { get; set; }
        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public PayrollRegionRef PayrollRegion { get; set; }

        public string FullName { get; set; }

        public LocationRef Location { get; set;}


    }
}