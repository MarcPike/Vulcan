using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Analysis;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using MoreLinq;

namespace DAL.Vulcan.Mongo.Models
{
    public class ProductAnalysisResultModel
    {
        public ProductMaster ProductInfo { get; set; }
        public string DisplayCurrency { get; set; }
        public List<TeamRef> TeamsFound { get; set; } = new List<TeamRef>();
        public List<CompanyRef> CompaniesFound { get; set; } = new List<CompanyRef>();
        public List<ProspectRef> ProspectsFound { get; set; } = new List<ProspectRef>();
        public int TotalQuotes { get; set; }

        public AnalysisSummaryModel Days7 { get; set; }
        public AnalysisSummaryModel Days15 { get; set; }
        public AnalysisSummaryModel Days30 { get; set; }
        public AnalysisSummaryModel Days60 { get; set; }
        public AnalysisSummaryModel Days90 { get; set; }
        public AnalysisSummaryModel Days120 { get; set; }
        public AnalysisSummaryModel DaysOlderThan120 { get; set; }

        public ProductAnalysisResultModel(ProductAnalysisQueryModel model)
        {
            var productWinLossData = ProductWinLossData.Helper.Find(x => x.ProductCode == model.ProductCode).FirstOrDefault();
            if (productWinLossData == null) return;

            DisplayCurrency = model.DisplayCurrency;
            var quotes = productWinLossData.History;

            ProductInfo = productWinLossData.ProductInfo;

            quotes = PreFilterQuotes(quotes, model);
            TotalQuotes = quotes.Count;
            //BuildUniqueLists(quotes);

            TeamsFound = quotes.Where(x => x.Team != null).Select(x => x.Team)
                .DistinctBy(x => x.Id).ToList();
            CompaniesFound = quotes.Where(x => x.Company != null).Select(x => x.Company)
                .DistinctBy(x => x.Id).ToList();
            ProspectsFound = quotes.Where(x => x.Prospect != null).Select(x => x.Prospect).DistinctBy(x => x.Id)
                .ToList();

            Calculate(quotes);

        }

        //private void BuildUniqueLists(List<ProductWinLossHistory> quotes)
        //{

        //    foreach (var quote in quotes)
        //    {

        //        if (TeamsFound.All(x => x.Id != quote.Team.Id))
        //        {
        //            TeamsFound.Add(quote.Team);
        //        }

        //        if (quote.Company != null && CompaniesFound.All(x => x.Id != quote.Company.Id))
        //        {
        //            CompaniesFound.Add(quote.Company);
        //        }

        //        if (quote.Company == null && quote.Prospect != null)
        //        {
        //            if (ProspectsFound.All(x => x.Id != quote.Prospect.Id))
        //            {
        //                ProspectsFound.Add(quote.Prospect);
        //            }
        //        }
        //    }
        //}

        private List<ProductWinLossHistory> PreFilterQuotes(List<ProductWinLossHistory> quotes, ProductAnalysisQueryModel model)
        {
            if (model.Companies.Any())
            {
                if (model.Prospects.Any())
                {
                    quotes = quotes.Where(x => model.Prospects.Any(p => p.Id == x.Prospect.Id) && model.Companies.Any(c=>c.Id == x.Company.Id)).ToList();
                }

                quotes = quotes.Where(x => model.Companies.Any(c => c.Id == x.Company.Id)).ToList();
            }
            else if (model.Prospects.Any())
            {
                quotes = quotes.Where(x => model.Prospects.Any(p => p.Id == x.Prospect.Id)).ToList();
            }

            if (model.Teams.Any())
            {
                quotes = quotes.Where(x => model.Teams.Any(t => t.Id == x.Team.Id)).ToList();
            }

            return quotes;
        }

