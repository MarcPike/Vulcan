using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using System;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public class BenefitsGridModel
    {
        public string EmployeeId { get; set; }
        public PropertyValueRef GenderCode { get; set; }
        public int Age { get; set; }
        public DateTime? HireDate { get; set; }
        public JobTitleRef JobTitle { get; set; }
        public EmployeeRef Manager { get; set; }

        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }
        public bool IsActive { get; set; }

        public BenefitsGridModel(Employee e)
        {
            EmployeeId = e.Id.ToString();
            GenderCode = e.GenderCode;
            Age = e.GetAge();
            HireDate = e.OriginalHireDate;
            JobTitle = e.JobTitle;
            Manager = e.Manager;

            PayrollId = e.PayrollId;
            LastName = e.LastName;
            FirstName = e.FirstName;
            MiddleName = e.MiddleName;
            PreferredName = e.PreferredName;
            CostCenterCode = e.CostCenterCode;
            Location = e.Location;
            IsActive = (e.TerminationDate == null || e.TerminationDate > DateTime.Now);
            KronosDepartmentCode = e.KronosDepartmentCode;
        }

        public BenefitsGridModel(BaseGridModel m)
        {
            EmployeeId = m.Id.ToString();
            GenderCode = m.GenderCode;
            Age = m.GetAge();
            HireDate = m.HireDate;
            JobTitle = m.JobTitle;
            Manager = m.Manager.Refresh();
            PayrollId = m.PayrollId;
            LastName = m.LastName;
            FirstName = m.FirstName;
            MiddleName = m.MiddleName;
            PreferredName = m.PreferredName;
            CostCenterCode = m.CostCenterCode;
            Location = m.Location;
            IsActive = m.IsActive;
            KronosDepartmentCode = m.KronosDepartmentCode;

        }

    }
}
