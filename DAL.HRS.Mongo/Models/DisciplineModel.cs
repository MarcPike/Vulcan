using DAL.HRS.Mongo.DocClass.Discipline;
using DAL.HRS.Mongo.DocClass.Employee;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public class DisciplineModel: BaseModel, IHavePropertyValues
    {
        public string Id { get; set; } 
        public EmployeeRef Employee { get; set; }
        public DateTime? DateOfAction { get; set; }
        public DateTime? DateOfActionAppeals { get; set; }
        public DateTime? DateOfViolation { get; set; }
        public PropertyValueRef DisciplinaryActionType { get; set; }
        public PropertyValueRef EmployeeInAgreement { get; set; }
        public string EmployeeStatement { get; set; }
        public EmployeeRef Manager { get; set; }
        public string ManagerStatement { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef NatureOfViolationType { get; set; }
        public string RepresentativeName { get; set; }
        public PropertyValueRef RepresentativePresent { get; set; }
        //public List<DisciplineHistoryModel> DisciplineHistory { get; set; } = new List<DisciplineHistoryModel>();
        public bool Locked { get; set; }

        public bool IsDirty { get; set; } = false;
        public HrsUserRef ModifiedBy { get; set; }

        public override string ToString()
        {
            var nullString = "null";
            return
                $"Id: {Id} DateOfAction: {DateOfAction?.ToShortDateString() ?? nullString} DateOfAppeals: {DateOfActionAppeals?.ToShortDateString() ?? nullString} DateOfViolation: {DateOfViolation?.ToShortDateString() ?? nullString} Action: {DisciplinaryActionType?.Code ?? nullString} EmpAgreement: {EmployeeInAgreement?.Code ?? nullString} EmpStatement: {EmployeeStatement} Mgr: {Manager.ToString()} Mgr Statement: {ManagerStatement} Location: {Location?.Office ?? nullString} NatureViolation: {NatureOfViolationType?.Code ?? nullString} RepName: {RepresentativeName} Rep Present: {RepresentativePresent?.Code ?? nullString}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, DisciplinaryActionType);
            LoadCorrectPropertyValueRef(entity, EmployeeInAgreement);
            LoadCorrectPropertyValueRef(entity, NatureOfViolationType);
            LoadCorrectPropertyValueRef(entity, RepresentativePresent);
        }

        public DisciplineModel()
        {
        }

        public DisciplineModel(EmployeeRef emp, Discipline disp)
        {
            EntityRef entity;
            var employee = emp.AsEmployee();
            if (disp == null)
            {
                disp = new Discipline()
                {
                    Employee = emp,
                    Location = employee.Location,
                    Locked = false,
                    Manager = employee.Manager,
                };
            }

            Id = disp.Id.ToString();
            Employee = emp;
            DateOfAction = disp.DateOfAction;
            DateOfActionAppeals = disp.DateOfActionAppeals;
            DateOfViolation = disp.DateOfViolation;
            DisciplinaryActionType = disp.DisciplinaryActionType;
            EmployeeInAgreement = disp.EmployeeInAgreement;
            EmployeeStatement = disp.EmployeeStatement;
            ManagerStatement = disp.ManagerStatement;
            Location = disp.Location;
            NatureOfViolationType = disp.NatureOfViolationType;
            RepresentativeName = disp.RepresentativeName;
            RepresentativePresent = disp.RepresentativePresent;
            Locked = disp.Locked;
            Manager = disp.Manager;

            LoadPropertyValuesWithThisEntity(employee.Entity);

        }
    }
}
