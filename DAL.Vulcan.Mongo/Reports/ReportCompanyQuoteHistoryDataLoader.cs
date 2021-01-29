using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Reports
{
    public class ReportCompanyQuoteHistoryDataLoader
    {
        public List<CrmQuote> Quotes { get; set; } = new List<CrmQuote>();
        public bool Loaded { get; set; } = false;
        public void LoadCompanyHistoryForDateRange(Company company, DateTime fromDate, DateTime toDate)
        {
            Quotes = new RepositoryBase<CrmQuote>().AsQueryable().Where(x =>
                    x.Company.Id == company.Id.ToString() &&
                    x.ReportDate != null &&
                    x.ReportDate >= fromDate &&
                    x.ReportDate <= toDate)
                .ToList().OrderByDescending(x => x.ReportDate).ToList();

            Loaded = true;
        }
    }
}
