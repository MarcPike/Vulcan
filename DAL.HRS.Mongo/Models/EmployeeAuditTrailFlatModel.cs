using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeAuditTrailFlatModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Module { get; set; } = string.Empty;
        public EmployeeRef Employee { get; set; }

        public bool IsActive
        {
            get
            {
                if (Employee == null) return false;

                var helper = DocClass.Employee.Employee.Helper;
                var project =
                    helper.ProjectionBuilder.Expression(x =>
                                new {x.OriginalHireDate, x.TerminationDate});
                var employeeId = ObjectId.Parse(Employee.Id);
                var filter = helper.FilterBuilder.Where(x => x.Id == employeeId);

                var dates = helper.FindWithProjection(filter, project).FirstOrDefault();
                if (dates == null) return false;


                //var maxDate = (dates.TerminationDate == null) ? DateTime.Parse("12/31/2100") : dates.TerminationDate;
                var maxDate = (dates.TerminationDate == null) ? DateTime.ParseExact("31/12/2100", "dd/mm/yyyy", null) : dates.TerminationDate;
                var minDate = (dates.OriginalHireDate == null) ? maxDate : dates.OriginalHireDate;

                return DateTime.Now >= minDate && DateTime.Now <= maxDate;
            }
        }

        public string NewValue { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string ListName { get; set; } = string.Empty;
        public string ListAction { get; set; } = string.Empty;
        public string ListValues { get; set; } = string.Empty;
        public HrsUserRef UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static List<string> LimitEmployeeDetailFieldsTo = new List<string>()
            {
                "Manager",
                "PayrollId",
                "GovernmentId",
                "WorkAreaCode",
                "Location",
                "TerminationDate",
                "Status1Code",
                "JobTitle",
                "CostCenter",
                "CostCenterCode"
            };

        public EmployeeAuditTrailFlatModel()
        {
            
        }

        public static List<EmployeeAuditTrailFlatModel> FlattenEmployeeAuditTrail(EmployeeAuditTrailModel audit, string module)
        {
            var result = new List<EmployeeAuditTrailFlatModel>();

            if (!audit.AnyChanges()) return result;

            
            if ((module == "Benefits") && (audit.BenefitsChanges.AnyChanges()))
            {

                foreach (var change in audit.BenefitsChanges.ValueChanges)
                {
                    var newRow = GetNewRow("Benefits");
                    newRow.FieldName = change.FieldName;
                    newRow.OldValue = change.OldValue;
                    newRow.NewValue = change.NewValue;
                    result.Add(newRow);
                }
                foreach (var change in audit.BenefitsChanges.ListChanges)
                {
                    var newRow = GetNewRow("Benefits");
                    newRow.ListName = change.ListName;
                    newRow.ListAction = change.Action;
                    newRow.ListValues = change.ListValues;
                    result.Add(newRow);
                }
            }

            if ((module == "Compensation") && (audit.CompensationChanges.AnyChanges()))
            {
                foreach (var change in audit.CompensationChanges.ValueChanges)
                {
                    var newRow = GetNewRow("Compensation");
                    newRow.FieldName = change.FieldName;
                    newRow.OldValue = change.OldValue;
                    newRow.NewValue = change.NewValue;
                    result.Add(newRow);
                }
                foreach (var change in audit.CompensationChanges.ListChanges)
                {
                    var newRow = GetNewRow("Compensation");
                    newRow.ListName = change.ListName;
                    newRow.ListAction = change.Action;
                    newRow.ListValues = change.ListValues;
                    result.Add(newRow);
                }
            }

            if ((module == "Discipline") && (audit.DisciplineChanges.AnyChanges()))
            {
                foreach (var change in audit.DisciplineChanges.ValueChanges)
                {
                    var newRow = GetNewRow("Discipline");
                    newRow.FieldName = change.FieldName;
                    newRow.OldValue = change.OldValue;
                    newRow.NewValue = change.NewValue;
                    result.Add(newRow);
                }
                foreach (var change in audit.DisciplineChanges.ListChanges)
                {
                    var newRow = GetNewRow("Discipline");
                    newRow.ListName = change.ListName;
                    newRow.ListAction = change.Action;
                    newRow.ListValues = change.ListValues;
                    result.Add(newRow);
                }
            }

            if ((module == "EmployeeDetails") && (audit.EmployeeDetailsChanges.AnyChanges()))
            {

                foreach (var change in audit.EmployeeDetailsChanges.ValueChanges)
                {
                    if (LimitEmployeeDetailFieldsTo.All(x=>x != change.FieldName)) 
                        continue;

                    var newRow = GetNewRow("EmployeeDetails");
                    newRow.FieldName = change.FieldName;
                    newRow.OldValue = change.OldValue;
                    newRow.NewValue = change.NewValue;
                    result.Add(newRow);
                }
                //foreach (var change in audit.EmployeeDetailsChanges.ListChanges)
                //{
                //    var newRow = GetNewRow("EmployeeDetails");
                //    newRow.ListName = change.ListName;
                //    newRow.ListAction = change.Action;
                //    newRow.ListValues = change.ListValues;
                //    result.Add(newRow);
                //}
            }

            if ((module == "Performance") && (audit.PerformanceChanges.AnyChanges()))
            {
                foreach (var change in audit.PerformanceChanges.ValueChanges)
                {
                    var newRow = GetNewRow("Performance");
                    newRow.FieldName = change.FieldName;
                    newRow.OldValue = change.OldValue;
                    newRow.NewValue = change.NewValue;
                    result.Add(newRow);
                }
                foreach (var change in audit.PerformanceChanges.ListChanges)
                {
                    var newRow = GetNewRow("Performance");
                    newRow.ListName = change.ListName;
                    newRow.ListAction = change.Action;
                    newRow.ListValues = change.ListValues;
                    result.Add(newRow);
                }
            }

            return result;

            EmployeeAuditTrailFlatModel GetNewRow(string thisModule)
            {
                return new EmployeeAuditTrailFlatModel() {  Employee = audit.Employee, Module = thisModule, UpdatedAt = audit.UpdatedAt, UpdatedBy = audit.UpdatedBy };

            }
        }

    }
}