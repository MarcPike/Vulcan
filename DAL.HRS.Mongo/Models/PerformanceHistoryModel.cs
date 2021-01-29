using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using System;

namespace DAL.HRS.Mongo.Models
{
    public class PerformanceHistoryModel
    {
        public string Id { get; set; } 
        public DateTime? CreatedOn { get; set; }
        public DateTime? DateOf { get; set; }
        public DateTime? DateOfNextReview { get; set; }
        public PropertyValueRef GradeRatingType { get; set; }
        public string Notes { get; set; }
        public EmployeeRef Reviewer { get; set; }
        public PropertyValueRef PerformanceReviewType { get; set; }
        public PropertyValueRef RecommendPayIncrease { get; set; }
        public PropertyValueRef RecommendPromotion { get; set; }

        public PerformanceHistoryModel()
        {
        }

        //public PerformanceHistoryModel

        //public PerformanceHistoryModel(PerformanceHistory hist)
        //{
        //    Id = hist.Id.ToString();
        //    CreatedOn = hist.CreatedOn;
        //    DateOf = hist.DateOf;
        //    DateOfNextReview = hist.DateOfNextReview;
        //    GradeRatingType = hist.GradeRatingType;
        //    Notes = hist.Notes;
        //    PerformanceReviewType = hist.PerformanceReviewType;
        //    RecommendPayIncrease = hist.RecommendPayIncrease;
        //    RecommendPromotion = hist.RecommendPromotion;

        //    UpdatePropertyReferences.Execute(this);

        //}

        //public static List<PerformanceHistoryModel> ConvertList(List<PerformanceHistory> list)
        //{
        //    var result = new List<PerformanceHistoryModel>();

        //    if (list != null)
        //    {
        //        foreach (var performanceHistory in list)
        //        {
        //            result.Add(new PerformanceHistoryModel(performanceHistory));
        //        }
        //    }

        //    return result;
        //}
    }
}