using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhHazardType : BaseDocument
    {
        public string HazardType { get; set; }

        public CoshhHazardTypeRef AsCoshhHazardTypeRef()
        {
            return new CoshhHazardTypeRef(this);
        }

        public static CoshhHazardTypeRef CreateAndReturnRef(RepositoryBase<CoshhHazardType> rep, string hazardType)
        {
            var newHazardType = rep.AsQueryable().FirstOrDefault(x => x.HazardType == hazardType);
            if (newHazardType == null)
            {
                newHazardType = new CoshhHazardType()
                {
                    HazardType = hazardType
                };
                rep.Upsert(newHazardType);
            }

            return newHazardType.AsCoshhHazardTypeRef();
        }

    }
}
