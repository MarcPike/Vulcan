using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.Models;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using MongoDB.Bson;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;
using Performance = DAL.HRS.Mongo.DocClass.Performance.Performance;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperPerformance: HelperBase, IHelperPerformance
    {
        private readonly HelperUser _helperUser;
        private readonly HelperEmployee _helperEmployee;

        public HelperPerformance()
        {
            _helperUser = new HelperUser();
            _helperEmployee = new HelperEmployee();
        }

        public List<PerformanceGridModel> GetPerformanceGrid(string userId)
        {
            var employees = _helperEmployee.GetAllMyEmployeeGridModelsForModule(userId, "Performance", true, withPerformance:true);
            var result = new List<PerformanceGridModel>();

            foreach (var employee in employees)
            {
                //if (missingValues.All(x=> x != queryHelper.FindById(employee.Id).PerformanceReviewType?.Code))
                    result.Add(new PerformanceGridModel(employee));
            }

            return result.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }

        public (PerformanceModel Current, List<PerformanceModel> History) GetPerformanceModelsForEmployee(string employeeId)
        {
            var employee = _helperEmployee.GetEmployee(employeeId);
            if (employee == null) throw new Exception("Employee not found");
            var employeeRef = employee.AsEmployeeRef();

            var currentPerf = employee.Performance.LastOrDefault(x => x.Locked == false) ?? new Performance();
            var historyPerf = employee.Performance.Where(x => x.Locked).ToList();

            var current = new PerformanceModel(employeeRef, currentPerf) {Id = Guid.NewGuid().ToString()};
            var history = new List<PerformanceModel>();
            foreach (var performance in historyPerf.OrderByDescending(x=>x.DateOf))
            {
                history.Add(new PerformanceModel(employeeRef, performance));
            }
            return (current, history);
        }

        public (PerformanceModel Current, List<PerformanceModel> History) SavePerformance(PerformanceModel model)
        {

            if (model.IsDirty)
            {
                var rep = new RepositoryBase<Employee>();
                var emp = rep.Find(model.Employee.Id);
                var audit = new EmployeeAuditTrail(emp, model.ModifiedBy);

                foreach (var performance in emp.Performance.Where(x => x.Locked == false).ToList())
                {
                    performance.Locked = true;
                }

                var perf = new Performance()
                {
                    Id = Guid.Parse(model.Id),
                    Comment = model.Comment,
                    DateOf = model.DateOf,
                    DateOfNextReview = model.DateOfNextReview,
                    GradeRatingType = model.GradeRatingType,
                    Notes = model.Notes,
                    PerformanceReviewType = model.PerformanceReviewType,
                    RatingOutcomeType = model.RatingOutcomeType,
                    RecommendPayIncrease = model.RecommendPayIncrease,
                    RecommendPromotion = model.RecommendPromotion,
                    Reviewer = model.Reviewer,
                    Locked = false
                };



                emp.Performance.Add(perf);
                rep.Upsert(emp);
                audit.Save(emp);

            }

            var result = GetPerformanceModelsForEmployee(model.Employee.Id);

            return (result.Current, result.History);

        }

        public PerformanceModel ModifyPerformanceHistory(PerformanceModel model)
        {

            var rep = new RepositoryBase<Employee>();
            var emp = rep.Find(model.Employee.Id);

            var perf = emp.Performance.FirstOrDefault(x => x.Id == Guid.Parse(model.Id));
            if (perf == null) throw new Exception("Performance History not found");

            if (!perf.Locked) throw new Exception("This cannot be performed on a current record, only historical items");

            perf.Comment = model.Comment;
            perf.DateOf = model.DateOf;
            perf.DateOfNextReview = model.DateOfNextReview;
            perf.GradeRatingType = model.GradeRatingType;
            perf.Notes = model.Notes;
            perf.PerformanceReviewType = model.PerformanceReviewType;
            perf.RatingOutcomeType = model.RatingOutcomeType;
            perf.RecommendPayIncrease = model.RecommendPayIncrease;
            perf.RecommendPromotion = model.RecommendPromotion;
            perf.Reviewer = model.Reviewer;
            rep.Upsert(emp);

            return new PerformanceModel(emp.AsEmployeeRef(), perf);

        }

    }
}
