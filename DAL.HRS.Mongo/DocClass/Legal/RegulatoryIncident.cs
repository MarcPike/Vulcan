using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.DocClass.Legal
{
    [BsonIgnoreExtraElements]
    public class RegulatoryIncident : BaseDocument
    {
        public static MongoRawQueryHelper<RegulatoryIncident> Helper = new MongoRawQueryHelper<RegulatoryIncident>();
        public int OldHrsId { get; set; } = 0;
        public int IncidentId { get; set; }
        public EmployeeRef Complainant { get; set; }
        public string ComplainantThirdParty { get; set; }
        public EmployeeRef ReportedTo { get; set; }
        public string  ReportedToThirdParty { get; set; }
        public EmployeeRef Accused { get; set; }

        public string AccusedThirdParty { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]

        public DateTime? IncidentDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]

        public DateTime? ReportedOnDate { get; set; }
        public PropertyValueRef Status { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef IncidentType { get; set; }
        public PropertyValueRef RegulatoryType { get; set; }
        public string IncidentDescription { get; set; }
        public List<RegulatoryIncidentInvestigator> InvestigatedBy { get; set; } = new List<RegulatoryIncidentInvestigator>();
        public List<RegulatoryIncidentWitness> Interviews { get; set; } = new List<RegulatoryIncidentWitness>();
        public string ImmediateAction { get; set; }
        public string CorrectiveAction { get; set; }
        public string PreventativeAction { get; set; }
        public string Recommendation { get; set; }
        public string Conclusion { get; set; }
        public string FollowUp { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ActionsTakenAccuserNotifiedDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ActionsTakenAccusedNotifiedDate { get; set; }

        public EmployeeRef ApprovedBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ApprovedByDate { get; set; }

        public bool IIQCompanyPolicyViolated { get; set; }
        public string IIQCompanyPolicyViolatedComment { get; set; }
        public bool IIQDisciplinaryActionTaken { get; set; }
        public string IIQDisciplinaryActionTakenComment { get; set; }
        public bool IIQAccusedWasInvolved { get; set; }
        public string IIQAccusedWasInvolvedComment { get; set; }
        public bool IIQAccusedNamedInPriorReportsPastYear { get; set; }
        public string IIQAccusedNamedInPriorReportsPastYearComment { get; set; }
        public bool IIQBenefitsAwarded { get; set; }
        public string IIQBenefitsAwardedComment { get; set; }
        public bool IIQEventWitness { get; set; }
        public string IIQEventWitnessComment { get; set; }

        //public List<SupportingDocument> IncidentDocuments { get; set; } = new List<SupportingDocument>();

        public RegulatoryIncident() 
        { 

        }
    }
}
