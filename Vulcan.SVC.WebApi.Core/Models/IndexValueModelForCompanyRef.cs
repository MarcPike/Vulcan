using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;

namespace Vulcan.SVC.WebApi.Core.Models
{
    public class IndexValueModelForCompanyRef   
    {
        public string Index { get; set; }
        public List<CompanyRef> Companies { get; set; }
    }
}
