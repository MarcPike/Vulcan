using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer.Model;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class BbsObservationModel
    {
        public string Id { get; set; }
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
        public int ObservationId { get; set; }
        public int? NumberOfPeopleObserved { get; set; }
        public PropertyValueRef ShiftType { get; set; }
        public string WorkerComments { get; set; }
        public PropertyValueRef Take5Department { get; set; }
        public bool TaskStopped { get; set; }
        public bool CorrectedOnJob { get; set; }
        public bool RasiedWithSupIndiv { get; set; }
        public bool RasiedWithSupObsrv { get; set; }
        public bool TrainingNeeded { get; set; }


        public bool IsDirty { get; set; } = false;

        public BbsObservationModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        public BbsObservationModel(BbsObservation o)
        {
            DateOf = o.DateOf;
            Department = o.Department;
            DepartmentSubCategory = o.DepartmentSubCategory;
            EmployeeType = o.EmployeeType;
            TaskType = o.TaskType;
            TaskSubCategory = o.TaskSubCategory;
            Id = o.Id.ToString();
            ShiftType = o.ShiftType;
            NumberOfPeopleObserved = o.NumberOfPeopleObserved;
            ObservationId = o.ObservationId;
            WorkerComments = o.WorkerComments;
            Take5Department = o.Take5Department;
            Location = o.Location;
            Observer = o.Observer;
            ObservationType = o.ObservationType;
            Items = o.Items;
            ObserverComments = o.ObserverComments;
            TaskStopped = o.TaskStopped;
            CorrectedOnJob = o.CorrectedOnJob;
            RasiedWithSupIndiv = o.RasiedWithSupIndiv;
            RasiedWithSupObsrv = o.RasiedWithSupObsrv;
            TrainingNeeded = o.TrainingNeeded;


        }

    }
}
