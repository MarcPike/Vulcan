using System;
using System.Diagnostics;
using DAL.Vulcan.Mongo.DateValues;
using DAL.Vulcan.Test;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Date_Time
{
    [TestFixture]
    public class DateTimeTests
    {
        [Test]
        public void DateValuesTest()
        {
            var values = DateValueItem.GetDateValues();
            Console.WriteLine(ObjectDumper.Dump(values));
        }

        [Test]
        public void DateValues()
        {

            var dateValues = new DateValue(DateTime.Now);

            Console.WriteLine($"Today={dateValues.Today.BegDate} to {dateValues.Today.EndDate}");

            Console.WriteLine($"ThisYearToDate={dateValues.ThisYearToDate.BegDate} to {dateValues.ThisYearToDate.EndDate}");
            Console.WriteLine($"LastYearToDate={dateValues.LastYearToDate.BegDate} to {dateValues.LastYearToDate.EndDate}");
            Console.WriteLine($"ThisMonth={dateValues.ThisMonth.BegDate} to {dateValues.ThisMonth.EndDate}");
            Console.WriteLine($"ThisMonth={dateValues.ThisMonth.BegDate} to {dateValues.ThisMonth.EndDate}");
            Console.WriteLine($"LastMonth={dateValues.LastMonth.BegDate} to {dateValues.LastMonth.EndDate}");
            Console.WriteLine($"NextMonth={dateValues.NextMonth.BegDate} to {dateValues.NextMonth.EndDate}");


            var thisYearToDate = dateValues.GetDateRangeFor("This Year to Date");
            Console.WriteLine($"This Year to Date={thisYearToDate.BegDate} to {thisYearToDate.EndDate}");

            var lastYearToDate = dateValues.GetDateRangeFor("Last Year to Date");
            Console.WriteLine($"Last Year to Date={lastYearToDate.BegDate} to {lastYearToDate.EndDate}");

        }
    }
}
