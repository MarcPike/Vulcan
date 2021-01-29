using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsDepartmentSubCategoryRef : ReferenceObject<BbsDepartmentSubCategory>
    {
        public string Name { get; set; }
       

        public BbsDepartmentSubCategoryRef()
        {
       
        }

        public BbsDepartmentSubCategoryRef(BbsDepartmentSubCategory dept) : base(dept)
        {
            Name = dept.Name;
           
        }


        public BbsDepartmentSubCategory AsBbsDepartmentSubCategory()
        {
            return ToBaseDocument();
        }

    }
}