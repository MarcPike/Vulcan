using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using PayrollRegionRef = DAL.Common.DocClass.PayrollRegionRef;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeDetailsGridModel: EmployeeBasedModel
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
        public int Age { get; set; } = 0;
        public int OldHrsId { get; set; }
        public LdapUserRef LdapUser { get; set; }

        public EmployeeDetailsGridModel()
        {
        }

        public EmployeeDetailsGridModel(Employee emp)
        {
            EmployeeId = emp.Id.ToString();
            PayrollId = emp.PayrollId;
            LastName = emp.LastName;
            FirstName = emp.FirstName;
            MiddleName = emp.MiddleName;
            PreferredName = emp.PreferredName;
            GovernmentId = _encryption.Decrypt<string>(emp.GovernmentId) ?? string.Empty;
            CostCenterCode = emp.CostCenterCode;
            Location = emp.Location;
            IsActive = (emp.TerminationDate == null || emp.TerminationDate > DateTime.Now);
            Manager = emp.Manager;
            JobTitle = emp.JobTitle;
            PayrollRegion = emp.PayrollRegion;


            BusinessRegionCode = emp.BusinessRegionCode;
            KronosDepartmentCode = emp.KronosDepartmentCode;
            Status1Code = emp.Status1Code;
            Age = emp.GetAge();
            OldHrsId = emp.OldHrsId;
            LdapUser = emp.LdapUser;

        }

        public EmployeeDetailsGridModel(BaseGridModel m)
        {
            EmployeeId = m.Id.ToString();
            PayrollId = m.PayrollId;
            LastName = m.LastName;
            FirstName = m.FirstName;
            MiddleName = m.MiddleName;
            PreferredName = m.PreferredName;
            GovernmentId = m.GovernmentId ?? string.Empty;
            CostCenterCode = m.CostCenterCode;
            Location = m.Location;
            IsActive = m.IsActive;
            Manager = m.Manager;
            JobTitle = m.JobTitle;
            PayrollRegion = m.PayrollRegion;

            BusinessRegionCode = m.BusinessRegionCode;
            KronosDepartmentCode = m.KronosDepartmentCode;
            Status1Code = m.Status1Code;
            Age = m.GetAge();
            OldHrsId = m.OldHrsId;
            LdapUser = m.LdapUser;


        }

    }
}
