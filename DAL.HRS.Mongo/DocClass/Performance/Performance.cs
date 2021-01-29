using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Performance
{
    public class Performance 
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateOf { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateOfNextReview { get; set; }
        public PropertyValueRef GradeRatingType { get; set; }
        public string Notes { get; set; }
        //public List<PerformanceHistory> PerformanceHistory { get; set; } = new List<PerformanceHistory>();
        public PropertyValueRef PerformanceReviewType { get; set; }
        public PropertyValueRef RecommendPayIncrease { get; set; }
        public PropertyValueRef RecommendPromotion { get; set; }
        public EmployeeRef Reviewer { get; set; }
        public bool Locked { get; set; } = true;
        public string Comment { get; set; }
        public PropertyValueRef RatingOutcomeType { get; set; }


    }

    //public class PerformanceRateOutcome
    //{
    //    public Guid Id { get; set; } = Guid.NewGuid();
    //    public string Comment { get; set; }
    //    public PropertyValueRef RatingOutcomeType { get; set; }

    //}

    //public class PerformanceHistory
    //{
    //    public Guid Id { get; set; } = Guid.NewGuid();
    //    public DateTime? CreatedOn { get; set; }
    //    public DateTime? DateOf { get; set; }
    //    public DateTime? DateOfNextReview { get; set; }
    //    public PropertyValueRef GradeRatingType { get; set; }
    //    public string Notes { get; set; }
    //    public PropertyValueRef PerformanceReviewType { get; set; }
    //    public PropertyValueRef RecommendPayIncrease { get; set; }
    //    public PropertyValueRef RecommendPromotion { get; set; }

    //}

}
