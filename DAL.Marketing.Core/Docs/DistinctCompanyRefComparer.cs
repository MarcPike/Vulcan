using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;

namespace DAL.Marketing.Core.Docs
{
    public class DistinctCompanyRefComparer : IEqualityComparer<CompanyRef>
    {
        public bool Equals(CompanyRef x, CompanyRef y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(CompanyRef obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}