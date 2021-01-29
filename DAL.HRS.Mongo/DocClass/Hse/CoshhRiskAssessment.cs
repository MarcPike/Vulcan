using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhRiskAssessment : BaseDocument
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? AssessmentDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? NextReviewDate { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef Department { get; set; }
        public string ProcessDescription { get; set; }
        public string WorkExposureLimits { get; set; }
        public bool IsExposureControlled { get; set; }
        public string IsExposureControlledComment { get; set; }
        public PropertyValueRef RiskRating { get; set; }
        public string RiskRatingComment { get; set; }
        public bool MaterialStoredProperly { get; set; }
        public string MaterialStoredProperlyComment { get; set; }
        public bool MaterialDisposedProperly { get; set; }
        public string MaterialDisposedProperlyComment { get; set; }
        public string PreparedBy { get; set; }
        public string ApprovedBy { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ApprovedOn { get; set; }

        public CoshhRiskAssessmentRef AsCoshhRiskAssessmentRef()
        {
            return new CoshhRiskAssessmentRef(this);
        }
    }

    public class CoshhRiskAssessmentRef : ReferenceObject<CoshhRiskAssessment>
    {
        public string ProcessDescription { get; set; }

        public CoshhRiskAssessmentRef()
        {
        }

        public CoshhRiskAssessmentRef(CoshhRiskAssessment assess) : base(assess)
        {
            ProcessDescription = assess.ProcessDescription;
        }

        public CoshhRiskAssessment AsCoshhRiskAssessment()
        {
            return ToBaseDocument();
        }
    }
}
