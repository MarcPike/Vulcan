using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using MongoDB.Bson.Serialization.Attributes;
using System;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.DocClass.Discipline
{
    public class Discipline : ISupportLocationNameChangesNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public EmployeeRef Employee { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateOfAction { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateOfActionAppeals { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
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

        public bool Locked { get; set; } = true;

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = Location?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = Employee?.Location.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = Manager?.Location.ChangeOfficeName(locationId, newName, modified) ?? modified;

            modified = DisciplinaryActionType?.ChangeOfficeName(locationId, newName, modified) ?? modified;
            modified = EmployeeInAgreement?.ChangeOfficeName(locationId, newName, modified) ?? modified; ;
            modified = NatureOfViolationType?.ChangeOfficeName(locationId, newName, modified) ?? modified; ;
            modified = RepresentativePresent?.ChangeOfficeName(locationId, newName, modified) ?? modified; ;

            return modified;
        }
    }

}
