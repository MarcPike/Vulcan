using DAL.HRS.Mongo.DocClass.Employee;
using System;
using DAL.Common.DocClass;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public class DisciplineHistoryModel
    {
        public string Id { get; set; } 
        public DateTime? CreatedOn { get; set; }
        public DateTime? DateOfAction { get; set; }
        public DateTime? DateOfViolation { get; set; }
        public PropertyValueRef DisciplinaryActionType { get; set; }
        public PropertyValueRef EmployeeInAgreement { get; set; }
        public string EmployeeStatement { get; set; }
        public PropertyValueRef NatureOfViolationType { get; set; }
        public EmployeeRef Manager { get; set; }
        public LocationRef Location { get; set; }
        public string ManagerStatement { get; set; }

        public DisciplineHistoryModel()
        {
        }

        //public DisciplineHistoryModel(DisciplineHistory hist)
        //{
        //    Id = hist.Id.ToString();
        //    CreatedOn = hist.CreatedOn;
        //    DateOfAction = hist.DateOfAction;
        //    DateOfViolation = hist.DateOfViolation;
        //    DisciplinaryActionType = hist.DisciplinaryActionType;
        //    EmployeeInAgreement = hist.EmployeeInAgreement;
        //    EmployeeStatement = hist.EmployeeStatement;
        //    NatureOfViolationType = hist.NatureOfViolationType;
        //    Manager = hist.Manager;
        //    ManagerStatement = hist.ManagerStatement;
        //    Location = hist.Location;

        //    UpdatePropertyReferences.Execute(this);
        //}

        //public static List<DisciplineHistoryModel> ConvertList(List<DisciplineHistory> list)
        //{
        //    var result = new List<DisciplineHistoryModel>();

        //    if (list != null)
        //    {
        //        foreach (var disciplineHistory in list)
        //        {
        //            result.Add(new DisciplineHistoryModel(disciplineHistory));
        //        }
        //    }

        //    return result;
        //}
    }
}