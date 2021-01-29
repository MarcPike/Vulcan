using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    public class CompensationHistory 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public byte[] ActualIncreaseAmount { get; set; }
        public byte[] AnnualAdjustmentAmount { get; set; } = Encryption.NewEncryption.Encrypt(0);
        public byte[] AnnualSalary { get; set; }
        public byte[] CreatedOn { get; set; }
        public byte[] EffectiveDate { get; set; }
        public PropertyValueRef IncreaseType { get; set; }
        public PropertyValueRef PayFrequencyType { get; set; }
        public PropertyValueRef PayGradeType { get; set; }
        public byte[] PayGradeMinimum { get; set; }
        public byte[] PayGradeMaximum { get; set; }
        public byte[] PayRateAmount { get; set; }
        public byte[] PercentOfIncrease { get; set; }

    }
}