using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Models
{
    public class WonQuoteItemModel
    {
        public string Application { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string QuoteId { get; set; } = string.Empty;
        public List<string> QuoteItemIdList { get; set; } = new List<string>();

    }
}