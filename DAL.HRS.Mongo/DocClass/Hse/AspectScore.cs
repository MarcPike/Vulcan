using System;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class AspectScore
    {
        public int Score { get; set; }
        public CriteriaType CriteriaType { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string Reason { get; set; }
    }
}