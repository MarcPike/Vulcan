using System.Collections.Generic;
using DAL.Vulcan.Mongo.DateValues;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuoteQueryDateOptions
    {
        public string DateRange { get; set; } = "This Week";
    }
}