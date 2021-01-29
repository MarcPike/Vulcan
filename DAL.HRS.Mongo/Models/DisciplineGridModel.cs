using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public class DisciplineGridModel: EmployeeBasedModel
    {

        public string EmployeeId { get; set; }
        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public PropertyValueRef CostCenter { get; set; }

        public string CostCenterDescription => CostCenter?.AsPropertyValue()?.Description ?? "Unknown";

        public LocationRef Location { get; set; }
        public bool IsActive { get; set; }


        public DisciplineGridModel()
        {
        }

        public DisciplineGridModel(Employee employee)
        {
            PayrollId = employee.PayrollId;
            LastName = employee.LastName;
            FirstName = employee.FirstName;
            CostCenter = employee.CostCenterCode;
            Location = employee.Location;
            IsActive = (employee.TerminationDate == null || employee.TerminationDate > DateTime.Now);
            EmployeeId = employee.Id.ToString();
            CostCenter = employee.CostCenterCode;
            //UpdatePropertyReferences.Execute(this);

        }


        public DisciplineGridModel(BaseGridModel m)
        {
            PayrollId = m.PayrollId;
            LastName = m.LastName;
            FirstName = m.FirstName;
            CostCenter = m.CostCenterCode;
            Location = m.Location;
            IsActive = m.IsActive;
            EmployeeId = m.Id.ToString();
            CostCenter = m.CostCenterCode;

        }


    }
}
