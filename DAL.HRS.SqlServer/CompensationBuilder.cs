using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.SqlServer.Model;
using NUnit.Framework;

namespace DAL.HRS.SqlServer
{

    [TestFixture]
    public class CompensationBuilder
    {
        private HrsContext _context = new HrsContext();
        private int _compensationId = 0;
        public CompensationModelHrs Data;

        [Test]
        public void TestEmployee()
        {
            var employee = _context.Employee.SingleOrDefault(x => x.LastName == "Liu" && x.FirstName == "Mei ling");
            if (employee != null)
                GetBaseCompensationHrs(employee.OID);

            //foreach (var compensationModelHrse in Data)
            //{
                Console.WriteLine(ObjectDumper.Dump(Data));
            //}
        }

        [Test]
        public void TestDavidMeyer()
        {
            GetBaseCompensationHrs(17);
        }

        [Test]
        public void TestOtherCompensation()
        {
            GetBaseCompensationHrs(295);
            Console.WriteLine(ObjectDumper.Dump(Data));
        }

        [Test]
        public void GetBaseCompensationHrs(int employeeId)
        {
            Data = null;

            var payFrequencyTypes = _context.PayFrequencyType.AsNoTracking().ToList();

            Dictionary<int, string> payFrequencies = new Dictionary<int, string>()
            {
                { 0, "Bi-Weekly"},
                { 1, "Monthly"},
                { 2, "Annually" },
                { 3, "Quarterly"},
                { 4, "Semi-Monthly"}
            };

            var comp = _context.Compensation.FirstOrDefault(x => x.Employee == employeeId && x.GCRecord == null);
            if (comp == null) return;

            _compensationId = comp.OID;

            var model = new CompensationModelHrs
            {
                OldHrsId = comp.Employee ?? -1,
                CompensationId =  comp.OID,
                MonthlyCompensation = comp.MonthlyCompensation,
                CurrentCompensation = comp.CurrentCompensation,
                PayRateType = Enum.GetName(typeof(PayRateEnum), comp.PayRateType ?? 5),
                EffectiveDate = comp.EffiectiveDate,
                CurrencyType = comp.CurrencyType1?.Symbol ?? "?",
                BaseHours = comp.BaseHoursType1?.BaseHours ?? 0,
                PayRateAmount = comp.PayRateAmount,
                PremiumRateFactor = comp.PremiumRateFactor,
                QuarterlyCompensation = comp.QuarterlyCompensation,
                YearToDateCompensation = comp.YearToDateCompensation,
                KronosPayRuleType = comp.KronosPayRule1?.Name ?? String.Empty,
                KronosPayRuleEffectiveDate = comp.KronosPayRuleEffectiveDate,
            };

            model.SetOtherCompensation(_context, _compensationId);

            model.PayFrequencyType = payFrequencies.FirstOrDefault(x => x.Key == comp.PayFrequencyType).Value;

            model.IncreaseType = comp.IncreaseType1?.Name ?? string.Empty;
            model.PayGradeType = "Unspecified";

            model.SetCompensationHistory(_context, _compensationId);
            model.SetBonusHistory(_context, _compensationId);
            model.SetPayGradeHistories(_context, _compensationId);
            model.SetBonusScheme(_context, _compensationId, payFrequencyTypes);
            model.SetPremiumComp(_context, _compensationId);

            Data = model;

        }
    }


    public class CompensationModelHrs
    {
        private Encryption _encryption = Encryption.NewEncryption;

        public Guid Id { get; set; } = Guid.NewGuid();
        public int CompensationId { get; set; }
        public int OldHrsId { get; set; }
        public byte[] MonthlyCompensation { get; set; }
        public byte[] CurrentCompensation { get; set; }
        public string PayFrequencyType { get; set; }
        public byte[] EffectiveDate { get; set; }
        public string CurrencyType { get; set; }
        public string PayRateType { get; set; }
        public string PayGradeType { get; set; }
        public string IncreaseType { get; set; }

        public List<PayGradeHistoryHrs> PayGradeHistory { get; set; } = new List<PayGradeHistoryHrs>();
        public List<BonusHrs> BonusHistory { get; set; } = new List<BonusHrs>();
        public List<KronosPayRuleHistoryHrs> KronosPayRuleHistory { get; set; } = new List<KronosPayRuleHistoryHrs>();

        public string KronosPayRuleType { get; set; }
        public DateTime? KronosPayRuleEffectiveDate { get; set; }

