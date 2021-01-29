using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hse;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class BbsObserverModel
    {
        public string Id { get; set; }
        public EmployeeRef Employee { get; set; }
        public bool IsActive { get; set; }

        public BbsObserverModel()
        {
            Id = ObjectId.GenerateNewId().ToString();
            IsActive = true;
        }

       

        public BbsObserverModel(BbsObserver o)
        {
            Id = o.Id.ToString();
            Employee = o.Employee;
            IsActive = o.IsActive;
        }
    }
}
