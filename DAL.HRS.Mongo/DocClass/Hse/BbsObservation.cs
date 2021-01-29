using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsObservation : BaseDocument
    {
        public static MongoRawQueryHelper<BbsObservation> Helper = new MongoRawQueryHelper<BbsObservation>();
        public int OldHrsId { get; set; }
        public DateTime? DateOf { get; set; }
        public BbsDepartmentRef Department { get; set; }
        public BbsDepartmentSubCategoryRef DepartmentSubCategory { get; set; }
        public BbsTaskRef TaskType { get; set; }
        public BbsTaskSubCategoryRef TaskSubCategory { get; set; }
        public PropertyValueRef EmployeeType { get; set; }
        public BbsObserverRef Observer { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef ObservationType { get; set; }
        public string ObserverComments { get; set; }
        public List<BbsObservationItem> Items { get; set; } = new List<BbsObservationItem>();
        public PropertyValueRef ShiftType { get; set; }
        public int? NumberOfPeopleObserved { get; set; }
        public int ObservationId { get; set; }
        public string WorkerComments { get; set; }
        public PropertyValueRef Take5Department { get; set; }
        public bool TaskStopped { get; set; }
        public bool CorrectedOnJob { get; set; }
        public bool RasiedWithSupIndiv { get; set; }
        public bool RasiedWithSupObsrv { get; set; }
        public bool TrainingNeeded { get; set; }

        

        public BbsObservation()
        {

            ObservationId = GetNextObservationId();

        }

        public static int GetNextObservationId()
        {

            var queryHelper = new MongoRawQueryHelper<BbsObservation>();
            var filter = queryHelper.FilterBuilder.Empty;
            var project = queryHelper.ProjectionBuilder.Expression(x => x.ObservationId);
            var incidents = queryHelper.FindWithProjection(filter, project).ToList();
            if (!incidents.Any()) return 1;

            return incidents.Max() + 1;
        }


    }
}
