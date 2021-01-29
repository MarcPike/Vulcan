using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.IntegrationDb;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.PublishSignalR;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    [TestFixture()]
    public class QuoteExportStatusChecker
    {
        [Test]
        public void TestQuoteStatusUpdating()
        {
            EnvironmentSettings.CrmProduction();
            var quote = new RepositoryBase<CrmQuote>().AsQueryable().FirstOrDefault(x => x.QuoteId == 67337);
            if (quote != null)
            {
                RefreshAndGetLastExportAttempt(quote);
            }
        }


        public ExportAttempt  RefreshAndGetLastExportAttempt(CrmQuote quote)
        {

            var changed = false;

            if (quote.ExportAttempts == null || quote.ExportAttempts.Count == 0) return null;
            var exportAttempt = quote.ExportAttempts.Last();
            var importCompanyReference = exportAttempt.ImportCompanyReference;
            var importBatchNumber = exportAttempt.ImportBatchNumber;

            var previousNotes = !String.IsNullOrWhiteSpace(exportAttempt.ImportNotes) ? "\r\n\r\n" + exportAttempt.ImportNotes : "";

            using (var context = new IntegrationDb())
            {
                try
                {
                    var importHeader = context.import_sales_headers.FirstOrDefault(x => x.import_company_reference == importCompanyReference && x.import_batch_number == importBatchNumber);
                    if (importHeader == null) return null;


                    //update lastImportAttempt status and Quote status.
                    var currentStatus = GetImportStatusFromStatusCode(importHeader.import_status);
                    if (quote.ExportStatus != currentStatus) 
                    {
                        // Are we doing a retry
                        quote.ExportStatus = currentStatus;
                        changed = true;


                        if (currentStatus == ExportStatus.Failed)
                        {
                            if (importHeader.import_notes == null)
                            {
                                exportAttempt.Errors =
                                    "Unknown iMetal error - iMetal did not provide reason for failure";
                            }
                            else
                            {
                                var importNotes = importHeader.import_notes;
                                exportAttempt.Errors = importNotes.Length > 60 ? importNotes.Substring(0, 60) : importNotes;
                            }

                            exportAttempt.ImportStatus = "F";
                            EmailExportError(quote, exportAttempt);
                        }
                        else if (currentStatus == ExportStatus.Success)
                        {
                            var parsedOrderNumber = GetOrderNumberFromImportNotes(importHeader.import_notes);

                            if (quote.ExternalOrderId != parsedOrderNumber)
                            {
                                quote.ExternalOrderId = parsedOrderNumber;
                                exportAttempt.SalesOrderId = parsedOrderNumber;
                                exportAttempt.ImportStatus = "I";
                            }
                        }
                    }

                    //Update notes. ImportAttempt.Notes are only for info from the Quote Tool, not from the iMetal importer.
                    if (changed)
                    {
                        var index = quote.ExportAttempts.Count - 1;
                        quote.ExportAttempts[index] = exportAttempt;
                        exportAttempt.ImportNotes = $"Quote updated from iMetal Importer on {DateTime.Now:G} {previousNotes}";
                        quote.SaveToDatabase();
                        try
                        {
                            PublishSignalREvents.QuoteExportStatusChanged(quote.Id.ToString()).Wait();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    if (changed && (currentStatus == ExportStatus.Success))
                    {
                        var salesPerson = quote.SalesPerson.AsCrmUser();
                        var salesPersonUser = salesPerson.User.AsUser();
                        var emailAddress =
                            salesPersonUser.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);
                        if (emailAddress != null)
                        {
                            var emailRecipients = new List<string> { emailAddress.Address };
                            var subject =
                                $"New iMetal Order: [{quote.ExternalOrderId}] derived from VulcanCRM Quote: {quote.QuoteId}";

                            var body = "Quote has been successfully converted to an iMetal Order";
                            if (quote.CurrentRevision != null)
                            {
                                body = quote.CurrentRevision.RevisionNotesForPdf;
                            }

                            SendEMail.Execute(subject, emailRecipients, body, null,  "VulcanCRM@howcogroup.com");

                        }
                    }

                }
                catch (Exception ex)
                {
                    exportAttempt.Errors = $"Last status refresh failed on {DateTime.Now:G} with the following message:\r\n{ex.Message}{previousNotes}";
                    throw;
                }
            }

            return exportAttempt;
        }

        private void EmailExportError(CrmQuote quote, ExportAttempt exportAttempt)
        {
            var builder = new EMailBuilder();
            builder.Subject = $"Vulcan iMetal Export error has occurred for Quote: {quote.QuoteId}";
            builder.EMailFromAddress = "VulcanCrm@Howcogroup.com";
            var exportCrmUser = quote.ExportRequestedBy.AsCrmUser();
            var exportUser = exportCrmUser.User.AsUser();
            var emailAddress = exportUser.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business) ??
                               exportUser.Person.EmailAddresses.First() ?? new EmailAddress()
                               {
                                   Address = "marc.pike@gmail.com"
                               };
            builder.Recipients = new List<string>()
            {
                emailAddress.Address,
                "marc.pike@howcogroup.com"
            };
            builder.Body = exportAttempt.Errors;
            builder.Send();
        }

        private ExportStatus GetImportStatusFromStatusCode(string code)
        {
            if (String.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
            code = code.ToUpper();

            if (code == "E") return ExportStatus.Processing;
            if (code == "I") return ExportStatus.Success; // Imported into iMetal
            if (code == "F") return ExportStatus.Failed;

            throw new ArgumentOutOfRangeException(nameof(code));
        }

        private string GetOrderNumberFromImportNotes(string importNotes)
        {
            if (string.IsNullOrWhiteSpace(importNotes)) return null;

            const string prefix = "Sales Document Created: Order:";
            var result = importNotes.Contains(prefix) ? importNotes.Replace(prefix, "") : null;
            if (result != null && result.Contains("-"))
            {
                result = result.Split('-').Last();
            }

            return result;
        }

    }
}