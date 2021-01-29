using System;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeDrugTestModel
    {
        public string Id { get; set; }
        public PropertyValueRef DrugTestType { get; set; }
        public PropertyValueRef DrugTestResult { get; set; }
        public DateTime? TestDate { get; set; }
        public DateTime? ResultDate { get; set; }
        public string Comments { get; set; }

        public EmployeeDrugTestModel() { }

        public EmployeeDrugTestModel(EmployeeDrugTest d)
        {
            Id = d.Id.ToString();
            DrugTestType = d.DrugTestType;
            DrugTestResult = d.DrugTestResult;
            TestDate = d.TestDate;
            ResultDate = d.ResultDate;
            Comments = d.Comments;
        }

    }
}