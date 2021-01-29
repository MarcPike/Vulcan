namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuoteQueryQuoteOptions
    {
        public string RfqNumber { get; set; } = string.Empty;
        public string SalesPersonNotesContain { get; set; } = string.Empty;
        public string CustomerNotesContain { get; set; } = string.Empty;

        public bool IsUsed => (RfqNumber != string.Empty) || (SalesPersonNotesContain != string.Empty) ||
                              (CustomerNotesContain != string.Empty);
    }
}