using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsObserver : BaseDocument
    {
        public static MongoRawQueryHelper<BbsObserver> Helper = new MongoRawQueryHelper<BbsObserver>();
        public int OldHrsId { get; set; }
        public EmployeeRef Employee { get; set; }
        public bool IsActive { get; set; }

        public BbsObserverRef AsBbsObserverRef()
        {
            return new BbsObserverRef(this);
        }

        public static BbsObserverRef GetBbsObserverRefByOldId(int oldHrsId)
        {
            var observer = Helper.Find(x => x.OldHrsId == oldHrsId).FirstOrDefault();
            return observer?.AsBbsObserverRef();
        }
    }

    public class BbsObserverRef : ReferenceObject<BbsObserver>
    {
        public EmployeeRef Employee { get; set; }

        public BbsObserverRef()
        {
            
        }

        public BbsObserverRef(BbsObserver o)
        {
            Employee = o.Employee;
        }

        public BbsObserver AsBbsObserver()
        {
            return ToBaseDocument();
        }
    }
}
