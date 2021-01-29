using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DateValues
{
    public class DateValueItem
    {
        private static DateValue _dateValue = new DateValue();
        public string Name { get; set; }
        public DateRange DateRange { get; set; }

        public static List<DateValueItem> GetDateValues()
        {
            _dateValue = new DateValue();
            var result = new List<DateValueItem>()
            {
                new DateValueItem()
                {
                    Name = "This Year",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.ThisYearToDate.BegDate,
                        EndDate = _dateValue.ThisYearToDate.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "This Year to Date",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.ThisYearToDate.BegDate,
                        EndDate = _dateValue.Today.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Last Year",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.LastYearToDate.BegDate,
                        EndDate = _dateValue.LastYearToDate.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Last Year to Date",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.LastYearToDate.BegDate,
                        EndDate = _dateValue.Today.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Last Quarter",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.LastQuarter.BegDate,
                        EndDate = _dateValue.LastQuarter.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Last Quarter to Date",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.LastQuarter.BegDate,
                        EndDate = _dateValue.Today.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "This Quarter",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.ThisQuarter.BegDate,
                        EndDate = _dateValue.ThisQuarter.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "This Quarter to Date",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.ThisQuarter.BegDate,
                        EndDate = _dateValue.Today.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Next Quarter",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.NextQuarter.BegDate,
                        EndDate = _dateValue.NextQuarter.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "This Week",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.ThisWeek.BegDate,
                        EndDate = _dateValue.ThisWeek.EndDate
                    },
                },
                new DateValueItem()
                {
                    Name = "This Week to Date",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.ThisWeek.BegDate,
                        EndDate = _dateValue.Today.EndDate
                    },
                },
                new DateValueItem()
                {
                    Name = "Last Week",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.LastWeek.BegDate,
                        EndDate = _dateValue.LastWeek.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Last Week to Date",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.LastWeek.BegDate,
                        EndDate = _dateValue.Today.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Next Week",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.NextWeek.BegDate,
                        EndDate = _dateValue.NextWeek.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Yesterday",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.YesterDay.BegDate,
                        EndDate = _dateValue.YesterDay.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "Today",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.Today.BegDate,
                        EndDate = _dateValue.Today.EndDate
                    }
                },
                new DateValueItem()
                {
                    Name = "This Month",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.ThisMonth.BegDate,
                        EndDate = _dateValue.NextMonth.BegDate.AddDays(-1)
                    }
                },
                new DateValueItem()
                {
                    Name = "Last Month",
                    DateRange = new DateRange()
                    {
                        BegDate = _dateValue.LastMonth.BegDate,
                        EndDate = _dateValue.LastMonth.EndDate
                    }
                },
            };

            return result;

        }

    }
}