using System;
using PropertyValueRef = HRS.Web.Client.CSharp.Referenced_Classes.PropertyValueRef;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class Discipline 
    {
        public Guid Id { get; set; }
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

        public bool Locked { get; set; } = true;

    }

}
