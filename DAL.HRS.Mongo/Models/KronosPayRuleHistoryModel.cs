using DAL.HRS.Mongo.DocClass.Properties;
using System;

namespace DAL.HRS.Mongo.Models
{
    public class KronosPayRuleHistoryModel
    {

        public string Id { get; set; }
        public PropertyValueRef KronosPayRuleType { get; set; }
        public DateTime? KronosPayRuleEffectiveDate { get; set; }

        public KronosPayRuleHistoryModel()
        {
        }

        public KronosPayRuleHistoryModel(DAL.HRS.Mongo.DocClass.Compensation.KronosPayRuleHistory h)
        {
            KronosPayRuleEffectiveDate = h.KronosPayRuleEffectiveDate;
            Id = h.Id.ToString();
            KronosPayRuleType = h.KronosPayRuleType;
        }
    }
}