        public List<OtherCompensationHrs> OtherCompensation { get; set; } = new List<OtherCompensationHrs>();

        public List<BonusSchemeHrs> BonusScheme { get; set; } = new List<BonusSchemeHrs>();

        public List<PremiumCompHrs> PremiumComp { get; set; } = new List<PremiumCompHrs>();
        public List<CompensationHistoryHrs> CompensationHistory { get; set; } = new List<CompensationHistoryHrs>();
        public decimal BaseHours { get; set; }
        public byte[] PayRateAmount { get; set; }
        public byte[] PremiumRateFactor { get; set; }
        public byte[] QuarterlyCompensation { get; set; }
        public byte[] YearToDateCompensation { get; set; }

        public void SetCompensationHistory(HrsContext context, int compId)
        {
            foreach (var compensationHistory in context.CompensationHistory.Where(x=>x.Compensation == compId).ToList())
            {
                if (compensationHistory.CreatedOn == null)
                {
                    //var message = "here";
                }

                try
                {
                    CompensationHistory.Add(new CompensationHistoryHrs()
                    {
                        ActualIncreaseAmount = compensationHistory.ActualIncreaseAmount ?? Encryption.NewEncryption.Encrypt(-1),
                        AnnualSalary = compensationHistory.AnnualSalary ?? Encryption.NewEncryption.Encrypt(-1),
                        CreatedOn = compensationHistory.CreatedOn ?? _encryption.Encrypt(DateTime.Parse("1/1/1900")),
                        EffectiveDate = compensationHistory.EffectiveDate,
                        IncreaseType = compensationHistory.IncreaseType1?.Name ?? String.Empty,
                        PayFrequencyType = context.PayFrequencyType.FirstOrDefault(x => x.OID == compensationHistory.PayFrequencyType)?.Name ?? "?",
                        PayGradeType = compensationHistory.PayGradeType1?.Name ,
                        
                        PayGradeMinimum = compensationHistory.PayGradeType1?.Minimum ?? Encryption.NewEncryption.Encrypt(-1),
                        PayGradeMaximum = compensationHistory.PayGradeType1?.Maximum ?? Encryption.NewEncryption.Encrypt(-1),
                        PayRateAmount = compensationHistory.PayRateAmount ?? Encryption.NewEncryption.Encrypt(-1),
                        PercentOfIncrease = compensationHistory.PercentOfIncrease ?? Encryption.NewEncryption.Encrypt(-1)

                    });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                
            }
        }


        public void SetBonusHistory(HrsContext context, int compId)
        {
            foreach (var bonus in context.Bonus.Where(x=>x.Compensation == compId).ToList())
            {
                BonusHistory.Add(new BonusHrs(context, bonus.DatePaid, bonus.Amount, bonus.BonusType, bonus.PercentPaid, bonus.FiscalYear, bonus.Comment));
            }
        }

        public void SetPayGradeHistories(HrsContext context, int compId)
        {
            foreach (var payGradeHistory in context.PayGradeHistory.Where(x=>x.Compensation == compId))
            {
                PayGradeHistory.Add(new PayGradeHistoryHrs(payGradeHistory.Minimum, payGradeHistory.Maximum, payGradeHistory.CreatedOn));
            }
        }

        public void SetOtherCompensation(HrsContext context, int compId)
        {
            foreach (var otherCompensation in context.OtherCompensation.Where(x=>x.Compensation == compId).ToList())
            {
                
                OtherCompensation.Add(new OtherCompensationHrs()
                {
                    CompensationType = otherCompensation.OtherCompensationType1.Name,
                    EffectiveDate = otherCompensation.EffectiveDate,
                    EndDate = otherCompensation.EndDate,
                    Comment = otherCompensation.Comment,
                    Amount = otherCompensation.Amount

                });
                
            }
        }

        public void SetBonusScheme(HrsContext context, int compId, List<PayFrequencyType> payFrequencyTypes)
        {
            foreach (var bonusScheme in context.BonusScheme.Include("PayFrequencyType").Where(x=>x.Compensation == compId).ToList())
            {
                var payFrequencyType = payFrequencyTypes.FirstOrDefault(x => x.OID == bonusScheme.PayFrequency)?.Name ?? "Unknown";

                BonusScheme.Add(new BonusSchemeHrs()
                {
                    BonusSchemeType = bonusScheme.BonusSchemeType1.Name,
                    Comment = bonusScheme.Comment,
                    EffectiveDate = bonusScheme.EffectiveDate ,
                    EndDate = bonusScheme.EndDate,
                    PayFrequencyType = payFrequencyType,
                    TargetPercentage = bonusScheme.TargetPercentageType?.Value,
                });
            }
        }

        public void SetPremiumComp(HrsContext context, int compId)
        {
            foreach (var premiumCompensation in context.PremiumCompensation.Where(x=>x.Compensation == compId).ToList())
            {
                PremiumComp.Add(new PremiumCompHrs()
                {
                    PremiumCompensationType = premiumCompensation.PremiumCompensationType1.Name,
                    ValueType = premiumCompensation.PremiumCompensationValueType1.Name,
                    Branch = premiumCompensation.PremiumCompensationValueType1.Location1.Branch1.Name,
                    OvertimeRateFactor = premiumCompensation.PremiumCompensationValueType1.OvertimeRateFactor ?? 0,
                    DoubleOvertimeRateFactor = premiumCompensation.PremiumCompensationValueType1.DoubleOvertimeRateFactor ?? 0,
                    Value = premiumCompensation.PremiumCompensationValueType1.Value,
                    Comment = premiumCompensation.Comment,

                });
            }
        }
    }

