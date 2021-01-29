using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Companies;

namespace DAL.Marketing.Docs
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