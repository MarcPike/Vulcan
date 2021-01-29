using System;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class AspectScoreHistory
    {
        public DateTime? DateUpdated { get; set; }
        public string Reason { get; set; }
        public int Score { get; set; }

        public AssignedWorkAreas WorkAreas = new AssignedWorkAreas();
    }
}