using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Properties;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class BonusSchemeModel : BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; }

        public PropertyValueRef BonusSchemeType { get; set; }
        public PropertyValueRef PayFrequencyType { get; set; }
        public float? TargetPercentage { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public string Comment { get; set; }

        public BonusSchemeModel()
        {
        }

        public override string ToString()
        {
            var nullString = "null";
            return
                $"Id: {Id} Type: {BonusSchemeType?.Code ?? nullString} PayFreq: {PayFrequencyType?.Code ?? nullString} Target%: {TargetPercentage} Effective: {EffectiveDate?.ToShortDateString() ?? nullString} End: {EndDate?.ToShortDateString() ?? nullString} Comments: {Comment}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, BonusSchemeType);
            LoadCorrectPropertyValueRef(entity, PayFrequencyType);
        }

        public BonusSchemeModel(BonusScheme s)
        {
            BonusSchemeType = s.BonusSchemeType;
            Comment = s.Comment;
            EffectiveDate = s.EffectiveDate?.Date;
            EndDate = s.EndDate?.Date;
            PayFrequencyType = s.PayFrequencyType;
            TargetPercentage = s.TargetPercentage;
            Id = s.Id.ToString();

            //UpdatePropertyReferences.Execute(this);

        }

    }
}