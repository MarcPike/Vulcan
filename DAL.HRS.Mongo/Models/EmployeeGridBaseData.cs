using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeGridBaseData
    {
        public string EmployeeId { get; set; }
        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }

        public PropertyValueRef CostCenterCode { get; set; }
        public LocationRef Location { get; set; }
        public bool IsActive { get; set; }
        public PropertyValueRef JobTitle { get; set; }
        public string GovernmentId { get; set; }
        public EmployeeRef Manager { get; set; }



    }
}
