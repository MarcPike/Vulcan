using System;

namespace Mrp.Prototype.MrpClasses
{
    public class JobHistory
    {
        public Job Job { get; set; }
        public ShopWorker ShopWorker { get; set; }
        public int CompletedPieces { get; set; }
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public DateTime EndTime { get; set; } = DateTime.MinValue;
        public TimeSpan Duration => ((StartTime != DateTime.MinValue) && (EndTime != DateTime.MinValue)) ? EndTime - StartTime : TimeSpan.Zero;
    }
}