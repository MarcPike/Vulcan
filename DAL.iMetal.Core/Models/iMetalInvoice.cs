using System;

namespace DAL.iMetal.Core.Models
{
    public class iMetalInvoice
    {
        public string Currency { get; set; }
        public int Number { get; set; }
        public int ItemNumber { get; set; }
        public string CustomerOrderNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime DueDate { get; set; }
        public double DaysToPay { get; set; }
        public int? DueDays { get; set; }
    }
}