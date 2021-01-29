using System;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuoteQueryExecutionHistory
    {
        public DateTime ExecutedOn { get; set; } = DateTime.Now;
        public DateTime? CompletedOn { get; set; } = null;

        public TimeSpan Duration => CompletedOn != null ? (CompletedOn - ExecutedOn).Value : new TimeSpan(0,0,0);

        public int RowCount { get; set; } = 0;
        public Exception Error { get; set; } = null;
    }
}