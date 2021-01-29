using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Models
{
    public class QuoteRevisionModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string QuoteId { get; set; }
        public int RevisionId { get; set; }
        public string RevisionNotesForPdf { get; set; } = string.Empty;

        public QuoteRevisionModel()
        {
        }

        public QuoteRevisionModel(string application, string userId, CrmQuote quote)
        {
            Application = application;
            UserId = userId;
            QuoteId = quote.Id.ToString();
            RevisionId = 1;
            if (quote.Revisions.Any())
            {
                RevisionId = quote.Revisions.Max(x => x.Id) + 1;
            }
        }
    }
}
