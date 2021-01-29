using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public struct PerformanceGridModel
    {
        public string EmployeeId { get; set; }
        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public string GovernmentId { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }
        public LocationRef Location { get; set; }
        public EmployeeRef Manager { get; set; }
        public bool IsActive { get; set; }
        public JobTitleRef JobTitle { get; set; }
        public PayrollRegionRef PayrollRegion { get; set; }
        public PropertyValueRef BusinessRegionCode { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }
        public PropertyValueRef Status1Code { get; set; }

        public DateTime? DateOfNextReview { get; set; }
        public PropertyValueRef PerformanceReviewType { get; set; }


        public PerformanceGridModel(BaseGridModel m)
        {
            EmployeeId = m.Id.ToString();
            PayrollId = m.PayrollId;
            LastName = m.LastName;
            FirstName = m.FirstName;
            MiddleName = m.MiddleName;
            PreferredName = m.PreferredName;
            GovernmentId = m.GovernmentId;
            CostCenterCode = m.CostCenterCode;
            Location = m.Location;
            IsActive = m.IsActive;
            Manager = m.Manager.Refresh();
            JobTitle = m.JobTitle;
            PayrollRegion = m.PayrollRegion;

            var collection = new RepositoryBase<Employee>().Collection;

            var builder = Builders<Employee>.Filter;
            var filter = builder.Eq(x => x.Id, ObjectId.Parse(m.EmployeeId));
            
            var performances = collection.Find(filter).Project(x=>x.Performance).FirstOrDefault();

            var currentPerformance = performances.FirstOrDefault(x => x.Locked == false);

            DateOfNextReview = currentPerformance?.DateOfNextReview;
            PerformanceReviewType = currentPerformance?.PerformanceReviewType;

            BusinessRegionCode = m.BusinessRegionCode;
            KronosDepartmentCode = m.KronosDepartmentCode;
            Status1Code = m.Status1Code;

        }

    }
}
