namespace Vulcan.IMetal.Queries.Companies
{
    public class iMetalCompanyCreditTotalResults
    {
        public decimal TotalOpen { get; set; }
        public decimal TotalDue { get; set; }
        public decimal TotalDue30 { get; set; }
        public decimal TotalDue60 { get; set; }
        public decimal TotalDue90 { get; set; }
        public decimal TotalDue120 { get; set; }
        public decimal TotalDueOver120 { get; set; }
    }
}