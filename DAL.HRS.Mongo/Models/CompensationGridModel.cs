using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.DocClass;
using PayrollRegionRef = DAL.Common.DocClass.PayrollRegionRef;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public struct CompensationGridModel
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
        public decimal BaseHours { get; }
        public PropertyValueRef PayGradeType { get; }

        //public PropertyValueRef PayGradeType { get; set; }
        public PayrollRegionRef PayrollRegion { get; set; }
        //public decimal BaseHours { get; set; }
        public PropertyValueRef BusinessRegionCode { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }
        public PropertyValueRef Status1Code { get; set; }


        public CompensationGridModel(BaseGridModel m)
        {
            //var employee = new RepositoryBase<Employee>().Find(m.EmployeeId);

            EmployeeId = m.Id.ToString();
            PayrollId = m.PayrollId;
            LastName = m.LastName;
            FirstName = m.FirstName;
            MiddleName = m.MiddleName;
            PreferredName = m.PreferredName;
            GovernmentId = m.GovernmentId;
            CostCenterCode = m.CostCenterCode;
            Location = m.Location;
            IsActive = m.IsActive;
            Manager = m.Manager.Refresh();
            JobTitle = m.JobTitle;
            BaseHours = m.BaseHours;
            PayGradeType = m.PayGradeType;
            //var compensation = employee.Compensation;

            //PayGradeType = compensation?.PayGradeType;
            PayrollRegion = m.PayrollRegion;
            //BaseHours = compensation?.BaseHours ?? 0;

            BusinessRegionCode = m.BusinessRegionCode;
            KronosDepartmentCode = m.KronosDepartmentCode;
            Status1Code = m.Status1Code;


        }

    }
}