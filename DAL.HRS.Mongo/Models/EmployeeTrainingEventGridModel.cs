using DAL.HRS.Mongo.DocClass.Employee;
using System;
using DAL.Common.DocClass;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public struct EmployeeTrainingEventGridModel
    {
        public DateTime Birthday { get; set; }
        public PropertyValueRef BusinessRegionCode { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }
        //public string Id { get; set; }
        public string FirstName { get; set; }
        public PropertyValueRef GenderCode { get; set; }
        public string GovernmentId { get; set; }
        public DateTime? OriginalHireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public JobTitleRef JobTitle { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }
        public string LastName { get; set; }
        public LocationRef Location { get; set; }
        public EmployeeRef Manager { get; set; }
        public string MiddleName { get; set; }
        public string PayrollId { get; set; }
        public PayrollRegionRef PayrollRegion { get; set; }
        public string PreferredName { get; set; }
        public PropertyValueRef Status1Code { get; set; }
        public bool IsActive { get; set; }
        public int TrainingEventsCount { get; set; }
        public string EmployeeId { get; set; }
    }
}
