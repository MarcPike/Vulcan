using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Models
{
    public class QuoteCopyModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string QuoteId { get; set; } = string.Empty;
        public List<string> QuoteItemIdList { get; set; } = new List<string>();
        //public List<string> QuickQuoteItemIdList { get; set; } = new List<string>();

        public QuoteCopyModel()
        {

        }

        public QuoteCopyModel(string application, string userId, string quoteId)
        {
            Application = application;
            UserId = userId;
            QuoteId = quoteId;
        }
    }
}
