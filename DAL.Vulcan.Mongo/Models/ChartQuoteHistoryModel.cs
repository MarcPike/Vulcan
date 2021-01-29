using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Models
{
    public class ChartQuoteHistoryModel
    {
        public List<ChartQuoteHistoryItem> Draft { get; set; } = new List<ChartQuoteHistoryItem>();
        public List<ChartQuoteHistoryItem> Active { get; set; } = new List<ChartQuoteHistoryItem>();
        public List<ChartQuoteHistoryItem> Won { get; set; } = new List<ChartQuoteHistoryItem>();
        public List<ChartQuoteHistoryItem> Lost { get; set; } = new List<ChartQuoteHistoryItem>();
        public List<ChartQuoteHistoryItem> Expired { get; set; } = new List<ChartQuoteHistoryItem>();

        private decimal DraftTotal => Draft.Sum(x => x.Y);
        private decimal ActiveTotal => Active.Sum(x => x.Y);
        private decimal WonTotal => Won.Sum(x => x.Y);
        private decimal LostTotal => Lost.Sum(x => x.Y);
        private decimal ExpiredTotal => Expired.Sum(x => x.Y);

        public List<SalesPersonTotal> SalesPersonTotals { get; set; } = new List<SalesPersonTotal>();
        public List<StatusTotals> StatusTotals { get; set; } = new List<StatusTotals>();

        public List<MaterialChartSummary> MaterialHistoryTotals { get; set; } = new List<MaterialChartSummary>();
        public List<MaterialChartSummaryDrilldown> MaterialHistoryTotalsDrilldown { get; set; } = new List<MaterialChartSummaryDrilldown>();

        public List<CustomerChartSummary> CustomerSummary { get; set; } = new List<CustomerChartSummary>();

        public void CalculateStatusTotals()
        {
            StatusTotals.Clear();
            StatusTotals.Add(new StatusTotals() {Name = "Draft", Y = DraftTotal});
            StatusTotals.Add(new StatusTotals() { Name = "Active", Y = ActiveTotal});
            StatusTotals.Add(new StatusTotals() { Name = "Won", Y = WonTotal });
            StatusTotals.Add(new StatusTotals() { Name = "Lost", Y = LostTotal });
            StatusTotals.Add(new StatusTotals() { Name = "Expired", Y = ExpiredTotal });
        }

        public void AddSalesPersonTotal(string salesPersonName, decimal total)
        {
            var salesPersonTotal = SalesPersonTotals.SingleOrDefault(x => x.Name == salesPersonName);
            if (salesPersonTotal == null)
            {
                salesPersonTotal = new SalesPersonTotal() {Name = salesPersonName, Y = total};
                SalesPersonTotals.Add(salesPersonTotal);
            }
            else
            {
                salesPersonTotal.Y += total;
            }

        }
    }

    public class CustomerChartSummary
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public decimal Y { get; set; }
    }

    public class MaterialChartSummary
    {
        public string Drilldown => Name;
        public string Name { get; set; }
        public decimal Y { get; set; }
    }

    public class MaterialChartSummaryDrilldown
    {
        public string Id { get; set; }
        public List<MaterialDrilldownValue> Data { get; set; } = new List<MaterialDrilldownValue>();
    }

    public class MaterialDrilldownValue
    {
        public string Name { get; set; }
        public decimal Y { get; set; }
    }

    public class StatusTotals
    {
        public string Name { get; set; }
        public decimal Y { get; set; }
    }

    public class SalesPersonTotal
    {
        public string Name { get; set; }
        public decimal Y { get; set; }
    }

    public class ChartQuoteHistoryItem
    {
        public string Name { get; set; }
        public long X { get; set; }
        public decimal Y { get; set; }

        public static long GetUnixDate(DateTime date)
        {
            date = date.ToUniversalTime();
            long unixTime = ((DateTimeOffset) date).ToUnixTimeMilliseconds();
            //Int32 unixTimestamp = (Int32)(date.Subtract(new DateTime(1970, 1, 1))).Milliseconds;
            return unixTime;
        }
    }
}
