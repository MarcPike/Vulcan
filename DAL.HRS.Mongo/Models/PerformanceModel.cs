using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Performance;
using DAL.HRS.Mongo.DocClass.Properties;
using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class PerformanceModel: BaseModel, IHavePropertyValues
    {
        public string Id { get; set; } 
        public EmployeeRef Employee { get; set; }
            
        public DateTime? DateOf { get; set; }
        public DateTime? DateOfNextReview { get; set; }
        public PropertyValueRef GradeRatingType { get; set; }
        public string Notes { get; set; }
        public PropertyValueRef PerformanceReviewType { get; set; }
        public PropertyValueRef RecommendPayIncrease { get; set; }
        public PropertyValueRef RecommendPromotion { get; set; }
        public EmployeeRef Reviewer { get; set; }
        public bool Locked { get; set; }
        public string Comment { get; set; }
        public PropertyValueRef RatingOutcomeType { get; set; }

        public bool IsDirty { get; set; } = false;
        public HrsUserRef ModifiedBy { get; set; }

        public override string ToString()
        {
            var nullString = "null";
            return
                $"Id: {Id} DateOf: {DateOf?.ToShortDateString() ?? nullString} NextReview: {DateOfNextReview?.ToShortDateString() ?? nullString} Grade: {GradeRatingType?.Code ?? nullString} Notes: {Notes} ReviewType: {PerformanceReviewType?.Code ?? nullString} PayIncrease: {RecommendPayIncrease?.Code ?? nullString} Promote: {RecommendPromotion?.Code ?? nullString} Reviewer: {Reviewer?.ToString() ?? nullString} RatingOutcome: {RatingOutcomeType?.Code ?? nullString} Comments: {Comment}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, GradeRatingType);
            LoadCorrectPropertyValueRef(entity, PerformanceReviewType);
            LoadCorrectPropertyValueRef(entity, RecommendPayIncrease);
            LoadCorrectPropertyValueRef(entity, RecommendPromotion);
            LoadCorrectPropertyValueRef(entity, RatingOutcomeType);
        }

        public PerformanceModel()
        {
        }

        public PerformanceModel(EmployeeRef emp, Performance perf)
        {
            var helper = DAL.HRS.Mongo.DocClass.Employee.Employee.Helper;
            var idObject = ObjectId.Parse(emp.Id);
            var filter = helper.FilterBuilder.Where(x => x.Id == idObject);
            var project = helper.ProjectionBuilder.Expression(x => x.Entity);
            var entity = helper.FindWithProjection(filter, project).SingleOrDefault();

            if (perf == null) return;
            Employee = emp;
            Id = perf.Id.ToString();
            DateOf = perf.DateOf;
            DateOfNextReview = perf.DateOfNextReview;
            GradeRatingType = perf.GradeRatingType;
            Notes = perf.Notes;
            //PerformanceHistory = PerformanceHistoryModel.ConvertList(perf.PerformanceHistory);
            PerformanceReviewType = perf.PerformanceReviewType;
            RecommendPayIncrease = perf.RecommendPayIncrease;
            RecommendPromotion = perf.RecommendPromotion;
            Reviewer = perf.Reviewer;
            Locked = perf.Locked;
            Comment = perf.Comment;
            RatingOutcomeType = perf.RatingOutcomeType;

            if (entity != null) 
                LoadPropertyValuesWithThisEntity(entity);

            //UpdatePropertyReferences.Execute(this);

        }

    }
}
