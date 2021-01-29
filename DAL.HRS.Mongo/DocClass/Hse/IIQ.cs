using System;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class IIQ 
    {
        public bool EmployeeAuthorized { get; set; } = false;
        public string EmployeeAuthorizedComment { get; set; }
        public bool EquipmentInDangerousPosition { get; set; }
        public string EquipmentInDangerousPositionComments { get; set; }
        public bool EquipmentPreChecksCompleted { get; set; }
        public string EquipmentPreChecksCompletedComments { get; set; }
        public bool OthersInAreaWarned { get; set; }
        public string OthersInAreaWarnedComments { get; set; }
        public bool RiskAssessmentAvailable { get; set; }
        public string RiskAssessmentAvailableComments { get; set; }
        public bool SafeWorkingProceduresFollowed { get; set; }
        public string SafeWorkingProceduresFollowedComments { get; set; }
        public bool UsingEquipmentCorrectly { get; set; }
        public string UsingEquipmentCorrectlyComments { get; set; }
        public bool UsingRequiredPPE { get; set; }
        public string UsingRequiredPPEComments { get; set; }

    }
}