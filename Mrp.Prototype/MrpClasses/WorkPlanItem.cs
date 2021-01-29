using System;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace Mrp.Prototype.MrpClasses
{
    public class WorkPlanItem : MrpObject
    {
        public ResourceType ResourceType { get; set; }
        public TimeSpan DurationPerPiece { get; set; } = new TimeSpan(0);
        public TimeSpan PrepTime { get; set; } = new TimeSpan(0);
        public TimeSpan CleanupTime { get; set; } = new TimeSpan(0);
        public TimeSpan TransportTime { get; set; } = new TimeSpan(0);

        public TimeSpan GetTotalDuration(int pieces)
        {
            var result = PrepTime + new TimeSpan(DurationPerPiece.Ticks * pieces) + CleanupTime + TransportTime;
            return result;
        }
    }
}