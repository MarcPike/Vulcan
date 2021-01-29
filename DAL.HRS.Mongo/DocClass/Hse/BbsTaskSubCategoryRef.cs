using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsTaskSubCategoryRef : ReferenceObject<BbsTaskSubCategory>
    {
        public string Name { get; set; }

        public BbsTaskSubCategoryRef()
        {
        }

        public BbsTaskSubCategoryRef(BbsTaskSubCategory dept) : base(dept)
        {
            Name = dept.Name;
        }


        public BbsTaskSubCategory AsBbsTaskSubCategoryRef()
        {
            return ToBaseDocument();
        }

    }
}