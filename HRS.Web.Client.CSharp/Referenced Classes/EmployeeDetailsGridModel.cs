namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class EmployeeDetailsGridModel
    {
        public string EmployeeId { get; set; }
        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public string GovernmentId { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }
        public LocationRef Location { get; set; }
        public EmployeeRef Manager { get; set; }
        public bool IsActive { get; set; }
        public JobTitleRef JobTitle { get; set; }
        public PropertyValueRef BusinessRegionCode { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }
        public PropertyValueRef Status1Code { get; set; }
        public PayrollRegionRef PayrollRegion { get; set; }
        public int Age { get; set; } 
        public int OldHrsId { get; set; }
        public LdapUserRef LdapUser { get; set; }
        public PropertyValueRef CompanyNumber { get; set; }

        public PropertyValueRef BusinessUnit { get; set; }



    }
}
