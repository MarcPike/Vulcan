using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class BbsObservationGridModel
    {
        public ObjectId Id { get; set; }

        //public string ObservationId
        //{
        //    get { return Id.ToString(); }
        //}

        public int ObservationId { get; set; }
        public LocationRef Location { get; set; }
        public DateTime? DateOf { get; set; }
        public BbsObserverRef Observer { get; set; }
        public PropertyValueRef ShiftType { get; set; }
        public int? NumberOfPeopleObserved { get; set; }
        public BbsDepartmentRef Department { get; set; }
        public BbsDepartmentSubCategoryRef DepartmentSubCategory { get; set; }
        public BbsTaskRef TaskType { get; set; }
        public BbsTaskSubCategoryRef TaskSubCategory { get; set; }
        public PropertyValueRef EmployeeType { get; set; }
        public PropertyValueRef Take5Department { get; set; }


    }
}
