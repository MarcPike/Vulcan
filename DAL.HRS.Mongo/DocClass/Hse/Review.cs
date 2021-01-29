using System;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class Review
    {
        public string Comment { get; set; }
        public DateTime? CurrentReviewDate { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string ReviewedBy { get; set; }
    }
}