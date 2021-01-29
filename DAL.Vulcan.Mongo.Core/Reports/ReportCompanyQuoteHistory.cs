using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Reports
{

    public class ReportCompanyQuoteHistory
    {
        private List<ReportCompanyQuoteHistoryDetail> _reportData = new List<ReportCompanyQuoteHistoryDetail>();
        private ReportCompanyQuoteHistoryDataLoader _dataLoader = null;
        private string _displayCurrency;

        public ReportCompanyQuoteHistory(ReportCompanyQuoteHistoryDataLoader dataLoader, string displayCurrency)
        {
            _dataLoader = dataLoader;
            _displayCurrency = displayCurrency;
        }

        public List<ReportCompanyQuoteHistoryDetail> GetReportDataFromLoader()
        {
            if (!_dataLoader.Loaded) throw new Exception("No attempt to Load data for report");
           
            foreach (var quote in _dataLoader.Quotes)
            {
                _reportData.AddRange(ReportCompanyQuoteHistoryDetail.ScrapeFromQuote(quote, _displayCurrency));
            }

            return _reportData;
        }
    }
}
