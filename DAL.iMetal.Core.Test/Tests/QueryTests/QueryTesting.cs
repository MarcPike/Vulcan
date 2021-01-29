using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.iMetal.Core.Context;
using DAL.iMetal.Core.DbUtilities;
using DAL.iMetal.Core.Helpers;
using DAL.iMetal.Core.Models;
using DAL.iMetal.Core.Queries;
using DAL.Vulcan.Mongo.Base.Core.Static_Helper_Classes;
using NUnit.Framework;

namespace DAL.iMetal.Core.Test.Tests.QueryTests
{
    [TestFixture]
    class QueryTesting
    {

        [SetUp]
        public void SetUp()
        {
            ConnectionFactory.Initialize();

        }

        [Test]
        public void QuickStringInterpolationTest()
        {
            string value = "String goes here";
            string txt1 = $"{value,60}";
            string txt2 = $"{value,-60}";
            Console.WriteLine($"[{txt1}]");
            Console.WriteLine($"[{txt2}]");
        }

        [Test]
        public void CompanyTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var companies = GetCompaniesAsync().Result;
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} {companies.Count} rows");
            foreach (var companyTestModel in companies.OrderBy(x=>x.name))
            {
                Console.WriteLine($"{companyTestModel.id} {companyTestModel.code} - {companyTestModel.name}");
            }

        }

        [Test]
        public void ProductMastersQueryProductCategories()
        {
            var sw = new Stopwatch();
            sw.Start();
            var cats = ProductMastersQuery.GetAllProductCategories("INC");
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} {cats.Count} rows");
            foreach (var cat in cats)
            {
                Console.WriteLine(cat);
            }
        }

        [Test]
        public void ProductMastersQueryProductConditions()
        {
            var sw = new Stopwatch();
            sw.Start();
            var conditions = ProductMastersQuery.GetAllProductConditions("INC");
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} {conditions.Count} rows");
            foreach (var condition in conditions)
            {
                Console.WriteLine(condition);
            }
        }

        [Test]
        public void WarehouseQueryTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var warehouses = WarehouseQuery.ExecuteAsync("INC").Result;
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} {warehouses.Count} rows");
            foreach (var warehouse in warehouses)
            {
                Console.WriteLine(ObjectDumper.Dump(warehouse));
            }
        }


        [Test]
        public void SalesGroupQueryTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var salesGroups = SalesGroupQuery.ExecuteAsync("INC").Result;
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} {salesGroups.Count} rows");
            foreach (var salesGroup in salesGroups)
            {
                Console.WriteLine(ObjectDumper.Dump(salesGroup));
            }
        }

        [Test]
        public void StockItemsQueryTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var stockItems = StockItemsQuery.ExecuteAsync("INC").Result;
            sw.Stop();

            var allocatedLength = stockItems.Count(x => x.AllocatedLength > 0);
            var allocatedWeight = stockItems.Count(x => x.AllocatedWeight > 0);
            var availableLength = stockItems.Count(x => x.AvailableLength > 0);
            var availableQuantity= stockItems.Count(x => x.AvailableQuantity > 0);

            Console.WriteLine($"{sw.Elapsed} {stockItems.Count} rows");
        }

        [Test]
        public void IncomingQueryTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var incoming = PurchaseOrderItemsQuery.ExecuteAsync("INC").Result;
            sw.Stop();


            Console.WriteLine($"{sw.Elapsed} {incoming.Count} rows");
        }

        [Test]
        public void ProductMastersQueryTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var product = ProductMastersQuery.GetForId("CAN", 5626374);
            sw.Stop();
            Console.WriteLine(ObjectDumper.Dump(product));
        }

        [Test]
        public void CompanyQueryTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var company = CompanyQuery.GetForCode("INC", "00015", true).Result;
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed}");
            Console.WriteLine(ObjectDumper.Dump(company));

            //sw.Start();
            //var creditRule = company.GetCreditRule().Result;
            //sw.Stop();

            //Console.WriteLine($"{sw.Elapsed}");
            //Console.WriteLine(ObjectDumper.Dump(creditRule));

            //sw.Start();
            //var contacts = company.GetContacts().Result;
            //sw.Stop();

            //Console.WriteLine($"{sw.Elapsed} Rows: {contacts.Count}");
            //foreach (var iMetalContactModel in contacts)
            //{
            //    Console.WriteLine(ObjectDumper.Dump(iMetalContactModel));
            //}
        }

        [Test]
        public void CompanyContactsQueryTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var companyContacts = CompanyQuery.GetCompanyContacts("INC", "02000").Result;
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} Rows: {companyContacts.Count}");
            foreach (var iMetalContactModel in companyContacts)
            {
                Console.WriteLine(ObjectDumper.Dump(iMetalContactModel));
            }
        }

        [Test]
        public void GetFreightTermsTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var freightTerms = CompanyQuery.GetFreightTerms("INC").Result;
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} Rows: {freightTerms.Count}");
            foreach (var iMetalContactModel in freightTerms)
            {
                Console.WriteLine(ObjectDumper.Dump(iMetalContactModel));
            }
        }

        [Test]
        public void GetCompanyCurrencyCodeTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var currency = CompanyQuery.GetCompanyCurrencyCode("INC", "02000").Result;
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed}");
            Console.WriteLine(ObjectDumper.Dump(currency));
        }

        [Test]
        public void GetPaymentTermsTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var paymentTerms = CompanyQuery.GetPaymentTerms("INC").Result;
            sw.Stop();

            Console.WriteLine($"{sw.Elapsed} Rows: {paymentTerms.Count}");
            foreach (var iMetalContactModel in paymentTerms)
            {
                Console.WriteLine(ObjectDumper.Dump(iMetalContactModel));
            }
        }

        [Test]
        public void CompaniesQueryTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var companies = CompanyQuery.GetAllCompaniesForCoid("INC", false).Result;
            sw.Stop();


            Console.WriteLine($"{sw.Elapsed} Rows: {companies.Count}");
        }

        [Test]
        public void GetCompanyCreditRuleResultTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var creditRules = CompanyQuery.GetCompanyCreditRuleResult("INC", "02000").Result;
            sw.Stop();


            Console.WriteLine($"{sw.Elapsed}");
            Console.WriteLine(ObjectDumper.Dump(creditRules));

        }

        [Test]
        public void GetInvoicesTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var invoices = CompanyQuery.GetInvoices("INC", "02000").Result.ToList();
            sw.Stop();


            Console.WriteLine($"{sw.Elapsed} Rows: {invoices.Count}");
            foreach (var iMetalInvoice in invoices)
            {
                Console.WriteLine(ObjectDumper.Dump(iMetalInvoice));
            }

        }

        [Test]
        public void GetCompanyCreditTotalResultsTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var creditTotals = CompanyQuery.GetCompanyCreditTotalResults("INC", "02000").Result;
            sw.Stop();


            Console.WriteLine($"{sw.Elapsed}");
            Console.WriteLine(ObjectDumper.Dump(creditTotals));

        }

        [Test]
        public void GetTotalOpenCompletedAndLargestTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var creditTotals = CompanyQuery.GetTotalOpenCompletedAndLargest("INC", "02000").Result;
            sw.Stop();


            Console.WriteLine($"{sw.Elapsed}");
            Console.WriteLine(ObjectDumper.Dump(creditTotals));

        }

        [Test]
        public void ExchangeRateTesting()
        {
            var sw = new Stopwatch();
            var helperCurrency = new HelperCurrencyForIMetal();
            
            sw.Start();
            var exchangeRatesInc = helperCurrency.GetExchangeRatesFromCoid("INC");
            sw.Stop();


            Console.WriteLine($"INC {sw.Elapsed}");
            Console.WriteLine($"USD:{exchangeRatesInc.USD} EUR:{exchangeRatesInc.EUR} GBP:{exchangeRatesInc.GBP}");

            sw.Start();
            var exchangeRatesEur = helperCurrency.GetExchangeRatesFromCoid("EUR");
            sw.Stop();

            Console.WriteLine($"EUR {sw.Elapsed}");
            Console.WriteLine($"USD:{exchangeRatesEur.USD} EUR:{exchangeRatesEur.EUR} GBP:{exchangeRatesEur.GBP}");

            Console.WriteLine();
            Console.WriteLine("2nd time");
            Console.WriteLine();

            sw.Start();
            exchangeRatesInc = helperCurrency.GetExchangeRatesFromCoid("INC");
            sw.Stop();


            Console.WriteLine($"INC {sw.Elapsed}");
            Console.WriteLine($"USD:{exchangeRatesInc.USD} EUR:{exchangeRatesInc.EUR} GBP:{exchangeRatesInc.GBP}");

            sw.Start();
            exchangeRatesEur = helperCurrency.GetExchangeRatesFromCoid("EUR");
            sw.Stop();

            Console.WriteLine($"EUR {sw.Elapsed}");
            Console.WriteLine($"USD:{exchangeRatesEur.USD} EUR:{exchangeRatesEur.EUR} GBP:{exchangeRatesEur.GBP}");


        }


        [Test]
        public void ExchangeRateTest()
        {
            //var gbp = new SqlQueryDynamic("INC", null);

            //    $"select exchange_rate from currency_codes where symbol ='GBP'").Results.First().exchange_rate;
            //Console.WriteLine(gbp);

        }

        [Test]
        public void LookForTag()
        {
            var stockItem = StockItemsQuery.GetForTag("INC", "1310779");
            Console.WriteLine(ObjectDumper.Dump(stockItem));
        }


        public async Task<List<dynamic>> GetCompaniesAsync()
        {
            return await CompanyTestQuery.ExecuteDynamicAsync("INC");

        }

    }
}
