using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhProduct : BaseDocument
    {
        public int OldHrsId { get; set; }
        public string ApplicationMethod { get; set; }
        public List<CoshhDisposalMethodRef> DisposalMethods { get; set; } = new List<CoshhDisposalMethodRef>();
        public List<CoshhFirstAidMeasureRef> FirstAidMeasures { get; set; } = new List<CoshhFirstAidMeasureRef>();
        public List<CoshhHazardTypeRef> HazardTypes { get; set; } = new List<CoshhHazardTypeRef>();
        public List<CoshhProductUsageRef> ProductUsages { get; set; } = new List<CoshhProductUsageRef>();
        public List<CoshhStorageMethodRef> StorageMethods { get; set; } = new List<CoshhStorageMethodRef>();
        public List<CoshhRiskAssessmentRef> RiskAssessments { get; set; } = new List<CoshhRiskAssessmentRef>();
        public List<CoshhProductGhsClassification> GhsClassifications { get; set; } = new List<CoshhProductGhsClassification>();
        public CoshhProductManufacturerRef Manufacturer { get; set; }
        public string Description { get; set; }
        public string DurationOfExposure { get; set; }
        public string GhsDescription { get; set; }
        public bool MsdsAvailable { get; set; }
        public PropertyValueRef ProductType { get; set; }
    }
}
