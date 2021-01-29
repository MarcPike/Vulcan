using System.Numerics;

namespace Vulcan.IMetal.Queries.Companies
{
    public class iMetalTotalOpenCompletedAndLargestResult
    {
        public long OpenInvoices { get; set; }
        public long CompletedInvoices { get; set; }
        public decimal LargestInvoice { get; set; }

    }
}