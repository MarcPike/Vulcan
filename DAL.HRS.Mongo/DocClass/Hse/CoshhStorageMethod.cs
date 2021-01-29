using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhStorageMethod : BaseDocument
    {
        public string StorageMethod { get; set; }

        public CoshhStorageMethodRef AsStorageMethodRef()
        {
            return new CoshhStorageMethodRef(this);
        }

        public static CoshhStorageMethodRef CreateAndReturnRef(RepositoryBase<CoshhStorageMethod> rep, string method)
        {
            var newStorageMethod = rep.AsQueryable().FirstOrDefault(x => x.StorageMethod == method);
            if (newStorageMethod == null)
            {
                newStorageMethod = new CoshhStorageMethod()
                {
                    StorageMethod = method
                };
                rep.Upsert(newStorageMethod);
            }

            return newStorageMethod.AsCoshhStorageMethodRef();
        }

        public CoshhStorageMethodRef AsCoshhStorageMethodRef()
        {
            return new CoshhStorageMethodRef(this);
        }
    }
}