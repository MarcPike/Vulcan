using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Models
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
