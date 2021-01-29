using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Common.DocClass
{
    public class PayrollRegionRef : ReferenceObject<PayrollRegion>
    {
        public string Name { get; set; }

        public PayrollRegionRef()
        {
        }

        public PayrollRegionRef(PayrollRegion doc) : base(doc)
        {
            Name = doc.Name;
        }

        public PayrollRegion AsPayrollRegion()
        {
            return ToBaseDocument();
        }


    }
}