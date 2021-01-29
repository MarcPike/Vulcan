using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DateValues;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuoteQueryDateOptions
    {
        public string DateRange { get; set; } = "This Week";
    }
}