using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhDisposalMethodRef : ReferenceObject<CoshhDisposalMethod>
    {
        public string DisposalMethod { get; set; }

        public CoshhDisposalMethodRef()
        {
        }

        public CoshhDisposalMethodRef(CoshhDisposalMethod method) : base(method) 
        {

        }

        public CoshhDisposalMethod AsCoshhDisposalMethod()
        {
            return ToBaseDocument();
        }
    }
}