using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsTaskRef : ReferenceObject<BbsTask>
    {
        public string Name { get; set; }
        public BbsTaskRef()
        {
        }

        public BbsTaskRef(BbsTask task) : base(task)
        {
            Name = task.Name;
        }

        public BbsTask AsBbsTask()
        {
            return ToBaseDocument();
        }
    }
}