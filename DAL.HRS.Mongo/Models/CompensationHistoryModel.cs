using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class CompensationHistoryModel : BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; }
        public PropertyValueRef IncreaseType { get; set; }
        public PropertyValueRef PayFrequencyType { get; set; }
        public PropertyValueRef PayGradeType { get; set; }
        public decimal PayGradeMinimum { get; set; }
        public decimal PayGradeMaximum { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal PercentOfIncrease { get; set; }
        public decimal PayRateAmount { get; set; }
        public decimal AnnualSalary { get; set; }
        public decimal ActualIncreaseAmount { get; set; }
        public decimal AnnualAdjustmentAmount { get; set; }

        public override string ToString()
        {
            var nullString = "null";
            return
                $"Id: {Id} Increase: {IncreaseType?.Code ?? nullString} PayFreq: {PayFrequencyType?.Code ?? nullString} PayGrade: {PayGradeType?.Code ?? nullString} PayGrade Min {PayGradeMinimum} Max {PayGradeMaximum} Created: {CreatedOn.ToShortDateString()} Effect: {EffectiveDate.ToShortDateString()} Percent: {PercentOfIncrease} PayRateAmt: {PayRateAmount} Annual: {AnnualSalary} Actual: {ActualIncreaseAmount} Annual Adj: {AnnualAdjustmentAmount}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, IncreaseType);
            LoadCorrectPropertyValueRef(entity, PayFrequencyType);
            LoadCorrectPropertyValueRef(entity, PayGradeType);
        }

        public CompensationHistoryModel()
        {
        }

        public CompensationHistoryModel(CompensationHistory c)
        {
            var enc = Encryption.NewEncryption;

            ActualIncreaseAmount = decimal.Round(enc.Decrypt<decimal>(c.ActualIncreaseAmount),2,MidpointRounding.AwayFromZero);
            AnnualSalary = decimal.Round(enc.Decrypt<decimal>(c.AnnualSalary),2,MidpointRounding.AwayFromZero);
            PayRateAmount = decimal.Round(enc.Decrypt<decimal>(c.PayRateAmount),2,MidpointRounding.AwayFromZero);
            PercentOfIncrease = decimal.Round(enc.Decrypt<decimal>(c.PercentOfIncrease),2,MidpointRounding.AwayFromZero);
            CreatedOn = enc.Decrypt<DateTime>(c.CreatedOn).ToUniversalTime();
            EffectiveDate = enc.Decrypt<DateTime>(c.EffectiveDate).Date;
            Id = c.Id.ToString();
            IncreaseType = c.IncreaseType;
            PayFrequencyType = c.PayFrequencyType;
            PayGradeMinimum = decimal.Round(enc.Decrypt<decimal>(c.PayGradeMinimum),2,MidpointRounding.AwayFromZero);
            PayGradeMaximum = decimal.Round(enc.Decrypt<decimal>(c.PayGradeMaximum),2,MidpointRounding.AwayFromZero);
            PayGradeType = c.PayGradeType;
            AnnualAdjustmentAmount = decimal.Round(enc.Decrypt<decimal>(c.AnnualAdjustmentAmount), 2, MidpointRounding.AwayFromZero);

            //UpdatePropertyReferences.Execute(this);



        }

    }
}