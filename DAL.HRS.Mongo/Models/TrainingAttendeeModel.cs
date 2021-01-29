using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Helpers;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingAttendeeModel
    {
        public string Id { get; set; }
        public EmployeeRef Employee { get; set; }
        public decimal Reimbursement { get; set; } = 0;
        public bool Dismissed { get; set; } = false;
        public DateTime? DateCompleted { get; set; }
        public RequiredActivityRef RequiredActivity { get; set; }

        public TrainingAttendeeModel()
        {
        }

        public TrainingAttendeeModel(TrainingAttendee t)
        {
            Id = t.Id.ToString();
            Employee = t.Employee;
            Reimbursement = t.Reimbursement;
            Dismissed = t.Dismissed;
            DateCompleted = t.DateCompleted;
        }

        public TrainingAttendeeModel(string employeeId)
        {
            var helperEmployee = new HelperEmployee();
            var emp = helperEmployee.GetEmployee(employeeId);
            if (emp == null) return;

            Employee = emp.AsEmployeeRef();
        }
    }
}
