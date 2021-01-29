using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CompanyCreditModel
    {
        public CompanyCreditModel()
        {
            
        }

        public CompanyCreditModel(CompanyRef companyRef)
        {
            var company = companyRef.AsCompany();

            var sqlId = company.SqlId;




        }
    }
}
