using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhStorageMethodRef : ReferenceObject<CoshhStorageMethod>
    {
        public string StorageMethod { get; set; }

        public CoshhStorageMethodRef()
        {
        }

        public CoshhStorageMethodRef(CoshhStorageMethod method) : base(method)
        {
            StorageMethod = method.StorageMethod;
        }

        public CoshhStorageMethod AsCoshhStorageMethod()
        {
            return ToBaseDocument();
        }
    }
}