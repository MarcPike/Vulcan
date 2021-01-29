using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Core.DateValues
{
    public class DateValue
    {
        private DateTime _dateOf;
        private int _year;
        private int _month;
        private int _dayOfMonth;
        private int _dayOfWeek;
        private int _quarterOfYear;

        private readonly List<YearQuarter> Quarters = new List<YearQuarter>();
        private readonly List<YearMonth> Months  = new List<YearMonth>();
        private readonly List<YearWeek> Weeks  = new List<YearWeek>();

        public YearQuarter LastQuarter { get; private set; }
        public YearQuarter ThisQuarter { get; private set; }
        public YearQuarter NextQuarter { get; private set; }
        public DateRange ThisYearToDate { get; private set; }
        public DateRange LastYearToDate { get; private set; }
        public DateRange ThisMonthToDate { get; private set; }
        public YearMonth NextMonth { get; set; }
        public YearMonth LastMonth { get; set; }
        public YearMonth ThisMonth { get; set; }
        public YearWeek NextWeek { get; set; }
        public YearWeek LastWeek { get; set; }
        public YearWeek ThisWeek { get; set; }
        public DateRange Tomorrow { get; set; } //= new DateRange() {BegDate = DateTime.Now.Date.AddDays(1), EndDate = DateTime.Now.Date.AddDays(2).AddSeconds(-1)};
        public DateRange YesterDay { get; set; } //= new DateRange() { BegDate = DateTime.Now.Date.AddDays(-1), EndDate = DateTime.Now.Date.AddSeconds(-1) };
        public DateRange Today { get; set; } //= new DateRange() { BegDate = DateTime.Now.Date, EndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1) };

        public static List<string> GetDateValueOptions()
        {

            return new List<string>()
            {
                "This Year to Date",
                "Last Year to Date",
                "This Month to Date",
                "Last Quarter",
                "This Quarter",
                "Next Quarter",
                "Next Month",
                "Last Month",
                "This Month",
                "Next Week",
                "Last Week",
                "This Week",
                "Tomorrow",
                "Today"
            };
        }

        public DateRange GetDateRangeFor(string name)
        {


            if (name == "This Year to Date")
            {
                return new DateRange() {BegDate = ThisYearToDate.BegDate, EndDate = Today.EndDate};
            }
            else if (name == "Last Year to Date")
            {
                return new DateRange() {BegDate = LastYearToDate.BegDate, EndDate = Today.EndDate};
            }
            else if (name == "Last Quarter")
            {
                return new DateRange() {BegDate = LastQuarter.BegDate, EndDate = LastQuarter.EndDate};
            }
            else if (name == "This Quarter")
            {
                return new DateRange() { BegDate = ThisQuarter.BegDate, EndDate = ThisQuarter.EndDate };
            }
            else if (name == "Last Quarter")
            {
                return new DateRange() { BegDate = LastQuarter.BegDate, EndDate = LastQuarter.EndDate };
            }
            else if (name == "Next Quarter")
            {
                return new DateRange() { BegDate = NextQuarter.BegDate, EndDate = NextQuarter.EndDate };
            }
            else if (name == "Last Month")
            {
                return new DateRange() { BegDate = LastMonth.BegDate, EndDate = LastMonth.EndDate };
            }
            else if (name == "This Month")
            {
                return new DateRange() { BegDate = ThisMonth.BegDate, EndDate = ThisMonth.EndDate };
            }
            else if (name == "Next Month")
            {
                return new DateRange() { BegDate = NextMonth.BegDate, EndDate = NextMonth.EndDate };
            }
            else if (name == "Last Week")
            {
                return new DateRange() { BegDate = LastWeek.BegDate, EndDate = LastWeek.EndDate };
            }
            else if (name == "This Week")
            {
                return new DateRange() { BegDate = ThisWeek.BegDate, EndDate = ThisWeek.EndDate };
            }
            else if (name == "Next Week")
            {
                return new DateRange() { BegDate = NextWeek.BegDate, EndDate = NextWeek.EndDate };
            }
            else if (name == "Yesterday")
            {
                return YesterDay;
            }
            else if (name == "Today")
            {
                return Today;
            }
            else if (name == "Tomorrow")
            {
                return Tomorrow;
            }
            else return new DateRange() {BegDate = DateTime.Now, EndDate = DateTime.Now};
        }

        public DateValue(DateTime dateOf)
        {
            _dateOf = dateOf;
            ComputeValues();
        }

        public DateValue()
        {
            _dateOf = DateTime.Now;
            ComputeValues();
        }

        private void ComputeValues()
        {
            _year = _dateOf.Year;
            _month = _dateOf.Month;
            _dayOfMonth = _dateOf.Day;
            _dayOfWeek = (int) _dateOf.DayOfWeek;
            _quarterOfYear = GetCurrentQuarter();

            Today = new DateRange() { BegDate = _dateOf.Date, EndDate = _dateOf };
            YesterDay = new DateRange() { BegDate = _dateOf.Date.AddDays(-1), EndDate = _dateOf.Date.AddDays(-1)};
            Tomorrow = new DateRange() { BegDate = _dateOf.Date.AddDays(1), EndDate = _dateOf.Date.AddDays(1)};

            GetQuarters();
            GetMonths();
            GetWeeks();

            ThisQuarter = Quarters.First(x => x.Year == _year && _dateOf >= x.BegDate && _dateOf <= x.EndDate);

            var indexOfThisQuarter = Quarters.IndexOf(ThisQuarter);

            LastQuarter = Quarters[indexOfThisQuarter - 1];
            NextQuarter = Quarters[indexOfThisQuarter + 1];

            ThisYearToDate = new DateRange()
            {
                BegDate = DateTime.Parse("1/1/" + _year),
                EndDate = _dateOf.Date
            };
            LastYearToDate = new DateRange()
            {
                BegDate = ThisYearToDate.BegDate.AddYears(-1),
                EndDate = _dateOf.Date
            };


            ThisMonth = Months.First(x => x.Year == _year && _dateOf >= x.BegDate && _dateOf <= x.EndDate);

            ThisMonthToDate = new DateRange() { BegDate = ThisMonth.BegDate , EndDate = Today.EndDate };

            var indexOfThisMonth = Months.IndexOf(ThisMonth);
            LastMonth = Months[indexOfThisMonth - 1];
            NextMonth = Months[indexOfThisMonth + 1];


            ThisWeek = Weeks.First(x => x.Year == _year && _dateOf >= x.BegDate && _dateOf <= x.EndDate);
            var indexOfThisWeek = Weeks.IndexOf(ThisWeek);
            LastWeek = Weeks[indexOfThisWeek - 1];
            NextWeek = Weeks[indexOfThisWeek + 1];

        }

        private void GetWeeks()
        {
            Weeks.AddRange(YearWeek.GetWeeksForYear(_year - 1));
            Weeks.AddRange(YearWeek.GetWeeksForYear(_year));
            Weeks.AddRange(YearWeek.GetWeeksForYear(_year + 1));
        }

        private void GetMonths()
        {
            Months.AddRange(YearMonth.GetMonthsForYear(_year - 1));
            Months.AddRange(YearMonth.GetMonthsForYear(_year));
            Months.AddRange(YearMonth.GetMonthsForYear(_year + 1));
        }

        private void GetQuarters()
        {
            Quarters.AddRange(YearQuarter.GetYearQuarters(_year - 1));
            Quarters.AddRange(YearQuarter.GetYearQuarters(_year));
            Quarters.AddRange(YearQuarter.GetYearQuarters(_year + 1));
        }


        private int GetCurrentQuarter()
        {
            if (_dateOf.Month >= 4 && _dateOf.Month <= 6)
                return 1;
            else if (_dateOf.Month >= 7 && _dateOf.Month <= 9)
                return 2;
            else if (_dateOf.Month >= 10 && _dateOf.Month <= 12)
                return 3;
            else
                return 4;
        }

    }

    public class DateRange
    {
        private DateTime _endDate;
        private DateTime _begDate;

        public DateTime BegDate
        {
            get
            {
                var result = _begDate.Date;
                return result;
            }
            set => _begDate = value;
        }

        public DateTime EndDate
        {
            get
            {
                var result = _endDate.Date;
                result = result.AddHours(23);
                result = result.AddMinutes(59);
                result = result.AddSeconds(59);
                return result;

            }
            set => _endDate = value;
        }
    }

    public class YearQuarter : DateRange
    {
        public int Year { get; set; }
        public int Quarter { get; set; }

        public static List<YearQuarter> GetYearQuarters(int year)
        {
            var result = new List<YearQuarter>
            {
                new YearQuarter()
                {
                    Year = year,
                    Quarter = 1,
                    BegDate = DateTime.Parse($"1/1/{year}"),
                    EndDate = DateTime.Parse($"3/31/{year}")
                },
                new YearQuarter()
                {
                    Year = year,
                    Quarter = 2,
                    BegDate = DateTime.Parse($"4/1/{year}"),
                    EndDate = DateTime.Parse($"6/30/{year}")
                },
                new YearQuarter()
                {
                    Year = year,
                    Quarter = 3,
                    BegDate = DateTime.Parse($"7/1/{year}"),
                    EndDate = DateTime.Parse($"9/30/{year}")
                },
                new YearQuarter()
                {
                    Year = year,
                    Quarter = 4,
                    BegDate = DateTime.Parse($"10/1/{year}"),
                    EndDate = DateTime.Parse($"12/31/{year}")
                }
            };

            return result;
        }

        public static List<YearQuarter> GetYearQuarters(DateTime dateOf)
        {
            var year = dateOf.Year;

            return YearQuarter.GetYearQuarters(year);
        }

    }

    public class YearMonth : DateRange
    {
        public int Year { get; private set; }
        public int Month { get; private set; }

        public static List<YearMonth> GetMonthsForYear(int year)
        {
            var months = new List<YearMonth>();

            var onMonth = DateTime.Parse("1/1/" + year);
            while (onMonth.Year == year) 
            {
                var yearMonth = new YearMonth()
                {
                    Year = year,
                    Month = onMonth.Month,
                    BegDate = onMonth,
                    EndDate = onMonth.AddMonths(1).AddDays(-1)
                };
                months.Add(yearMonth);
                onMonth = onMonth.AddMonths(1);
            }

            return months;
        }
    }

    public class YearWeek : DateRange
    {
        public int Year { get; set; }
        public int Week { get; set; }

        public static List<YearWeek> GetWeeksForYear(int year)
        {
            var weeks = new List<YearWeek>();
            int week = 0;
            var onWeek = DateTime.Parse("1/1/" + year);
            while (onWeek.Year == year)
            {
                ++week;
                var dayOfWeek = (int)onWeek.DayOfWeek;
                var lastDayOfWeek = onWeek.AddDays(6 - dayOfWeek);
                var yearWeek = new YearWeek()
                {
                    Year = year,
                    Week = week,
                    BegDate = onWeek,
                    EndDate = lastDayOfWeek
                };
                weeks.Add(yearWeek);
                onWeek = lastDayOfWeek.AddDays(1);
            }

            return weeks;
        }
    }

}
