using System.Collections.Generic;
using DAL.Vulcan.Mongo.Analysis;
using DAL.Vulcan.Mongo.DocClass.Companies;

namespace DAL.Vulcan.Mongo.Models
{
    public class CompanyProspectAnalysisModel
    {
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Name{ get; set; }
        public bool IsProspect { get; set; }


        //public ProductAnalysisBase SummaryResults { get; set; }

        public CompanyProspectAnalysisModel(string displayCurrency, List<ProductWinLossHistory> quotes, string coid, string code, string name, bool isProspect)
        {
            Coid = coid;
            Code = code;
            Name = name;
            IsProspect = isProspect;
            //SummaryResults = new ProductAnalysisBase(displayCurrency, quotes);
            //SummaryResults.Calculate();
        }

    }
}