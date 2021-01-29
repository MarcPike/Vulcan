using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Compensation;

namespace DAL.HRS.Mongo.Models
{
    public class BonusYtdModel
    {
        public int CurrentCalendarYear => DateTime.Now.Year;
        public decimal CurrentCalendarYearTotal { get; set; } = 0;

        public int CurrentFiscalYear
        {
            get
            {
                var result = CurrentCalendarYear;
                var date = DateTime.Now;
                if (date.Month < 4) --result;
                return result;
            }
        }

        public decimal CurrentFiscalYearTotal { get; set; } = 0;

        public List<YearTotal> CalendarYearTotals { get; set; } = new List<YearTotal>();
        public List<YearTotal> FiscalYearTotals { get; set; } = new List<YearTotal>();


        

        public BonusYtdModel()
        {
        }

        public BonusYtdModel(List<Bonus> bonusList)
        {
            CurrentCalendarYearTotal = bonusList.Where(x => x.CalendarYear == CurrentCalendarYear).Sum(x => x.Amount);
            CurrentFiscalYearTotal = bonusList.Where(x => x.FiscalYear == CurrentFiscalYear).Sum(x => x.Amount);

            var calendarYears = bonusList.Select(x => x.CalendarYear).Distinct().OrderByDescending(x=>x);
            foreach (var calendarYear in calendarYears)
            {
                CalendarYearTotals.Add(new YearTotal()
                {
                    Year = calendarYear,
                    Total = bonusList.Where(x=>x.CalendarYear == calendarYear).Sum(x=>x.Amount)
                });
            }

            var fiscalYears = bonusList.Select(x => x.FiscalYear).Distinct().OrderByDescending(x => x);
            foreach (var fiscalYear in fiscalYears)
            {
                FiscalYearTotals.Add(new YearTotal()
                {
                    Year = fiscalYear,
                    Total = bonusList.Where(x => x.FiscalYear == fiscalYear).Sum(x => x.Amount)
                });
            }

        }
    }

    public class YearTotal
    {
        public int Year { get; set; }
        public decimal Total { get; set; }
    }
}
