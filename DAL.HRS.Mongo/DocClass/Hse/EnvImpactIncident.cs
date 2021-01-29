using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.DocClass;
using System;
using System.Collections.Generic;
using DAL.Common.DocClass;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class EnvImpactIncident : BaseDocument
    {
        public int OldHrsId { get; set; }
        public PropertyValueRef Status { get; set; }
        public bool Osha { get; set; }
        public bool Riddor { get; set; }
        public bool EmployeeInvoked { get; set; }
        public EmployeeRef RecordedBy { get; set; }
        public EmployeeRef Employee { get; set; }
        public EmployeeRef Manager { get; set; }
        public DateTime? IncidentDate { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef SeverityType { get; set; }
        public string SeverityTypeNotes { get; set; }
        public string WorkBeingPerformed { get; set; }
        public string DescriptionOfEvent { get; set; }
        public string CorrectiveAction { get; set; }
        public string Recommendation { get; set; }
        public EmployeeRef ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool OneDayNotificationSent { get; set; }
        public bool TenDayNotificationSent { get; set; }
        public List<EnvImpactIncidentInvestigator> Investigators { get; set; } = new List<EnvImpactIncidentInvestigator>();
        public List<EnvImpactIncidentWitness> Witnesses { get; set; } = new List<EnvImpactIncidentWitness>();
        public List<EnvImpactJobFactor> JobFactors = new List<EnvImpactJobFactor>();
        public List<EnvImpactEvent> ImpactEvents = new List<EnvImpactEvent>();
        public List<EnvImpactLackOfControl> LackOfControl = new List<EnvImpactLackOfControl>();
        public List<EnvImpactNatureOfEvent> NatureOfEvent = new List<EnvImpactNatureOfEvent>();
        public List<EnvImpactPersonalFactor> PersonalFactors = new List<EnvImpactPersonalFactor>();
        public List<EnvImpactRootCause> RootCauses = new List<EnvImpactRootCause>();
        public List<EnvImpactImpactType> ImpactTypes = new List<EnvImpactImpactType>();

    }
}
