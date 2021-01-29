using System;

namespace Vulcan.IMetal.Queries.Companies
{
    public class iMetalInvoice
    {
        public int Number { get; set; }
        public int ItemNumber { get; set; }
        public string CustomerOrderNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysToPay { get; set; }
        public int? DueDays { get; set; }
    }
}