    public class CompensationHistoryHrs
    {
        public byte[] ActualIncreaseAmount { get; set; } 
        public byte[] AnnualSalary { get; set; }
        public byte[] CreatedOn { get; set; }
        public byte[] EffectiveDate { get; set; }
        public string IncreaseType { get; set; }
        public string PayFrequencyType { get; set; }
        public byte[] PayGradeType { get; set; }
        public byte[] PayGradeMinimum { get; set; }
        public byte[] PayGradeMaximum { get; set; }
        public byte[] PayRateAmount { get; set; }
        public byte[] PercentOfIncrease { get; set; }
    }

    public class PremiumCompHrs
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string PremiumCompensationType { get; set; }
        public string ValueType { get; set; }
        public string Branch { get; set; }
        public decimal OvertimeRateFactor { get; set; }
        public decimal DoubleOvertimeRateFactor { get; set; }
        public decimal? Value { get; set; }
        public byte[] Comment { get; set; }
    }

    public class BonusSchemeHrs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string BonusSchemeType { get; set; }
        public string Comment { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PayFrequencyType { get; set; }
        public float? TargetPercentage { get; set; }
    }

    public class KronosPayRuleHistoryHrs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime? KronosPayRuleEffectiveDate { get; set; }
        public string KronosPayRuleName { get; set; }

    }

    public class OtherCompensationHrs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CompensationType { get; set; }
        public byte[] EffectiveDate { get; set; }
        public byte[] EndDate { get; set; }
        public byte[] Comment { get; set; }
        public byte[] Amount { get; set; }
    }

    public class BonusHrs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DatePaid { get; set; }
        public decimal Amount { get; set; }
        public string BonusType { get; set; }
        public decimal PercentPaid { get; set; }
        public int FiscalYear { get; set; }
        public string Comment { get; set; }

        public BonusHrs()
        {

        }

        public BonusHrs(HrsContext context, DateTime? datePaid, decimal? amount, int? bonusType, decimal? percentPaid, int? fiscalYear,
            string comment)
        {
            DatePaid = datePaid ?? new DateTime(1980, 1, 1);
            Amount = amount ?? 0;
            bonusType = bonusType ?? -1;
            BonusType = context.BonusType.FirstOrDefault(x => x.OID == bonusType)?.Name ?? "?";
            PercentPaid = percentPaid ?? 0;
            FiscalYear = fiscalYear ?? 1980;
            Comment = comment;
        }

    }

    public class PayGradeHistoryHrs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PayGradeType { get; set; }
        public byte[] Minimum { get; set; }
        public byte[] Maximum { get; set; }
        public byte[] CreateDateTime { get; set; }

        public PayGradeHistoryHrs()
        {
        }

        public PayGradeHistoryHrs(byte[] min, byte[] max, byte[] createdOn)
        {
            PayGradeType = "Unspecified";
            Minimum = min;
            Maximum = max;
            CreateDateTime = createdOn;
        }
    }

    public enum PayRateValueTypeEnum
    {
        Amount,
        Percent
    }

    public enum PayRateEnum
    {
        Hourly,
        Monthly,
        BiWeekly,
        SemiMonthly,
        Unknown
    }

}
