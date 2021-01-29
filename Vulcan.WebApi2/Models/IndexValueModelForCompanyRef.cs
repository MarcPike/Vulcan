using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Companies;

namespace Vulcan.WebApi2.Models
{
    public class IndexValueModelForCompanyRef   
    {
        public string Index { get; set; }
        public List<CompanyRef> Companies { get; set; }
    }
}
