namespace DAL.Vulcan.Mongo.Core.Models
{
    public class RfqCustomerModel
    {
        public string Id { get; set; }
        public int RequestForQuoteId { get; set; } 
        public string ContactEmailAddress { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string RfqText { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}