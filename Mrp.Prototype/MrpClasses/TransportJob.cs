using System;
using DAL.Vulcan.Mongo.Quotes;

namespace Mrp.Prototype.MrpClasses
{
    public class TransportJob
    {
        public WorkOrderItem WorkOrderItem { get; set; }
        public RequiredQuantity RequiredQuantity { get; set; }
        public Resource FromResource { get; set; }
        public Resource ToResource { get; set; }
        public Transporter ClaimedBy { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ClaimedTime { get; set; }
        public TimeSpan WaitTime => RequestTime - ClaimedTime;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TransportTime => EndTime - StartTime;

        public TransportJob(WorkOrderItem item, Resource fromResource)
        {
            WorkOrderItem = item;
            FromResource = fromResource;
            
        }

    }
}