        public void Calculate(List<ProductWinLossHistory> quotes)
        {
            var days7 = DateTime.Now.AddDays(-7);
            var days15 = DateTime.Now.AddDays(-15);
            var days30 = DateTime.Now.AddDays(-30);
            var days60 = DateTime.Now.AddDays(-60);
            var days90 = DateTime.Now.AddDays(-90);
            var days120 = DateTime.Now.AddDays(-120);

            Task getDays7 = new Task(() =>
                {

                    Days7 = new AnalysisSummaryModel()
                    {
                        Won = new AnalysisDetailModel(quotes.Where(x => x.Win && x.WonLossDate >= days7).ToList(),
                            DisplayCurrency),
                        Lost = new AnalysisDetailModel(quotes.Where(x => x.Loss && x.WonLossDate >= days7).ToList(),
                            DisplayCurrency),
                        Expired = new AnalysisDetailModel(
                            quotes.Where(x => x.Expired && x.ExpiredDate >= days7).ToList(),
                            DisplayCurrency),
                        Submitted = new AnalysisDetailModel(
                            quotes.Where(x => x.Status == PipelineStatus.Submitted && x.SubmitDate >= days7).ToList(),
                            DisplayCurrency)
                    };
                });
            getDays7.Start();


            Task getDays15 = new Task(() =>
            {
                Days15 = new AnalysisSummaryModel()
                {
                    Won = new AnalysisDetailModel(
                        quotes.Where(x => x.Win && x.WonLossDate >= days15 && x.WonLossDate < days7).ToList(),
                        DisplayCurrency),
                    Lost = new AnalysisDetailModel(
                        quotes.Where(x => x.Loss && x.WonLossDate >= days15 && x.WonLossDate < days7).ToList(),
                        DisplayCurrency),
                    Expired = new AnalysisDetailModel(
                        quotes.Where(x => x.Expired && x.ExpiredDate >= days15 && x.ExpiredDate < days7).ToList(),
                        DisplayCurrency),
                    Submitted = new AnalysisDetailModel(
                        quotes.Where(
                                x => x.Status == PipelineStatus.Submitted && x.SubmitDate >= days15 && x.SubmitDate < days7)
                            .ToList(),
                        DisplayCurrency)
                };
            });
            getDays15.Start();

            Task getDays30 = new Task(() =>
            {
                Days30 = new AnalysisSummaryModel()
                {
                    Won = new AnalysisDetailModel(
                        quotes.Where(x => x.Win && x.WonLossDate >= days30 && x.WonLossDate < days15).ToList(),
                        DisplayCurrency),
                    Lost = new AnalysisDetailModel(
                        quotes.Where(x => x.Loss && x.WonLossDate >= days30 && x.WonLossDate < days15).ToList(),
                        DisplayCurrency),
                    Expired = new AnalysisDetailModel(
                        quotes.Where(x => x.Expired && x.ExpiredDate >= days30 && x.ExpiredDate < days15).ToList(),
                        DisplayCurrency),
                    Submitted = new AnalysisDetailModel(
                        quotes.Where(x =>
                            x.Status == PipelineStatus.Submitted && x.SubmitDate >= days30 && x.SubmitDate < days15).ToList(),
                        DisplayCurrency)
                };
            });
            getDays30.Start();

            Task getDays60 = new Task(() =>
            {
                Days60 = new AnalysisSummaryModel()
                {
                    Won = new AnalysisDetailModel(
                        quotes.Where(x => x.Win && x.WonLossDate >= days60 && x.WonLossDate < days30).ToList(),
                        DisplayCurrency),
                    Lost = new AnalysisDetailModel(
                        quotes.Where(x => x.Loss && x.WonLossDate >= days60 && x.WonLossDate < days30).ToList(),
                        DisplayCurrency),
                    Expired = new AnalysisDetailModel(
                        quotes.Where(x => x.Expired && x.ExpiredDate >= days60 && x.ExpiredDate < days30).ToList(),
                        DisplayCurrency),
                    Submitted = new AnalysisDetailModel(
                        quotes.Where(x =>
                            x.Status == PipelineStatus.Submitted && x.SubmitDate >= days60 && x.SubmitDate < days30).ToList(),
                        DisplayCurrency)
                };
            });
            getDays60.Start();

            Task getDays90 = new Task(() =>
            {
                Days90 = new AnalysisSummaryModel()
                {
                    Won = new AnalysisDetailModel(
                        quotes.Where(x => x.Win && x.WonLossDate >= days90 && x.WonLossDate < days60).ToList(),
                        DisplayCurrency),
                    Lost = new AnalysisDetailModel(
                        quotes.Where(x => x.Loss && x.WonLossDate >= days90 && x.WonLossDate < days60).ToList(),
                        DisplayCurrency),
                    Expired = new AnalysisDetailModel(
                        quotes.Where(x => x.Expired && x.ExpiredDate >= days90 && x.ExpiredDate < days60).ToList(),
                        DisplayCurrency),
                    Submitted = new AnalysisDetailModel(
                        quotes.Where(x =>
                            x.Status == PipelineStatus.Submitted && x.SubmitDate >= days90 && x.SubmitDate < days60).ToList(),
                        DisplayCurrency)
                };
            });
            getDays90.Start();


            Task getDays120 = new Task(() =>
            {
                Days120 = new AnalysisSummaryModel()
                {
                    Won = new AnalysisDetailModel(
                        quotes.Where(x => x.Win && x.WonLossDate >= days120 && x.WonLossDate < days90).ToList(),
                        DisplayCurrency),
                    Lost = new AnalysisDetailModel(
                        quotes.Where(x => x.Loss && x.WonLossDate >= days120 && x.WonLossDate < days90).ToList(),
                        DisplayCurrency),
                    Expired = new AnalysisDetailModel(
                        quotes.Where(x => x.Expired && x.ExpiredDate >= days120 && x.ExpiredDate < days90).ToList(),
                        DisplayCurrency),
                    Submitted = new AnalysisDetailModel(
                        quotes.Where(x =>
                            x.Status == PipelineStatus.Submitted && x.SubmitDate >= days120 && x.SubmitDate < days90).ToList(),
                        DisplayCurrency)
                };
            });
            getDays120.Start();

            Task getDaysOlderThan120 = new Task(() =>
            {
                DaysOlderThan120 = new AnalysisSummaryModel()
                {
                    Won = new AnalysisDetailModel(quotes.Where(x => x.Win && x.WonLossDate < days120).ToList(),
                        DisplayCurrency),
                    Lost = new AnalysisDetailModel(quotes.Where(x => x.Loss && x.WonLossDate < days120).ToList(),
                        DisplayCurrency),
                    Expired = new AnalysisDetailModel(
                        quotes.Where(x => x.Expired && x.ExpiredDate < days120).ToList(),
                        DisplayCurrency),
                    Submitted = new AnalysisDetailModel(
                        quotes.Where(x => x.Status == PipelineStatus.Submitted && x.SubmitDate < days120).ToList(),
                        DisplayCurrency)
                };
            });
            getDaysOlderThan120.Start();


            Task.WaitAll(
                getDays7,
                getDays15,
                getDays30,
                getDays60,
                getDays90,
                getDays120,
                getDaysOlderThan120);

        }
    }
}
