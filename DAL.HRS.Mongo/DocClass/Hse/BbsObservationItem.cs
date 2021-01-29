using System;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsObservationItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public BbsInfluenceBehaviorTypeRef InfluenceBehaviorType { get; set; }
        public PropertyValueRef InfluenceOnBehaviorType { get; set; }
        public PropertyValueRef BbsPrecautionType { get; set; }
        public int ConcernCount { get; set; } = 0;
        public string ReasonForConcernWhat { get; set; } = string.Empty;
        public string ReasonForConcernWhy { get; set; } = string.Empty;
        public PropertyValueRef atRiskActivitiesObserved { get; set; }
        public int SafeCount { get; set; } = 0;

    }
}