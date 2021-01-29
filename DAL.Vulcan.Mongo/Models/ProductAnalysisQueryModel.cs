using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Models
{
    public class ProductAnalysisQueryModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public List<TeamRef> Teams { get; set; } = new List<TeamRef>();
        public List<CompanyRef> Companies { get; set; } = new List<CompanyRef>();
        public List<ProspectRef> Prospects { get; set; } = new List<ProspectRef>();
        public string ProductCode { get; set; }
        public string DisplayCurrency { get; set; }

        public ProductAnalysisQueryModel()
        {
            
        }
    }
}
