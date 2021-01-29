using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.Companies;

//using Vulcan.IMetal.Queries.Companies;

namespace Vulcan.IMetal.Tests.QueryCompanyTest
{
    [TestFixture]
    public class QueryCompanyTesting
    {


        [Test]
        public void BasicTest()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var query = new QueryCompany("EUR");
            stopwatch.Stop();
            
            var companySearchResult = query.GetForCode("00020");
            //Console.WriteLine(ObjectDumper.Dump(companySearchResult));
            Console.WriteLine(ObjectDumper.Dump(companySearchResult));
        }

        [Test]
        public void BasicTestMSA()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var query = new QueryCompany("MSA");
            stopwatch.Stop();

            var companySearchResult = query.GetForCode("00047");
            //Console.WriteLine(ObjectDumper.Dump(companySearchResult));
            Console.WriteLine(ObjectDumper.Dump(companySearchResult));
        }



        [Test]
        public void PaymentTermsTest()
        {
            var query = new QueryCompany("EUR");
            var helperCompanyPaymentTerms = new HelperCompanyPaymentTerms();
            query.Code.Equals("02408");
            var result = query.Execute();
            foreach (var companySearchResult in result)
            {

                var terms = helperCompanyPaymentTerms.GetPaymentTermsForCompany(companySearchResult.Coid,
                    companySearchResult.Id);
                Console.WriteLine(ObjectDumper.Dump(terms));


            }
        }

        [Test]
        public void FindCompanyTest()
        {
            var query = new QueryCompany("INC");
            var helperCompanyPaymentTerms = new HelperCompanyPaymentTerms();
            query.Code.Equals("02000");
            var result = query.Execute();
            foreach (var companySearchResult in result)
            {

                Console.WriteLine(companySearchResult.DefaultSalesGroupCode);
                Console.WriteLine(companySearchResult.CurrencyCode);


            }
        }

        [Test]
        public void FindCompanyTestEurope()
        {
            var query = new QueryCompany("EUR");
            query.Code.Equals("00144");
            var result = query.Execute();
            foreach (var companySearchResult in result)
            {

                Console.WriteLine(companySearchResult.DefaultSalesGroupCode);
                Console.WriteLine(companySearchResult.CurrencyCode);
            }
        }

        [Test]
        public void FindCompanyTestDubai()
        {
            var query = new QueryCompany("DUB");
            query.Code.Equals("00030");
            var result = query.Execute().First();
            Console.WriteLine(result.Name);
            
            query.Code.Equals("00034");
            result = query.Execute().First();
            Console.WriteLine(result.Name);
        }


    }
}
