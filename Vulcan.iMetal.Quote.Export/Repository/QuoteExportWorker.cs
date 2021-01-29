using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.IntegrationDb;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    public class QuoteExportWorker
    {
        private readonly QuoteExporter _exporter = new QuoteExporter();
        private QuoteExportStatusChecker _checker = new QuoteExportStatusChecker();
        private readonly RepositoryBase<CrmQuote> _repository = new RepositoryBase<CrmQuote>();

        public int LookForQuotesToExport()
        {

            var quotesPending = _repository.AsQueryable()
                .Where(x => x.ExportStatus == ExportStatus.Pending || x.ExportStatus == ExportStatus.Retry).ToList();
            var quoteExported = 0;
            //var lookingForQuotes = new List<int>()
            //{
            //    16566,
            //    16661
            //};

            foreach (var crmQuote in quotesPending)
            {
                //if (lookingForQuotes.Any(x => x == crmQuote.QuoteId))
                //{
                //    var message = "Start debugging here";
                //}

                try
                {
                    if (crmQuote.ExportStatus != ExportStatus.Retry)
                    {
                        if (DuplicateExportJobAlreadyStarted(crmQuote))
                        {
                            continue;
                        }
                    }

                    _exporter.Export(quote: crmQuote, writeToImportDatabase: true);
                    quoteExported++;
                }
                catch (Exception ex)
                {
                    EMailSupport.SendEmailToSupport("Export to iMetal Exception:", new List<string>()
                    {
                        "marc.pike@howcogroup.com",
                        "isidro.gallegos@howcogroup.com"
                    }, ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }

            return quoteExported;
        }

        private bool DuplicateExportJobAlreadyStarted(CrmQuote crmQuote)
        {
            var rep = new RepositoryBase<CrmQuote>();
            var currentQuote = rep.Find(crmQuote.Id);
            var lastAttempt = currentQuote.ExportAttempts.LastOrDefault();
            if ((lastAttempt != null) && (lastAttempt.ImportStatus == "E"))
            {
                UpdateQuoteToProcessing();
                return true;
            }

            using (var context = new IntegrationDb())
            {
                if (context.import_sales_headers.Any(x =>
                    x.import_number == crmQuote.QuoteId && x.import_status == "E"))
                {
                    UpdateQuoteToProcessing();
                    return true;
                }
            }

            return false;

            void UpdateQuoteToProcessing()
            {
                if (crmQuote.ExportStatus != ExportStatus.Processing)
                {
                    crmQuote.ExportStatus = ExportStatus.Processing;
                    rep.Upsert(crmQuote);
                }
            }
        }


        public int CheckQuoteImportStatus()
        {
            var quotesToCheck = _repository.AsQueryable().Where(x => x.ExportStatus == ExportStatus.Processing)
                .ToList();
            var quotesChecked = 0;
            foreach (var crmQuote in quotesToCheck)
            {
                try
                {
                    _checker.RefreshAndGetLastExportAttempt(crmQuote);
                    quotesChecked++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

            }

            return quotesChecked;
        }

       
    }
}