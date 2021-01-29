using System;
using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.Models;
using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Models
{
    public class CompanyModel : BaseModel<CompanyModelHelper>
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string CustomerType { get; set; }

        public List<CompanySubAddress> Addresses { get; set; }


        public static CompanyModel Convert(string coid, int companyId, CompanyContext context)
        {

            var company = context.Company.SingleOrDefault(x => x.Id == companyId);
            if (company == null) return null;

            var companyType = context.CompanyTypeCode.First(x => x.Id == (company.TypeId ?? 0));


            var companyModel = new CompanyModel()
            {
                Id = companyId,
                Coid = coid,
                Code = company.Code,
                ShortName = company.ShortName,
                Name = company.Name,
                CreatedOn = company.Cdate ?? DateTime.UtcNow,
                ModifiedOn = company.Mdate,
                CustomerType = companyType.Description,
                Addresses = company.CompanySubAddress.ToList()
            };


            return companyModel;
        }

    }
}
