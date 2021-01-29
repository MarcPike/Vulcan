using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class LostQuoteModel
    {
        public string Application { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string QuoteId { get; set; } = string.Empty;
        public string LostReasonId { get; set; } = string.Empty;
        public string LostComments { get; set; } = string.Empty;

        public string Competitor { get; set; } = string.Empty;
        public List<string> QuoteItemIdList { get; set; } = new List<string>();
        public string LostProductCode { get; set; } = string.Empty;

    }
}
