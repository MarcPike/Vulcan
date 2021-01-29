using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    [BsonIgnoreExtraElements]
    public class Compensation 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OldHrsId { get; set; }
        public byte[] MonthlyCompensation { get; set; }

        public byte[] AnnualSalary { get; set; }

        public byte[] CurrentCompensation { get; set; }
        public PropertyValueRef PayFrequencyType { get; set; }
        public byte[] EffectiveDate { get; set; }
        public PropertyValueRef CurrencyType { get; set; }
        public PropertyValueRef PayRateType { get; set; }

        public PropertyValueRef PayGradeType { get; set; } = PropertyBuilder
            .CreatePropertyValue("PayGradeType", "Type of Pay Grade", "Unspecified", "").AsPropertyValueRef();
        public PropertyValueRef IncreaseType { get; set; }

        public PropertyValueRef AwsEligible { get; set; } //= PropertyBuilder
            //.CreatePropertyValue("AwsEligible", "Is AWS Eligible", "Unknown", "").AsPropertyValueRef();

        public List<PayGradeHistory> PayGradeHistory { get; set; } = new List<PayGradeHistory>();
        public List<Bonus> BonusHistory { get; set; } = new List<Bonus>();

        public List<OtherCompensation> OtherCompensation { get; set; } = new List<OtherCompensation>();

        public List<BonusScheme> BonusScheme { get; set; } = new List<BonusScheme>();

        public List<CompensationHistory> CompensationHistory { get; set; } = new List<CompensationHistory>();

        //public List<KronosPayRuleHistory> KronosPayRuleHistory { get; set; } = new List<KronosPayRuleHistory>();

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? KronosPayRuleEffectiveDate { get; set; }
        //public string KronosPayRuleName { get; set; }
        public PropertyValueRef KronosPayRuleType { get; set; }


        public List<PremiumComp> PremiumComp { get; set; } = new List<PremiumComp>();
        public decimal BaseHours { get; set; } = 0;
        public byte[] PayRateAmount { get; set; }
        public byte[] PremiumRateFactor { get; set; }
        public byte[] QuarterlyCompensation { get; set; }
        public byte[] YearToDateCompensation { get; set; }
        public byte[] FiscalYearToDateCompensation { get; set; }

        public PropertyValueRef BonusEligible { get; set; } = PropertyBuilder
            .CreatePropertyValue("Bonus Eligible Type", "Is Bonus Eligible", "No", "Not Bonus Eligible")
            .AsPropertyValueRef();

        public Compensation()
        {
        }

    }
}
