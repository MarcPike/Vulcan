using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsDepartmentRef : ReferenceObject<BbsDepartment>
    {
        public string Name { get; set; }

        public BbsDepartmentRef()
        {
        }

        public BbsDepartmentRef(BbsDepartment dept) : base(dept)
        {
            Name = dept.Name;
        }

        public BbsDepartment AsBbsDepartment()
        {
            return ToBaseDocument();
        }
    }
}