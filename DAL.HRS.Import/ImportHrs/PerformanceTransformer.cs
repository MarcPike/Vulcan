using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Performance;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;
using Performance = DAL.HRS.Mongo.DocClass.Performance.Performance;

namespace DAL.HRS.Import.ImportHrs
{
    public static class PerformanceTransformer
    {
        public static void TransformPerformance(Employee employee, PerformanceHrs performanceHrs)
        {
            var rep = new RepositoryBase<DAL.HRS.Mongo.DocClass.Employee.Employee>();

            if (performanceHrs.PerformanceHistory != null)
            {
                foreach (var history in performanceHrs.PerformanceHistory)
                {
                    var oldPerformance = TransformPerformanceHistory(rep, history);
                    employee.Performance.Add(oldPerformance);

                }
            }

            var ratingOutcomeComment = string.Empty;
            var ratingOutcomeType = "Unknown";

            if (performanceHrs.RatingOutcomes != null)
            {
                foreach (var outcome in performanceHrs.RatingOutcomes)
                {

                    ratingOutcomeComment = outcome.Comment;
                    ratingOutcomeType = outcome.RatingOutcomeType;
                }
            }

            var reviewer = rep.AsQueryable().FirstOrDefault(x => x.OldHrsId == performanceHrs.ReviewerId)
                ?.AsEmployeeRef();

            var newPerformance =  new Performance()
            {
                
                DateOf = performanceHrs.DateOf,
                DateOfNextReview = performanceHrs.DateOfNextReview,
                GradeRatingType = PropertyBuilder.CreatePropertyValue("PerformanceGradeRatingType","Performance Grade Rating Type", 
                    performanceHrs.GradeRatingType, "Grade Rating").AsPropertyValueRef(),
                Notes = performanceHrs.Notes,
                //PerformanceHistory = perfHist,
                PerformanceReviewType = PropertyBuilder.CreatePropertyValue("PerformanceReviewType", "Performance Review Type", 
                    performanceHrs.PerformanceReviewType, "Review Type").AsPropertyValueRef(),
                RecommendPayIncrease = PropertyBuilder.CreatePropertyValue("PerformanceRecommendPayIncrease", "Pay Increasse recommended",
                    performanceHrs.RecommendPayIncrease, "Pay Increase recommended").AsPropertyValueRef(),
                RecommendPromotion = PropertyBuilder.CreatePropertyValue("RecommendPromotion", "Promotion recommended",
                    performanceHrs.RecommendPromotion, "Promotion recommended").AsPropertyValueRef(),
                Reviewer = reviewer,
                Comment = ratingOutcomeComment,
                RatingOutcomeType = PropertyBuilder.CreatePropertyValue("PerformanceRateOutcomeType", "Performance Rate Outcome Type",
                    ratingOutcomeType, "Outcome Type").AsPropertyValueRef(),

                Locked = false
            };
            employee.Performance.Add(newPerformance);

        }

        public static Performance TransformPerformanceHistory(RepositoryBase<Employee> rep, PerformanceHistoryHrs perfHist)
        {
            var reviewer = rep.AsQueryable().FirstOrDefault(x => x.OldHrsId == perfHist.Reviewer)
                ?.AsEmployeeRef();

            return new Performance()
            {
                DateOf = perfHist.Date,
                DateOfNextReview = perfHist.DateOfNextReview,
                GradeRatingType = PropertyBuilder.CreatePropertyValue("PerformanceGradeRatingType", "Performance Grade Rating Type",
                    perfHist.GradeRatingType, "Grade Rating").AsPropertyValueRef(),
                Notes = perfHist.Notes,
                PerformanceReviewType = PropertyBuilder.CreatePropertyValue("PerformanceReviewType", "Performance Review Type",
                    perfHist.PerformanceReviewType, "Review Type").AsPropertyValueRef(),
                RecommendPayIncrease = PropertyBuilder.CreatePropertyValue("PerformanceRecommendPayIncrease", "Pay Increasse recommended",
                    perfHist.RecommendPayIncrease, "Pay Increase recommended").AsPropertyValueRef(),
                RecommendPromotion = PropertyBuilder.CreatePropertyValue("RecommendPromotion", "Promotion recommended",
                    perfHist.RecommendPromotion, "Promotion recommended").AsPropertyValueRef(),
                Reviewer = reviewer,
                Locked = true
            };
        }

        //public static PerformanceRateOutcome TransformRatingOutcome(PerformanceRateOutcomeHrs outcome)
        //{
        //    return new PerformanceRateOutcome()
        //    {
        //        Comment = outcome.Comment,
        //        RatingOutcomeType = PropertyBuilder.CreatePropertyValue("PerformanceRateOutcomeType", "Performance Rate Outcome Type",
        //            outcome.RatingOutcomeType, "Outcome Type").AsPropertyValueRef(),
        //    };
        //}
    }

}
