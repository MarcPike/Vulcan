using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.IntegrationDb;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Vulcan.iMetal.Quote.Export;
using Vulcan.iMetal.Quote.Export.Model;
using Vulcan.iMetal.Quote.Export.Repository;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace Vulcan.IMetal.Quote.Export.Test
{
    [TestFixture]
    public class ExportQuoteTests
    {
        private void SetContactToMarcPike(CrmQuote crmQuote)
        {
            if (crmQuote.Contact.FirstName != "Marc")
            {
                var contact = new Contact()
                {
                    Companies = new ReferenceList<Company, CompanyRef>()
                    {
                        crmQuote.Company
                    },
                    Person = new Person()
                    {
                        FirstName = "Marc",
                        LastName = "Pike",
                        Addresses = new List<Address>()
                        {
                            new Address()
                            {
                                AddressLine1 = "10626 Archmont Dr.",
                                City = "Houston",
                                StateProvince = "TX",
                                PostalCode = "77070",
                                Country = "USA",
                                County = "Harris",
                                Type = AddressType.Home
                            },
                            new Address()
                            {
                                AddressLine1 = "9611 Telge",
                                City = "Houston",
                                StateProvince = "TX",
                                PostalCode = "77095",
                                Country = "USA",
                                County = "Harris",
                                Type = AddressType.Office
                            },
                        },
                        EmailAddresses = new List<EmailAddress>()
                        {
                            new EmailAddress()
                            {
                                Type = EmailType.Business,
                                Address = "marc.pike@howcogroup.com"
                            },
                            new EmailAddress()
                            {
                                Type = EmailType.Personal,
                                Address = "marc.pike@gmail.com"
                            }
                        },
                        PhoneNumbers = new List<PhoneNumber>()
                        {
                            new PhoneNumber()
                            {
                                Type = PhoneType.Mobile,
                                Number = "1-713-292-4278"
                            },
                            new PhoneNumber()
                            {
                                Type = PhoneType.Office,
                                Number = "1-281-649-8954"
                            }
                        }
                    },
                    CreatedByUserId = crmQuote.CreatedByUserId
                };
                new RepositoryBase<Contact>().Upsert(contact);
                crmQuote.Contact = contact.AsContactRef();
            }
        }

        [Test]
        public void ExportQuoteToIMetal()
        {

            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var crmQuote = new RepositoryBase<CrmQuote>().AsQueryable().SingleOrDefault(x => x.QuoteId == 14001);
            Assert.IsNotNull(crmQuote);

            SetContactToMarcPike(crmQuote);

            Assert.IsTrue(crmQuote.Contact.FirstName == "Marc");

            var importCompanyReference = "HOUQC";
            using (var context = new IntegrationDb())
            {
                var exporter = new QuoteExporter();
                exporter.Export(crmQuote, true, true);
            }

            import_sales_headers salesHeader;
            using (var ctx = new IntegrationDb())
            {
                salesHeader = ctx.import_sales_headers
                    .Where(x => x.import_company_reference == importCompanyReference &&
                                x.import_number == crmQuote.QuoteId).OrderByDescending(x => x.import_batch_number)
                    .FirstOrDefault();
            }

            Assert.IsNotNull(salesHeader);

            Console.WriteLine("Initial [import_sales_headers] values for dates");
            Console.WriteLine(
                $"QuoteId: {crmQuote.QuoteId.ToString()} | Sale Date: {salesHeader.sale_date}");

            while (salesHeader.import_status == "E")
            {
                Thread.Sleep(1000);

                using (var ctx = new IntegrationDb())
                {
                    salesHeader = ctx.import_sales_headers
                        .Where(x => x.import_company_reference == importCompanyReference &&
                                    x.import_number == crmQuote.QuoteId).OrderByDescending(x => x.import_batch_number)
                        .FirstOrDefault();
                }

                Assert.IsNotNull(salesHeader);
            }

            Console.WriteLine("[import_sales_headers] values for dates after process completes");
            Console.WriteLine(
                $"QuoteId: {crmQuote.QuoteId.ToString()} | Sale Date: {salesHeader.sale_date}");


        }

        [Test]
        public void GetQuoteExportAttemptResultsModel()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var quote = new RepositoryBase<CrmQuote>().AsQueryable().SingleOrDefault(x => x.QuoteId == 14345);
            Assert.IsNotNull(quote);
            var model = new QuoteExportAttempResultsModel(quote);
            Console.WriteLine(ObjectDumper.Dump(model));
        }

        [Test]
        public void LookForQuotesWithPrdCharges()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var exportedQuotes = new RepositoryBase<CrmQuote>().AsQueryable()
                .Where(x => x.ExportStatus == ExportStatus.Success).OrderByDescending(x => x.WonDate).ToList();
            var quotesToLookAt = new List<CrmQuote>();
            var quotesWithProblems = new List<CrmQuote>();
            foreach (var exportedQuote in exportedQuotes)
            {
                foreach (var crmQuoteItem in exportedQuote.Items.Where(x => x.IsQuickQuoteItem == false)
                    .Select(x => x.AsQuoteItem()).ToList())
                {
                    if (crmQuoteItem.CalculateQuotePriceModel.ProductionCosts.Any())
                    {
                        quotesToLookAt.Add(exportedQuote);
                        break;
                    }
                }
            }

            using (var context = new IntegrationDb())
            {

                foreach (var crmQuote in quotesToLookAt)
                {
                    var exportAttempt = crmQuote.ExportAttempts.Last();
                    //Console.WriteLine($"Batch#: {exportAttempt.ImportBatchNumber} SalesOrderId: {exportAttempt.SalesOrderId} ExportDate: {exportAttempt.ExecutionDate}");
                    //Console.WriteLine($"{exportAttempt.ImportBatchNumber},");

                    if (context.import_sales_charges.Any(x => x.charge_unit_code != "MAT"))
                    {
                        quotesWithProblems.Add(crmQuote);
                    }
                }
            }

            foreach (var quotesWithProblem in quotesWithProblems)
            {
                Console.WriteLine(quotesWithProblem.QuoteId);
            }

        }


    }
}
