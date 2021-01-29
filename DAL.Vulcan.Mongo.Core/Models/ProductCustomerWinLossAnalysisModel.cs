using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.Analysis;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ProductCustomerWinLossAnalysisModel
    {
        public List<CompanyProspectAnalysisModel> CompanySummary { get; set; } =
            new List<CompanyProspectAnalysisModel>();

        public List<CompanyProspectAnalysisModel> ProspectSummary { get; set; } =
            new List<CompanyProspectAnalysisModel>();

        public ProductCustomerWinLossAnalysisModel(ProductWinLossData data, string displayCurrency)
        {
            var companies = data.History.Where(x => x.Company != null).Select(x =>  new { x.Company.Id, x.Coid, x.Company.Code, x.Company.Name }).Distinct().ToList();
            foreach (var company in companies)
            {
                var quotes = data.History.Where(x => x.Company.Id == company.Id).ToList();
                var newCompanySummary = new CompanyProspectAnalysisModel(displayCurrency, quotes, company.Coid, company.Code, company.Name, false );
                //newCompanySummary.SummaryResults.Calculate();
                CompanySummary.Add(newCompanySummary);
            }

            var prospects = data.History.Where(x => x.Company == null && x.Prospect != null).Select(x => new { x.Prospect.Id, x.Coid, x.Prospect.Code, x.Prospect.Name }).Distinct().ToList();
            var count = prospects.Count;
            foreach (var prospect in prospects)
            {
                var quotes = data.History.Where(x => x.Prospect.Id == prospect.Id).ToList();
                var newCompanySummary = new CompanyProspectAnalysisModel(displayCurrency, quotes, prospect.Coid, prospect.Code, prospect.Name, false);
                //newCompanySummary.SummaryResults.Calculate();
                CompanySummary.Add(newCompanySummary);
            }

        }

        public static ProductCustomerWinLossAnalysisModel CreateForProductCode(string productCode, string displayCurrency)
        {
            var data = ProductWinLossAnalysisBuilder.GetProductAnalysisObject(productCode);
            return new ProductCustomerWinLossAnalysisModel(data, displayCurrency);
        }


    }
}