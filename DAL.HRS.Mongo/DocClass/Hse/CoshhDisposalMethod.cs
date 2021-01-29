using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhDisposalMethod : BaseDocument
    {
        public string DisposalMethod { get; set; }

        public CoshhDisposalMethodRef AsCoshhDisposalMethodRef()
        {
            return new CoshhDisposalMethodRef(this);
        }

        public static CoshhDisposalMethodRef CreateAndReturnRef(RepositoryBase<CoshhDisposalMethod> rep, string method)
        {
            var disposeMethod = rep.AsQueryable().FirstOrDefault(x => x.DisposalMethod == method);
            if (disposeMethod == null)
            {
                disposeMethod = new CoshhDisposalMethod()
                {
                    DisposalMethod = method
                };
                rep.Upsert(disposeMethod);
            }

            return disposeMethod.AsCoshhDisposalMethodRef();
        }
    }
}
